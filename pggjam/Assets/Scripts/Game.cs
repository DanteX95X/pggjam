using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Linq;
using System;

public class Game : MonoBehaviour 
{
	[SerializeField]
	Transform grid = null;

	[SerializeField]
	Transform vessels = null;

	[SerializeField]
	GameObject nodePrefab = null;

	[SerializeField]
	GameObject vesselPrefab = null;

	[SerializeField]
	List<LineRenderer> lines = null;

    List<Node> nodes = new List<Node>();
	Dictionary<Vector2, Vessel> ships = new Dictionary<Vector2, Vessel>();

	public Vector2 currentShip = noInput;

	Model.GameState state;

	bool isInCouroutine = false;

	public Model.GameState State
	{
		get { return state;}
	}

	public Dictionary<Vector2, Vessel> Ships
	{
		get { return ships; }
	}

	public Vector2 CurrentShip 
	{
		get { return currentShip; }
		set { currentShip = value; }
	}

	public int CurrentPlayer
	{
		get { return state.CurrentPlayer;}
	}

	public static readonly Vector2 noInput = new Vector2(int.MaxValue, int.MaxValue);
	Vector2 lastInput;// = noInput;

	public Vector2 Input
	{
		set{ lastInput = value;}
	}

	enum ControllerType
	{
		HUMAN,
		AI,
		RANDOM,
	}

	ControllerType[] controllers = new ControllerType[2];


	void ParseGraph(string name)
	{
		using (StreamReader reader = new StreamReader(name))
		{
			string line = reader.ReadLine();
			string[] words;

			int count = Int32.Parse(line);
			for(int i = 0; i < count; ++i)
			{
				line = reader.ReadLine();
				words = line.Split();
				Vector3 position = new Vector3(float.Parse(words[0]), float.Parse(words[1]), 0);
				GameObject instance = Instantiate(nodePrefab, position, Quaternion.identity);
				instance.transform.parent = grid;
				nodes.Add(instance.GetComponent<Node>());
			}

			//Debug.Log(" nodes : " + nodes.Count);
			line = reader.ReadLine();
			line = reader.ReadLine();
			count = Int32.Parse(line);
			for(int i = 0; i < count; ++i)
			{
				line = reader.ReadLine();
				words = line.Split();
				int first = Int32.Parse(words[0]);
				int second = Int32.Parse(words[1]);

				nodes[first].Neighbours.Add(nodes[second]);
				nodes[second].Neighbours.Add(nodes[first]);
			}

			line = reader.ReadLine();
			line = reader.ReadLine();
			count = Int32.Parse(line);
			for(int i = 0; i < count; ++i)
			{
				line = reader.ReadLine();
				words = line.Split();
				int index = Int32.Parse(words[0]);
				int owner = Int32.Parse(words[1]);
				GameObject instance = Instantiate(vesselPrefab, nodes[index].transform.position - new Vector3(0,0,1), Quaternion.identity);
				instance.transform.parent = vessels;
				instance.GetComponent<Vessel>().Owner = owner;
				ships[(Vector2)instance.transform.position] = (instance.GetComponent<Vessel>());
			}
			 
		}
	}

	void ParseGraph(Dictionary<Vector2, List<Vector2>> map)
	{
		Dictionary<Vector2, Node> nodeMap = new Dictionary<Vector2, Node>();
		foreach(Vector2 position in map.Keys)
		{
			nodeMap[position] = Instantiate(nodePrefab, position, Quaternion.identity).GetComponent<Node>();
			nodeMap[position].transform.parent = grid;
		}

		foreach(Vector2 position in map.Keys)
		{
			//if(map[position].Count < 3) continue;

			foreach(Vector2 neighbour in map[position])
			{
				nodeMap[position].Neighbours.Add(nodeMap[neighbour]);
			}
		}
	}

	List<Vector2> GeneratePoints(float radius, int rangeX, int rangeY, int quantity)
	{
		//rangeX *= 10;
		//rangeY *= 10;
		List<Vector2> deltas = new List<Vector2>();
		for (int x = -rangeX; x <= rangeX; ++x)
		{
			for(int y = -rangeY; y <= rangeY; ++y)
			{
				if(x*x + y*y <= radius * radius)
				{
					deltas.Add(new Vector2(x,y));
				}
			}
		}

		List<Vector2> points = new List<Vector2>();
		HashSet<Vector2> excluded = new HashSet<Vector2>();
		for(int i = 0; i < quantity; ++i)
		{
			Vector2 point = new Vector2(UnityEngine.Random.Range(-rangeX, rangeX), UnityEngine.Random.Range(-rangeY, rangeY));
			if(excluded.Contains(point)) continue;

			points.Add(point);

			foreach(Vector2 delta in deltas)
			{
				excluded.Add(point + delta);
			}
		}
		return points;
	}


