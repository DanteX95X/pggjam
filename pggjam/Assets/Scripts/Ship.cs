using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

    public GameObject otherShip;

    private LineRenderer line;

    public int lineSegments = 100;

    public float speed = 100.0f;

	// Use this for initialization
	void Start () {
        line = GetComponent<LineRenderer>();
        line.positionCount = lineSegments;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 deltaVector = otherShip.transform.position - transform.position;
        float segment = 0.0f;
        for (int i = 0; i < lineSegments; i++)
        {
            segment = (float)i / (float)(lineSegments - 1);
            line.SetPosition(i, transform.position + segment * deltaVector );
        }
        
        
    }
}
