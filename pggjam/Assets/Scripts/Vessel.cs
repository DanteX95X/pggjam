using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vessel : MonoBehaviour 
{

    public GameObject otherShip;

    private LineRenderer line;

    public int lineSegments = 100;

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

	public void OnMouseDown()
	{
		if (gameManager != null)
		{
			gameManager.Input = transform.position;
		}
    }


}
