using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour 
{
	[SerializeField]
	List<Node> neighbours = null;

    Game gameManager = null;

    [SerializeField]
    Animator animator = null;

    public GameObject linePrefab = null;

	public List<Node> Neighbours 
	{
		get { return neighbours; }
	}


    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Game>();
        animator = GetComponent<Animator>();
    }

    void Start () 
	{
		if(neighbours.Count == 0)
			Debug.LogError("No neighbours in object " + gameObject.name);

        foreach (Node neighbour in Neighbours)
        {
            if (linePrefab != null)
            {
                GameObject go = Instantiate(linePrefab, transform);
                LineRenderer line = go.GetComponent<LineRenderer>();
                line.positionCount = 2;
                line.SetPosition(0, new Vector3(transform.position.x, transform.position.y, 1));
                line.SetPosition(1, new Vector3(neighbour.transform.position.x, neighbour.transform.position.y, 1));
            }
        }
	}

    public void OnMouseDown()
	{
		if (gameManager != null)
		{
			/*if (gameManager.CurrentShip != -1)
			{
				gameManager.moveShip(transform.position);
			}*/
			gameManager.Input = transform.position;
        }
    }

    public void OnMouseEnter()
    {
        animator.SetBool("isMouseOn", true);
    }

    private void OnMouseExit()
    {
        animator.SetBool("isMouseOn", false);
    }

    void Update ()
	{
		
	}
}
