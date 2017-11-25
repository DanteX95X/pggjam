using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
	public class MoveAction : Action
	{
		Vector2 source;
		Vector2 destination;

		public MoveAction(Vector2 source, Vector2 destination)
			: base(ActionType.MOVE)
		{
			this.source = source;
			this.destination = destination;
		}

		public override void ApplyAction(GameState state)
		{
			for(int i = 0; i < state.Vessels[state.CurrentPlayer].Count; ++i)
			{
				Vector2 currentPosition = state.Vessels[state.CurrentPlayer][i];
				if(currentPosition == source)
				{
					state.Winner = CheckWinConditions(state.Vessels[state.CurrentPlayer], state.Vessels[(state.CurrentPlayer+1)%2], i, state.CurrentPlayer);

					state.Vessels[state.CurrentPlayer][i] = destination;
					return;
				}
			}
		}

		public override void ApplyAction(Game game)
		{
			foreach(Vessel vessel in game.Ships)
			{
				if((Vector2)vessel.transform.position == source)
				{
					vessel.transform.position = new Vector3(destination.x, destination.y, vessel.transform.position.z);
				}
			}
		}

		public override void Print()
		{
			Debug.Log(source + " -> " + destination);
		}

		int CheckWinConditions(List<Vector2> playerShips, List<Vector2> opponentShips, int index, int currentPlayer)
		{
			bool result = false;
			for(int i = 0; i < opponentShips.Count; ++i)
			{
				if(index > 0)
				{
					result = Utilities.isPointInTriangle(playerShips[index-1], source, destination, opponentShips[i]);
					if(result)
						return currentPlayer;
				}
				if(index < playerShips.Count -1)
				{
					result = Utilities.isPointInTriangle(source, playerShips[index+1], destination, opponentShips[i]);
					if(result)
						return currentPlayer;
				}
			}
			return -1;
		}
	}
}