using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
	class Result
	{
		public int value;
		public Action action;
	}

	public static class AlphaBeta
	{
		static bool needToExit = false;
		public static Model.Action ufo = null;

		public static IEnumerator StartPruning(GameState state, float timeLimit)
		{
			needToExit = false;
			int depth = 1;
			float timeStart = Time.time;
			Action bestAction = null;
			do
			{
				Model.Action current = Prune(state, depth, int.MinValue, int.MaxValue, state.CurrentPlayer == 1, null, timeLimit, timeStart).action;
				if(!needToExit)
					bestAction = current;
				++depth;
				yield return null;
				System.GC.Collect();
			}
			while(Time.time - timeStart < timeLimit);
			//bestAction.Print();
			ufo = bestAction;
			needToExit = false;
		}
		
		static Result Prune(GameState state, int depth, int alpha, int beta, bool isMaximizing, Action lastAction, float timeLimit, float timeStart)
		{
			Result result = null;// = new Result();
			if(needToExit)
			{
				return result;
			}
		
			if(depth == 0 || state.WhoWon() != -1)
			{
				result = new Result();
				result.value = state.Payoff();
				result.action = lastAction;
				return result;
			}

			if(Time.time - timeStart > timeLimit)
			{
				needToExit = true;
				return null;
			}

			int value;
			Action bestAction = null;

			if(isMaximizing)
			{
				value = int.MinValue;
				foreach(Action action in state.GenerateActions())
				{
					GameState child = state.Clone();
					action.ApplyAction(child);
					Result nextLevelResult = Prune(child, depth-1, alpha, beta, false, action, timeLimit, timeStart);
					if(needToExit)
					{
						return null;
					}

					if(nextLevelResult.value > value)
					{
						value = nextLevelResult.value;
						bestAction = action;
					}
					alpha = Mathf.Max(alpha, value);
					if( beta <= alpha)
						break;
				}
				result = new Result();
				result.value = value;
				result.action = bestAction;
				return result;
			}
			else
			{
				value = int.MaxValue;
				foreach(Action action in state.GenerateActions())
				{
					GameState child = state.Clone();
					action.ApplyAction(child);
					Result nextLevelResult = Prune(child, depth-1, alpha, beta, true, action, timeLimit, timeStart);
					if(needToExit)
					{
						return null;
					}
					if(nextLevelResult.value < value)
					{
						value = nextLevelResult.value;
						bestAction = action;
					}

					beta = Mathf.Min(beta, value);
					if(beta <= alpha)
						break;
				}
				result = new Result();
				result.value = value;
				result.action = bestAction;
				return result;
			}
		}
	}
}