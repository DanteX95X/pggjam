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

		public Dictionary<Vector2, List<Vector2>> Nodes
		{
			get { return nodes; }
		}

		/*public GameState()
		{
		}*/

		public GameState(List<Vector2> playerVessels, List<Vector2> opponentVessels, List<Vector2> neutral, int currentPlayer, Dictionary<Vector2, List<Vector2>> nodes, Vector2 selectedPosition, int winner = -1)
		{
			vessels = new List<Vector2>[3] {playerVessels, opponentVessels, neutral};
			this.currentPlayer = currentPlayer;
			this.nodes = nodes;
			this.winner = winner;
			this.selectedPosition = selectedPosition;
		}

		public GameState Clone()
		{
				return new GameState(new List<Vector2>(vessels[0]), new List<Vector2>(vessels[1]), new List<Vector2>(vessels[2]), currentPlayer, nodes, selectedPosition, winner);
		}

		public List<Action> GenerateActions()
		{
			List<Action> actions = new List<Action>();
			if (SelectedPosition == Game.noInput)
			{
				foreach (Vector2 position in vessels[currentPlayer])
				{
					bool isPossible = false;
					foreach(Vector2 neighbour in nodes[position])
					{
						bool canAdd = true;
						for(int i = 0; i < vessels.Length; ++i)
						{
							for(int j = 0; j < vessels[i].Count; ++j)
							{
								if(vessels[i][j] == neighbour)
									canAdd = false;
							}
						}
						isPossible = isPossible || canAdd;
					}
					if(isPossible)
						actions.Add(new Model.SelectShipAction(position));
				}
			}
			else
			{
				foreach(Vector2 position in nodes[selectedPosition])
				{
					bool canAdd = true;
					for(int i = 0; i < vessels.Length; ++i)
					{
						for(int j = 0; j < vessels[i].Count; ++j)
						{
							if(vessels[i][j] == position)
								canAdd = false;
						}
					}
					if(canAdd)
						actions.Add(new Model.MoveAction(position));
				}
			}

			return actions;
		}

		public int WhoWon()
		{
			int winner = -1;
			if(vessels[0].Count < 2)
				winner = 1;
			else if(vessels[1].Count < 2)
				winner = 0;
			return winner;
		}

		public int Payoff()
		{
			return vessels[0].Count - vessels[1].Count;
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

		public void NextTurn()
		{
			//Debug.Log("next turn");
			currentPlayer = (currentPlayer + 1)%2;
		}
	}
}