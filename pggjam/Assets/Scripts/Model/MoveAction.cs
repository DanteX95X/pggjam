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
				}
			}
			for(int i = 0; i < positions.Count; ++i)
			{
				state.Vessels[players[i]].Remove(positions[i]);
			}

			indexes.Clear();
			positions.Clear();
			players.Clear();

			playerID = state.CurrentPlayer;
			state.NextTurn();
		}

		public override void ApplyAction(Game game)
		{
			game.MoveSelection(destination, false);
			Vessel ship = game.Ships[game.CurrentShip];
    		game.Ships.Remove(game.CurrentShip);
    		game.Ships[destination] = ship;
    		game.CurrentShip = destination;

			game.moveShip(destination);

			for(int i = 0; i < positions.Count; ++i)
			{
				game.Ships[positions[i]].Owner = playerID;
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
			List<float> ufo = new List<float>();
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
								int k = 0;
								float value = 0;
								for(int l = 0; l < ufo.Count; ++l)
								{
									value = Utilities.DotProduct(destination - ships[currentPlayer][index-1], ships[i][j] - ships[currentPlayer][index-1]);
									if(ufo[l] > value)
										k = l;
								}
								ufo.Insert(k, value);
								indexes.Insert(k, index);
								positions.Insert(k, ships[i][j]);
								players.Insert(k, i);
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
								int k = 0;
								float value = 0;
								for(int l = 0; l < ufo.Count; ++l)
								{
									value = Utilities.DotProduct(ships[currentPlayer][index+1] - destination, ships[i][j] - destination);
									if(ufo[l] > value)
										k = l;
								}
								ufo.Insert(k, value);
								indexes.Insert(k, index+1);
								positions.Insert(k, ships[i][j]);
								players.Insert(k, i);
							}
						}
					}
				}
			}
		}
	}
}