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

		public abstract void ApplyAction(GameState state);
		public abstract void ApplyAction(Game game);
		public abstract void Print();
	}
}