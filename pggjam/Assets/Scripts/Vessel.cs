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

        //line = GetComponent<LineRenderer>();
        //line.positionCount = lineSegments;
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
        /*if (otherShip != null)
        {
            line.positionCount = lineSegments;
            Vector3 deltaVector = otherShip.transform.position - transform.position;
            float segment = 0.0f;
            for (int i = 0; i < lineSegments; i++)
            {
                segment = (float)i / (float)(lineSegments - 1);
                line.SetPosition(i, transform.position + segment * deltaVector);
            }
        }
        else
        {
            line.positionCount = 0;
        }*/
       
    }

	private void OnMouseDown()
	{
		if (gameManager != null)
		{
			gameManager.Input = transform.position;
		}
    }
}
