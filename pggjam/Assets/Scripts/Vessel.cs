using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vessel : MonoBehaviour 
{
	[SerializeField]
	int owner = -1;

	[SerializeField]
	float speed = 10.0f;

	public float Speed
	{
		get { return speed;}
	}

	public int Owner
	{
		get { return owner; }
	}

	void Start () 
	{
		
	}

	void Update () 
	{
		
	}
}
