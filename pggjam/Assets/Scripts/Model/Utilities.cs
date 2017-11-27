using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
	public static class Utilities
	{
		public static float DistanceFromLine(Vector2 start, Vector2 end, Vector2 point) 
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

		public static bool isPointInTriangle(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 point, float threshold = 0.5f)
		{
			List<float> results = new List<float>();
			results.Add((DistanceFromLine(p0, p1, point)));
			results.Add((DistanceFromLine(p1, p2, point)));
			results.Add((DistanceFromLine(p2, p0, point)));

			bool result = false;
			result = result || ((canBeProjected(point, p0, p1) && Mathf.Abs(results[0]) < threshold));
			result = result || ((canBeProjected(point, p1, p0) && Mathf.Abs(results[0]) < threshold));

			result = result || ((canBeProjected(point, p1, p2) && Mathf.Abs(results[1]) < threshold));
			result = result || ((canBeProjected(point, p2, p1) && Mathf.Abs(results[1]) < threshold));

			result = result || ((canBeProjected(point, p2, p0) && Mathf.Abs(results[2]) < threshold));
			result = result || ((canBeProjected(point, p0, p2) && Mathf.Abs(results[2]) < threshold));

			for(int i = 0; i < results.Count; ++i)
			{
				results[i] = Signum(results[i]);
			}

			return result || (results[0] == results[1] && results[0] == results[2]);
		}

		public static bool canBeProjected(Vector2 point, Vector2 begin, Vector2 end)
		{
			Vector2 Q = end-begin;
			Vector2 P = point - begin;
			Vector2 projection = (Q).normalized * (DotProduct((P), (Q)) / (Q).magnitude);
			return projection.x >= 0 && projection.x <= Q.x && projection.y >= 0 && projection.y <= Q.y;
		}

		public static float angleBetweenVectors(Vector2 first, Vector2 second)
		{
			float dot = first.x * second.x + first.y * second.y;
			float det = first.x * second.y - first.y * second.x;
			float angle = -Mathf.Atan2(det, dot) * 180/Mathf.PI;
			if(angle < 0)
				angle = 360 + angle;
			return angle;
		}

		public static float CrossProduct(Vector2 origin, Vector2 first, Vector2 second)
		{
			return (first.x - origin.x)  * (second.y - origin.y)  -  (first.y - origin.y) * (second.x - origin.x);
		}

		public static float DotProduct(Vector2 first, Vector2 second)
		{
			//Debug.Log(first + " " + second);
			return first.x * second.x + first.y * second.y;
		}

	}
}