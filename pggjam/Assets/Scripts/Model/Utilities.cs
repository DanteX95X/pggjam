using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
	public static class Utilities
	{
		static float DistanceFromLine(Vector2 start, Vector2 end, Vector2 point) 
		{
			float a = end.y - start.y;
			float b = start.x - end.x;
			float c = start.y * end.x - start.x * end.y;
			return -(a * point.x + b * point.y + c);
		}

		static float Signum(float number)
		{
			return number / Mathf.Abs(number); 
		}

		public static bool isPointInTriangle(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 point)
		{
			List<float> results = new List<float>();
			results.Add(Signum(DistanceFromLine(p0, p1, point)));
			results.Add(Signum(DistanceFromLine(p1, p2, point)));
			results.Add(Signum(DistanceFromLine(p2, p0, point)));

			return results[0] == results[1] && results[0] == results[2];
		}
	}
}