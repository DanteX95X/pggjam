﻿using System.Collections;
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

	public List<Vessel> Ships
	{
		get { return ships; }
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

		currentPlayer = 0;

		Model.GameState state = CreateState();
		Model.GameState clone = state.Clone();
		state.Print();
		for (int i = 0; i < 0; ++i)
		{
			Model.Action action = state.GenerateActions()[0];
			action.ApplyAction(state);
			action.ApplyAction(this);
			state.Print();
		}

		Model.Action action0 = new Model.MoveAction(new Vector2(-3,-3), new Vector2(3,-3));
		action0.ApplyAction(state);
		state.Print();
		Debug.Log("Winner: " + state.WhoWon());

		clone.Print();

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

		return new Model.GameState(vesselsPositions[0], vesselsPositions[1], currentPlayer, map);
	}

	void Update() 
	{
		
	}
}
