using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphGenerator
{
	static void FindHull(ref List<Vector2> points, Vector2 begin, Vector2 end, Dictionary<Vector2, List<Vector2>> map, int depth)
	{
		List<Vector2> partOfHull = new List<Vector2>();
		if (points.Count == 0 || depth > 3)
		{
			return;
		}

		float maxDistance = int.MinValue;
		Vector2 maxPoint = points[0];
		foreach (Vector2 point in points)
		{
			int roll = UnityEngine.Random.Range(0, 100);
			if(roll > 90)
			{
				if(!map.ContainsKey(point))
				{
					map[point] = new List<Vector2>();
				}
				map[point].Add(begin);
				map[begin].Add(point);
				map[point].Add(end);
				map[end].Add(point);
			}

			float distance = Model.Utilities.DistanceFromLine(begin, end, point);
			if (distance > maxDistance)
			{
				maxDistance = distance;
				maxPoint = point;
			}
		}

		List<Vector2> firstSet = new List<Vector2>();
		List<Vector2> secondSet = new List<Vector2>();
		foreach (Vector2 point in points)
		{
			float distance = Model.Utilities.DistanceFromLine(begin, maxPoint, point);
			if (distance > 0)
			{
				firstSet.Add(point);
			}

			distance = Model.Utilities.DistanceFromLine(maxPoint, end, point);
			if (distance > 0)
				secondSet.Add(point);
		}

		map[maxPoint] = new List<Vector2>();
		map[begin].Add(maxPoint);
		map[maxPoint].Add(begin);
		map[end].Add(maxPoint);
		map[maxPoint].Add(end);

		FindHull(ref firstSet, begin, maxPoint, map, depth+1);
		FindHull(ref secondSet, maxPoint, end, map, depth+1);
	}

	public static Dictionary<Vector2, List<Vector2>> Quickhull(List<Vector2> points) 
	{
		Debug.Log("Call");
		List<Vector2> hull = new List<Vector2>();
		Dictionary<Vector2, List<Vector2>> map = new Dictionary<Vector2, List<Vector2>>();

		Vector2 minPoint = new Vector2(int.MaxValue, int.MaxValue);
		Vector2 maxPoint = new Vector2(int.MinValue, int.MinValue);

		for(int i = 0; i < points.Count; ++i)
		{
			if(points[i].x < minPoint.x || (points[i].x == minPoint.x && points[i].y < minPoint.y))
				minPoint = points[i];
			if(points[i].x > maxPoint.x || (points[i].x == maxPoint.x && points[i].y > maxPoint.y)) 
				maxPoint = points[i];
		}

		List<Vector2> above = new List<Vector2>();
		List<Vector2> below = new List<Vector2>();

		foreach(Vector2 point in points)
		{
			float distance = Model.Utilities.DistanceFromLine(minPoint, maxPoint, point);
			if(distance > 0)
				above.Add(point);

			distance = Model.Utilities.DistanceFromLine(maxPoint, minPoint, point);
			if(distance > 0)
				below.Add(point);
		}

		map[minPoint] = new List<Vector2>();
		map[maxPoint] = new List<Vector2>();
		map[minPoint].Add(maxPoint);
		map[maxPoint].Add(minPoint);

		hull.Add(minPoint);
		FindHull(ref above, minPoint, maxPoint, map, 1);
		FindHull(ref below, maxPoint, minPoint, map, 1);

		return map;
	}
}
