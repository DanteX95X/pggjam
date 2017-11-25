using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour 
{
	[SerializeField]
	Transform grid = null;

	[SerializeField]
	Transform vessels = null;

	[SerializeField]
	int currentPlayer = -1;

    [SerializeField]
    int currentShip = 0;

    List<Node> nodes = new List<Node>();
	List<Vessel> ships = new List<Vessel>();


	void Start() 
	{
		foreach(Transform child in grid)
		{
			nodes.Add(child.gameObject.GetComponent<Node>());
		}

		foreach(Transform child in vessels)
		{
			ships.Add(child.gameObject.GetComponent<Vessel>());
		}

		currentPlayer = 0;

		CreateState().Serialize();
	}

	Model.GameState CreateState()
	{
		List<Vector2> playerVessels = new List<Vector2>();
		List<Vector2> opponentVessels = new List<Vector2>();

		Dictionary<Vector2, List<Vector2>> map = new Dictionary<Vector2, List<Vector2>>();
		foreach(Node node in nodes)
		{
			List<Vector2> neighbours = new List<Vector2>();
			foreach(Node neighbour in node.Neighbours)
			{
				neighbours.Add(neighbour.transform.position);
			}
		}

		foreach(Vessel vessel in ships)
		{
			playerVessels.Add(vessel.transform.position);
		}

		return new Model.GameState(playerVessels, opponentVessels, currentPlayer, map);
	}

    public void moveShip(Vector2 pos)
    {
        //Check if can move to that node


        StartCoroutine(MovePlayer(Time.time, Vector3.Distance(ships[currentShip].transform.position, new Vector3(pos.x, pos.y, 0)), ships[currentShip].transform.position, new Vector3(pos.x, pos.y, 0)));
        //ships[currentShip].transform.position = new Vector3(pos.x, pos.y, 0);
    }


    IEnumerator MovePlayer(float startTime, float journeyLength, Vector3 startPos, Vector3 destpos)
    {
        while (ships[currentShip].transform.position != destpos)
        {
            float distCovered = (Time.time - startTime) * ships[currentShip].speed * Time.deltaTime;
            float fracJourney = distCovered / journeyLength;
            ships[currentShip].transform.position = Vector3.Lerp(startPos, destpos, fracJourney);
            yield return null;
        }
        ShipMovementEnd();
    }

    void ShipMovementEnd()
    {
        Debug.Log("ShipMovementEnd");
    }

	void Update() 
	{
		
	}
}
