using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vessel : MonoBehaviour 
{

    public GameObject otherShip;

    public ParticleSystem particles;

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
        particles = GetComponentInChildren<ParticleSystem>();
        particles.Stop();
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


    public void SetParticlesActive(bool value = true)
    {
        if (value)
            particles.Play();
        else
            particles.Stop();
    }


	public void OnMouseDown()
	{
		if (gameManager != null)
		{
			gameManager.Input = transform.position;
		}
    }


}
