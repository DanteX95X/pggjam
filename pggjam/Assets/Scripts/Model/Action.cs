using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
	public enum ActionType
	{
		MOVE,
	}

	public abstract class Action
	{
		public ActionType type;

		public Action(ActionType type)
		{
			this.type = type;
		}

		public abstract void ApplyMove(GameState state);
		public abstract void Print();
	}
}