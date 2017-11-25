using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour 
{
	[SerializeField]
	List<Node> neighbours = null;

    Game gameManager = null;

	public List<Node> Neighbours 
	{
		get { return neighbours; }
	}


    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Game>();
    }

    void Start () 
	{
		if(neighbours.Count == 0)
			Debug.LogError("No neighbours in object " + gameObject.name);
	}

    private void OnMouseDown()
    {
        if (gameManager != null)
        {
            gameManager.moveShip(transform.position);
        }
    }

    void Update ()
	{
		
	}
}
