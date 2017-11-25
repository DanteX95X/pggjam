using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
	public class SelectShipAction : Action 
	{
		Vector2 position;

		public SelectShipAction(Vector2 position)
			: base(ActionType.SELECT)
		{
			this.position = position;
		}

		public override bool IsLegal(GameState state)
		{
			return state.Vessels[state.CurrentPlayer].Contains(position);
		}

		public override void ApplyAction(GameState state)
		{
			state.SelectedPosition = position;
		}

		public override void ApplyAction(Game game)
		{
			game.AquireShip(position);
		}

		public override void Print()
		{
			Debug.Log("Selection " + position);
		}

		
	}
}
