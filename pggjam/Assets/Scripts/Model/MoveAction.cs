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
				if(state.Vessels[state.CurrentPlayer][i] == source)
				{
					state.Vessels[state.CurrentPlayer][i] = destination;
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
	}
}