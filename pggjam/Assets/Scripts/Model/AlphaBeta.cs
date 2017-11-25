using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
	struct Result
	{
		public int value;
		public Action action;
	}

	public static class AlphaBeta
	{
		static bool needToExit = false;

		public static Action StartPruning(GameState state, float timeLimit)
		{
			needToExit = false;
			int depth = 1;
			float timeStart = Time.time;
			Action bestAction = null;
			do
			{
				//maybe clone state if it wont work
				bestAction = Prune(state, depth, int.MinValue, int.MaxValue, state.CurrentPlayer == 0, null, timeLimit, timeStart).action;
				++depth;
			}
			while(Time.time - timeStart < timeLimit);

			return bestAction;
				
		}
		
		static Result Prune(GameState state, int depth, int alpha, int beta, bool isMaximizing, Action lastAction, float timeLimit, float timeStart)
		{
			Result result = new Result();
			if(needToExit)
			{
				return result;
			}
		
			if(depth == 0 || state.WhoWon() != -1)
			{
				result.value = state.Payoff();
				result.action = lastAction;
				return result;
			}

			if(Time.time - timeStart > timeLimit)
			{
				needToExit = true;
				return result;
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
					if(nextLevelResult.value > value)
					{
						value = nextLevelResult.value;
						bestAction = action;
					}
					alpha = Mathf.Max(alpha, value);
					if( beta <= alpha)
						break;
				}
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
					if(nextLevelResult.value < value)
					{
						value = nextLevelResult.value;
						bestAction = action;
					}

					beta = Mathf.Min(beta, value);
					if(beta <= alpha)
						break;
				}
				result.value = value;
				result.action = bestAction;
				return result;
			}
		}
	}
}