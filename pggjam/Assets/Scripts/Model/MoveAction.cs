using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
	public class MoveAction : Action
	{
		Vector2 source;
		Vector2 destination;

		MoveAction(Vector2 source, Vector2 destination)
			: base(ActionType.MOVE)
		{
			this.source = source;
			this.destination = destination;
		}

		public override void ApplyMove(GameState state)
		{
			for(int i = 0; i < state.Vessels[state.CurrentPlayer].Count; ++i)
			{
				if(state.Vessels[state.CurrentPlayer][i] == source)
				{
					state.Vessels[state.CurrentPlayer][i] = destination;
				}
			}
		}

		public override void Print()
		{
			Debug.Log(source + " -> " + destination);
		}
	}
}