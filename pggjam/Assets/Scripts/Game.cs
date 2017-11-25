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

	//[SerializeField]
	//int currentPlayer = -1;

    //[SerializeField]
    //int currentShip = 0;

    List<Node> nodes = new List<Node>();
	List<Vessel> ships = new List<Vessel>();

	public int currentShip = -1;

	Model.GameState state;

	public Model.GameState State
	{
		get { return state;}
	}

	public List<Vessel> Ships
	{
		get { return ships; }
	}

	public int CurrentShip 
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
				Vector3 position = new Vector3(float.Parse(words[0]), float.Parse(words[1]), -1);
				GameObject instance = Instantiate(nodePrefab, position, Quaternion.identity);
				instance.transform.parent = grid;
				nodes.Add(instance.GetComponent<Node>());
			}

			Debug.Log(" nodes : " + nodes.Count);
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
				ships.Add(instance.GetComponent<Vessel>());
			}
			 
		}
	}


	void Start()
	{
		ParseGraph("ufo");
		lastInput = noInput;

		/*foreach (Transform child in grid)
		{
			nodes.Add(child.gameObject.GetComponent<Node>());
		}*/

		/*foreach (Transform child in vessels)
		{
			ships.Add(child.gameObject.GetComponent<Vessel>());
		}*/

		state = CreateState();

		state.Print();

        SetupShipLines();
	}

	Model.GameState CreateState()
	{
		List<Vector2>[] vesselsPositions = {new List<Vector2>(), new List<Vector2>()};
		//List<Vector2> opponentVessels = new List<Vector2>();

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

		foreach(Vessel vessel in ships)
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
        while ((ships[currentShip].transform.position - destpos).magnitude > 0.01)
        {
            float distCovered = (Time.time - startTime) * ships[currentShip].Speed * Time.deltaTime;
            float fracJourney = distCovered / journeyLength;
            ships[currentShip].transform.position = Vector3.Lerp(startPos, destpos, fracJourney);
            yield return null;
        }
        ShipMovementEnd();
    }

    void ShipMovementEnd()
    {
    	currentShip = -1;
        Debug.Log("ShipMovementEnd");
    }

    public void SetupShipLines()
    {
        int numberTeams = 2;
        for (int i = 0; i < numberTeams; i++)
        {
            Debug.Log("Next team!");
            for (int j = 0 ; j < state.Vessels[i].Count - 1; j++ )
            {
                Debug.Log("Current Ship : " + j);

                Vessel current = null;
                foreach (Vessel ves in ships)
                {
                    if (ves.transform.position.x == state.Vessels[i][j].x && ves.transform.position.y == state.Vessels[i][j].y)
                    {
                        current = ves;
                        break;
                    }
                }
                Vessel next = null;
                foreach (Vessel ves in ships)
                {
                    if (current != ves && ves.transform.position.x == state.Vessels[i][j+1].x && ves.transform.position.y == state.Vessels[i][j+1].y)
                    {
                        next = ves;
                        break;
                    }
                }
                Debug.Log("Current : " + (current != null).ToString() + ", Next : " + (next != null).ToString());
                if (current != null && next != null)
                    current.otherShip = next.gameObject;
            }   
        }
    }

    public void AquireShip(Vector2 position)
	{
		for (int i = 0; i < ships.Count; ++i)
		{
			if ((Vector2)ships[i].transform.position == position)// && ships[i].Owner == CurrentPlayer)
			{
				currentShip = i;
				Debug.Log("Ship aquired");
				return;
			}
    	}
    }

	void Update() 
	{
		if(state.WhoWon() == -1)
		{
			Model.Action  action = ProcessInput();
			if(action != null && action.IsLegal(state))
			{
				action.ApplyAction(state);
				action.ApplyAction(this);
			}
		}
		else
		{
			Debug.Log("Game Over " + state.WhoWon());
		}
	}

	Model.Action ProcessInput()
	{
		Model.Action action = null;
		if (lastInput != noInput)
		{
			if (currentShip == -1)
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
