using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour 
{
	[SerializeField]
	Transform grid = null;

	[SerializeField]
	Transform vessels = null;

	//[SerializeField]
	//int currentPlayer = -1;

    //[SerializeField]
    //int currentShip = 0;

    List<Node> nodes = new List<Node>();
	List<Vessel> ships = new List<Vessel>();

	public int currentShip = -1;

	Model.GameState state;

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

	void Start()
	{
		foreach (Transform child in grid)
		{
			nodes.Add(child.gameObject.GetComponent<Node>());
		}

		foreach (Transform child in vessels)
		{
			ships.Add(child.gameObject.GetComponent<Vessel>());
		}

		state = CreateState();
		Model.Action action0 = new Model.MoveAction(new Vector2(0,3), new Vector2(3,-3));
		action0.ApplyAction(state);
		state.Print();
		Debug.Log("Winner: " + state.WhoWon());

		state = CreateState();

		Debug.Log(Model.Utilities.isPointInTriangle(new Vector2(0,1), new Vector2(-1,-1), new Vector2(1, -1), new Vector2(1,0)));
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

		return new Model.GameState(vesselsPositions[0], vesselsPositions[1], 0, map);
	}

    public void moveShip(Vector2 pos)
    {
        //Check if can move to that node
        Model.MoveAction move = new Model.MoveAction(ships[currentShip].transform.position, pos);
        if(!move.IsLegal(state))
        	return;

        move.ApplyAction(state);
        state.Print();
        //move.ApplyAction(this);

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

    public void AquireShip(Vector2 position)
	{
		for (int i = 0; i < ships.Count; ++i)
		{
			if ((Vector2)ships[i].transform.position == position)// && ships[i].Owner == CurrentPlayer)
			{
				currentShip = i;
				return;
			}
    	}
    }

	void Update() 
	{
		
	}
}