	void Start()
	{
		//List<Vector2> vector = new List<Vector2> {  ;
		Dictionary<Vector2, List<Vector2>> triangulation = GraphGenerator.Quickhull( GeneratePoints(1.0f, 10, 5, 100));

		ParseGraph(triangulation);

		//ParseGraph("ufo");
		lastInput = noInput;

		state = CreateState();

		state.Print();

        SetupShipLines();

        controllers[0] = ControllerType.HUMAN;
        controllers[1] = ControllerType.RANDOM;
	}

	Model.GameState CreateState()
	{
		List<Vector2>[] vesselsPositions = {new List<Vector2>(), new List<Vector2>()};

		Dictionary<Vector2, List<Vector2>> map = new Dictionary<Vector2, List<Vector2>>();
		foreach(Node node in nodes)
		{
			List<Vector2> neighbours = new List<Vector2>();
			foreach(Node neighbour in node.Neighbours)
			{
				neighbours.Add(neighbour.transform.position);
			}
			map[node.transform.position] = neighbours;
		}

		foreach(Vessel vessel in ships.Values)
		{
			vesselsPositions[vessel.Owner].Add(vessel.transform.position);
		}

		return new Model.GameState(vesselsPositions[0], vesselsPositions[1], 0, map, noInput);
	}

    public void moveShip(Vector2 pos)
    {
		StartCoroutine(MovePlayer(Time.time, Vector3.Distance(ships[currentShip].transform.position, new Vector3(pos.x, pos.y, ships[currentShip].transform.position.z)), ships[currentShip].transform.position, new Vector3(pos.x, pos.y, ships[currentShip].transform.position.z)));
    }


    IEnumerator MovePlayer(float startTime, float journeyLength, Vector3 startPos, Vector3 destpos)
    {
    	isInCouroutine = true;
        while ((ships[currentShip].transform.position - destpos).magnitude > 0.01)
        {
            float distCovered = (Time.time - startTime) * ships[currentShip].Speed * Time.deltaTime;
            float fracJourney = distCovered / journeyLength;
            ships[currentShip].transform.position = Vector3.Lerp(startPos, destpos, fracJourney);
            yield return null;
        }
        isInCouroutine = false;
        ships[currentShip].transform.position = destpos;
        ShipMovementEnd();
    }

    void ShipMovementEnd()
    {
    	currentShip = noInput;
        //Debug.Log("ShipMovementEnd");
    }

    public void SetupShipLines()
    {
        int numberTeams = 2;
        for (int i = 0; i < numberTeams; ++i)
        {
        	lines[i].positionCount = state.Vessels[i].Count;
        	for(int j = 0; j < state.Vessels[i].Count; ++j)
        	{
        		lines[i].SetPosition(j, ships[state.Vessels[i][j]].transform.position);
        	}
        }
    }

    public void AquireShip(Vector2 position)
	{
		currentShip = position;
		//Debug.Log("Ship aquired");
		return;
    }

	void Update()
	{
		if (!isInCouroutine)
		{
			if (state.WhoWon() == -1)
			{
				Model.Action action = null;
				switch(controllers[CurrentPlayer])
				{
					case ControllerType.HUMAN:
					{
						action = ProcessInput();
						break;
					}
					case ControllerType.AI:
					{
						StartCoroutine(Model.AlphaBeta.StartPruning(state, 1.0f));//state.GenerateActions()[0];//Model.AlphaBeta.StartPruning(state.Clone(), 1.0f);
						action = Model.AlphaBeta.ufo;
						break;
					}
					case ControllerType.RANDOM:
					{
						List<Model.Action> actions = state.GenerateActions();
						action = actions[UnityEngine.Random.Range(0, actions.Count)];
						break;
					}
				}
				if (action != null && action.IsLegal(state))
				{
					action.ApplyAction(state);
					action.ApplyAction(this);
					//action.Print();
					Model.AlphaBeta.ufo = null;
				}
			} 
			else
			{
				Debug.Log("Game Over " + state.WhoWon());
				//Time.timeScale = 0;
			}
		}

		SetupShipLines();
	}

	public Model.Action ProcessInput()
	{
		Model.Action action = null;
		if (lastInput != noInput)
		{
			if (currentShip == noInput)
			{
				action = new Model.SelectShipAction(lastInput);
			} 
			else
			{
				action = new Model.MoveAction(lastInput);
			}
		}

		lastInput = noInput;
		return action;
	}
}
