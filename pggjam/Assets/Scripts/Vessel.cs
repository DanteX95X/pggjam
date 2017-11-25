using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vessel : MonoBehaviour 
{
	[SerializeField]
	int owner = -1;

	[SerializeField]
	float speed = 10.0f;

	Game gameManager = null;

	void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Game>();
    }

	public float Speed
	{
		get { return speed;}
	}

	public int Owner
	{
		get { return owner; }
		set { owner = value; }
	}

	void Start () 
	{
		
	}

	void Update () 
	{
		
	}

	private void OnMouseDown()
	{
		if (gameManager != null)
		{
			gameManager.Input = transform.position;
		}
    }
}
