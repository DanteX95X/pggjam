using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
	public class MoveAction : Action
	{
		Vector2 source;
		Vector2 destination;

		List<int> indexes = new List<int>();
		List<Vector2> positions = new List<Vector2>();
		List<int> players = new List<int>();
		List<int> playersIndexes = new List<int>();

		public MoveAction(Vector2 destination)
			: base(ActionType.MOVE)
		{
			this.destination = destination;
		}

		public override void ApplyAction(GameState state)
		{
			source = state.SelectedPosition;
			for(int i = 0; i < state.Vessels[state.CurrentPlayer].Count; ++i)
			{
				Vector2 currentPosition = state.Vessels[state.CurrentPlayer][i];
				if(currentPosition == state.SelectedPosition)
				{
					//state.Winner = CheckWinConditions(state.Vessels[state.CurrentPlayer], state.Vessels[(state.CurrentPlayer+1)%2], i, state.CurrentPlayer);
					TakeShipOver(i, state.CurrentPlayer, state.Vessels);

					state.Vessels[state.CurrentPlayer][i] = destination;
					state.SelectedPosition = Game.noInput;
					break;
				}
			}

			for(int i = 0; i < indexes.Count; ++i)
			{
				//Debug.Log("" + indexes.Count + " " + positions.Count + " " + players.Count + " " + playersIndexes.Count);
				//Debug.Log("" + players[0] + " " + players[1]);
				state.Vessels[state.CurrentPlayer].Insert(indexes[i], positions[i]);
				for(int j = 0; j < indexes.Count; ++j)
				{
					++indexes[j];
				}
				state.Vessels[players[i]].RemoveAt(playersIndexes[i]);
				for(int j = 0; j < playersIndexes.Count; ++j)
				{
					--playersIndexes[j];
				}
			}
			state.Print();
		}

		public override void ApplyAction(Game game)
		{
			foreach(Vessel vessel in game.Ships)
			{
				if((Vector2)vessel.transform.position == source /*game.State.SelectedPosition*/)
				{
					game.moveShip(destination);

					for(int i = 0; i < positions.Count; ++i)
					{
						for(int j = 0; j < game.Ships.Count; ++j)
						{
							if((Vector2)game.Ships[j].transform.position == positions[i])
							{
								game.Ships[j].Owner = game.CurrentPlayer;
							}
						}
					}
					break;
					//vessel.transform.position = new Vector3(destination.x, destination.y, vessel.transform.position.z);
				}
			}
            game.SetupShipLines();
		}

		public override bool IsLegal(GameState state)
		{
			return state.Nodes[state.SelectedPosition].Contains(destination) && !state.Vessels[state.CurrentPlayer].Contains(destination) && !state.Vessels[(state.CurrentPlayer+1)%2].Contains(destination);
		}

		public override void Print()
		{
			Debug.Log("Move to " + destination);
		}

		void TakeShipOver(int index, int currentPlayer, List<Vector2>[] ships)
		{

			if (index > 0)
			{
				for (int i = 0; i < ships.Length; ++i)
				{
					if (i == currentPlayer)
						continue;
					for (int j = 0; j < ships[i].Count; ++j)
					{
						if (Utilities.isPointInTriangle(ships[currentPlayer][index - 1], source, destination, ships[i][j]))
						{
							Debug.Log(ships[i][j]);
							if (!positions.Contains(ships[i][j]))
							{
								Debug.Log("Taken over");
								indexes.Add(index);
								positions.Add(ships[i][j]);
								players.Add(i);
								playersIndexes.Add(j);
							}
						}
					}
				}
			}
			if (index < ships[currentPlayer].Count - 1)
			{
				for (int i = 0; i < ships.Length; ++i)
				{
					if (i == currentPlayer)
						continue;
					for (int j = 0; j < ships[i].Count; ++j)
					{
						if (Utilities.isPointInTriangle(source, ships[currentPlayer][index + 1], destination, ships[i][j]))
						{
							Debug.Log(ships[i][j]);
							if (!positions.Contains(ships[i][j]))
							{
								Debug.Log("Taken over");
								indexes.Add(index + 1);
								positions.Add(ships[i][j]);
								players.Add(i);
								playersIndexes.Add(j);
							}
						}
					}
				}
			}
		}

		/*int CheckWinConditions(List<Vector2> playerShips, List<Vector2> opponentShips, int index, int currentPlayer)
		{
			int winner = -1;
			bool result = false;
			for(int i = 0; i < opponentShips.Count; ++i)
			{
				if(index > 0)
				{
					result = Utilities.isPointInTriangle(playerShips[index-1], source, destination, opponentShips[i]);
					Debug.Log("angle: " + Utilities.angleBetweenVectors(new Vector2(0,1), new Vector2(0,-1) ));
					Debug.Log("angle: " + Utilities.angleBetweenVectors(source - playerShips[index-1], destination - playerShips[index-1]));
					float angle = Utilities.angleBetweenVectors(source - playerShips[index-1], destination - playerShips[index-1]);
					if(result && angle < CheckLossConditions(source, i, opponentShips))
					{
						winner = currentPlayer;
						break;
					}
					else
					{
						winner = (currentPlayer + 1)%2;
						break;
					}
					
				}
				if(index < playerShips.Count -1)
				{
					result = Utilities.isPointInTriangle(source, playerShips[index+1], destination, opponentShips[i]);
					Debug.Log("angle: " + (Utilities.angleBetweenVectors(source - playerShips[index+1], destination - playerShips[index+1] )));
					float angle = Utilities.angleBetweenVectors(source - playerShips[index+1], destination - playerShips[index+1] );
					if(result && angle < CheckLossConditions(source, i, opponentShips))
					{
						winner = currentPlayer;
						break;
					}
					else
					{
						winner = (currentPlayer + 1)%2;
						break;
					}
				}
			}
			return winner;
		}

		float CheckLossConditions(Vector2 source, int opponentShipIndex, List<Vector2> opponentShips)
		{
			if(opponentShipIndex > 0)
			{
				Debug.Log("    " + Utilities.angleBetweenVectors(source - opponentShips[opponentShipIndex], opponentShips[opponentShipIndex-1] - opponentShips[opponentShipIndex] ));
				return Utilities.angleBetweenVectors(source - opponentShips[opponentShipIndex], opponentShips[opponentShipIndex-1] - opponentShips[opponentShipIndex] );
			}
			if(opponentShipIndex < opponentShips.Count - 1)
			{
				Debug.Log("    " + Utilities.angleBetweenVectors(source - opponentShips[opponentShipIndex], opponentShips[opponentShipIndex+1] - opponentShips[opponentShipIndex] ));
				return Utilities.angleBetweenVectors(source - opponentShips[opponentShipIndex], opponentShips[opponentShipIndex+1] - opponentShips[opponentShipIndex] );
			}
			return 360;
		}*/
	}
}