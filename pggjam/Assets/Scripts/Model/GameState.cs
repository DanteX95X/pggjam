﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
	public class GameState
	{
		List<Vector2>[] vessels;
		int currentPlayer;
		Dictionary<Vector2, List<Vector2>> nodes;

		public List<Vector2>[] Vessels
		{
			get { return vessels; }
		}

		public int CurrentPlayer
		{
			get { return currentPlayer; }
		}

		public GameState(List<Vector2> playerVessels, List<Vector2> opponentVessels, int currentPlayer, Dictionary<Vector2, List<Vector2>> nodes)
		{
			vessels = new List<Vector2>[2] {playerVessels, opponentVessels};
			this.currentPlayer = currentPlayer;
			this.nodes = nodes;
		}

		public GameState Clone()
		{
			return new GameState(vessels[0], vessels[1], currentPlayer, nodes);
		}

		public List<Action> GenerateActions()
		{
			List<Action> actions = new List<Action>();
			foreach(Vector2 position in vessels[currentPlayer])
			{
				foreach(Vector2 neighbour in nodes[position])
				{
					actions.Add(new Model.MoveAction(position, neighbour));
				}
			}

			return actions;
		}

		int WhoWon()
		{
			//TODO
			return -1;
		}

		public void Print()
		{
			string serializedState = "";
			serializedState += currentPlayer + "\n";
			for(int i = 0; i < vessels.Length; ++i)
			{
				serializedState += " " + i + ": ";
				foreach(Vector2 position in vessels[i] )
				{
					serializedState += position + " ";
				}
				serializedState += "\n";
			}
			Debug.Log(serializedState);
		}
	}
}