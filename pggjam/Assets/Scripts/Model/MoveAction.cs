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

		int playerID;

		public MoveAction(Vector2 destination)
			: base(ActionType.MOVE)
		{
			this.destination = destination;
			playerID = -1;
		}

		public override void ApplyAction(GameState state)
		{
			source = state.SelectedPosition;
			for(int i = 0; i < state.Vessels[state.CurrentPlayer].Count; ++i)
			{
				Vector2 currentPosition = state.Vessels[state.CurrentPlayer][i];
				if(currentPosition == state.SelectedPosition)
				{
					TakeShipOver(state, i, state.CurrentPlayer, state.Vessels);

					state.Vessels[state.CurrentPlayer][i] = destination;
					state.SelectedPosition = Game.noInput;
					break;
				}
			}

			for(int i = 0; i < indexes.Count; ++i)
			{
				state.Vessels[state.CurrentPlayer].Insert(indexes[i], positions[i]);
				for(int j = i; j < indexes.Count; ++j)
				{
					++indexes[j];
					//++playersIndexes[j];
				}

				for(int j = i; j < playersIndexes.Count; ++j)
				{
					--playersIndexes[j];
					//--indexes[j];
				}
			}
			for(int i = 0; i < playersIndexes.Count; ++i)
			{
				/*state.Vessels[players[i]].RemoveAt(playersIndexes[i]);
				for(int j = i; j < playersIndexes.Count; ++j)
				{
					--playersIndexes[j];
				}*/
				state.Vessels[players[i]].Remove(positions[i]);
			}

			indexes.Clear();
			positions.Clear();
			players.Clear();
			playersIndexes.Clear();

			playerID = state.CurrentPlayer;
			state.NextTurn();
		}

		public override void ApplyAction(Game game)
		{
			Vessel ship = game.Ships[game.CurrentShip];
    		game.Ships.Remove(game.CurrentShip);
    		game.Ships[destination] = ship;
    		game.CurrentShip = destination;

			game.moveShip(destination);

			for(int i = 0; i < positions.Count; ++i)
			{
				try
				{
					game.Ships[positions[i]].Owner = playerID;
				}
				catch(System.Exception)
				{
					Debug.Log("Not ok");
				}
			}
		}

		public override bool IsLegal(GameState state)
		{
			return state.Nodes.ContainsKey(state.SelectedPosition) && state.Nodes[state.SelectedPosition].Contains(destination) && !state.Vessels[state.CurrentPlayer].Contains(destination) && !state.Vessels[(state.CurrentPlayer+1)%2].Contains(destination);
		}

		public override void Print()
		{
			Debug.Log("Move to " + destination);
		}

		public override Vector2 TargetPosition()
		{
			return destination;
		}

		void TakeShipOver(Model.GameState state, int index, int currentPlayer, List<Vector2>[] ships)
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
							if (!positions.Contains(ships[i][j]))
							{
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
							if (!positions.Contains(ships[i][j]))
							{
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
	}
}