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
	}

	void Start () 
	{
		
	}

	void Update () 
	{
		
	}

	private void OnMouseDown()
	{
		/*if (gameManager != null && gameManager.CurrentShip == -1)
		{
			Debug.Log("ship aquired");
			gameManager.AquireShip(transform.position);
        }*/
		if (gameManager != null)
		{
			Debug.Log("ok");
			gameManager.Input = transform.position;
		}
    }
}
