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
		Vector2 selectedPosition;

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

		public Vector2 SelectedPosition
		{
			get { return selectedPosition; }
			set { selectedPosition = value;}
		}

		public GameState()
		{
		}

		public GameState(List<Vector2> playerVessels, List<Vector2> opponentVessels, int currentPlayer, Dictionary<Vector2, List<Vector2>> nodes, Vector2 selectedPosition, int winner = -1)
		{
			vessels = new List<Vector2>[2] {playerVessels, opponentVessels};
			this.currentPlayer = currentPlayer;
			this.nodes = nodes;
			this.winner = winner;
			this.selectedPosition = selectedPosition;
		}

		public GameState Clone()
		{
				return new GameState(new List<Vector2>(vessels[0]), new List<Vector2>(vessels[1]), currentPlayer, nodes, selectedPosition, winner);
		}

		public List<Action> GenerateActions()
		{
			List<Action> actions = new List<Action>();
			if (SelectedPosition != Game.noInput)
			{
				foreach (Vector2 position in vessels[currentPlayer])
				{
					actions.Add(new Model.SelectShipAction(position));
					/*foreach (Vector2 neighbour in nodes[position])
						{
							actions.Add(new Model.MoveAction(position, neighbour));
						}*/
				}
			}
			else
			{
				foreach(Vector2 position in nodes[selectedPosition])
				{
					actions.Add(new Model.MoveAction(position));
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
		Debug.Log("ufo");
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