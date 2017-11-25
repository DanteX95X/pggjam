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

	void Update() 
	{
		
	}
}
