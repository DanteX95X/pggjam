using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
	public struct ToRemove
	{
		public int index;
		public Vector2 position;
		public float projection;
		public int player;

		public ToRemove(int index, Vector2 position, int player, float projection)
		{
			this.index = index;
			this.position = position;
			this.projection = projection;
			this.player = player;
		}
	}

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
			List<ToRemove> left = new List<ToRemove>();
			List<ToRemove> right = new List<ToRemove>();

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
								//left.Add(new ToRemove(index, ships[i][j], i, Utilities.DotProduct(destination - ships[currentPlayer][index-1], ships[i][j] - ships[currentPlayer][index-1])));
								indexes.Add(index);
								positions.Add(ships[i][j]);
								players.Add(i);
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
								//left.Add(new ToRemove(index+1, ships[i][j], i, Utilities.DotProduct(ships[currentPlayer][index+1] - destination, ships[i][j] - destination)));
								indexes.Add(index + 1);
								positions.Add(ships[i][j]);
								players.Add(i);
							}
						}
					}
				}
			}

			//left.Sort((x,y) => x.projection.CompareTo(y.projection));
			//right.Sort((x,y) => x.projection.CompareTo(y.projection));
			/*indexes.Clear();
			positions.Clear();
			players.Clear();
			foreach(ToRemove i in left)
			{
				//Debug.Log(i.position + " " + i.projection);
				indexes.Add(i.index);
				positions.Add(i.position);
				players.Add(i.player);
			}
			foreach(ToRemove i in right)
			{
				//Debug.Log(i.position + " " + i.projection);
				indexes.Add(i.index);
				positions.Add(i.position);
				players.Add(i.player);
			}
			left.Clear();*/
		}
	}
}