using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
	public enum ActionType
	{
		MOVE,
		SELECT,
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
		public abstract bool IsLegal(GameState state);
		public abstract void Print();
	}
}