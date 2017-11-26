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
			foreach(Action action in state.GenerateActions())
			{
				if(equals(action))
					return true;
			}
			return false;//state.Vessels[state.CurrentPlayer].Contains(position);
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

		bool equals(Action other)
		{
			if(type == other.type)
			{
				return position == ((SelectShipAction)other).position;
			}
			return false;
		}

		public override Vector2 TargetPosition()
		{
			return position;
		}

		
	}
}
