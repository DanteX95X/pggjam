﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour 
{
	[SerializeField]
	List<Node> neighbours = null;

	void Start () 
	{
		if(neighbours.Count == 0)
			Debug.LogError("No neighbours in object " + gameObject.name);
	}

	void Update ()
	{
		
	}
}
