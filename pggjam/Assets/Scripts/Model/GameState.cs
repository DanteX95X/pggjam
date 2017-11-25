using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
	public class GameState
	{
		List<Vector2>[] vessels;
		int currentPlayer;
		Dictionary<Vector2, List<Vector2>> nodes;
		int winner;

		public List<Vector2>[] Vessels
		{
			get { return vessels; }
		}

		public int CurrentPlayer
		{
			get { return currentPlayer; }
		}

		public int Winner 
		{
			get { return winner; }
			set { winner = value; }
		}

		public GameState()
		{
		}

		public GameState(List<Vector2> playerVessels, List<Vector2> opponentVessels, int currentPlayer, Dictionary<Vector2, List<Vector2>> nodes, int winner = -1)
		{
			vessels = new List<Vector2>[2] {playerVessels, opponentVessels};
			this.currentPlayer = currentPlayer;
			this.nodes = nodes;
			this.winner = winner;
		}

		public GameState Clone()
		{
			return new GameState(new List<Vector2>(vessels[0]), new List<Vector2>(vessels[1]), currentPlayer, nodes, winner);
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

		public int WhoWon()
		{
			return winner;
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