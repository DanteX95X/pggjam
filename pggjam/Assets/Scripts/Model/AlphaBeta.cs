﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
	struct Result
	{
		public float value;
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
				Model.Action current = Prune(state, depth, int.MinValue, int.MaxValue, state.CurrentPlayer == 0, null, timeLimit, timeStart).action;
				if(!needToExit)
					bestAction = current;
				++depth;
				System.GC.Collect();
				yield return null;
			}
			while(Time.time - timeStart < timeLimit);
			//bestAction.Print();
			ufo = bestAction;
			needToExit = false;
		}
		
		static Result Prune(GameState state, int depth, float alpha, float beta, bool isMaximizing, Action lastAction, float timeLimit, float timeStart)
		{
			Result result = new Result();// = new Result();
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
				return result;
			}

			float value;
			Action bestAction = null;

			if(isMaximizing)
			{
				value = int.MinValue;
				foreach(Action action in state.GenerateActions())
				{
					if(!action.IsLegal(state)) return result;
					GameState child = state.Clone();
					action.ApplyAction(child);
					Result nextLevelResult = Prune(child, depth-1, alpha, beta, false, action, timeLimit, timeStart);
					if(needToExit)
					{
						return nextLevelResult;
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
						return nextLevelResult;
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