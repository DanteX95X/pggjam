using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class GameManager : MonoBehaviour {


	//TODO:add eveerywhere where we want interaction gazeaware

	//Gaze Related Parameters
	public bool isETEnabled = true; //will hold info about whether we 
	GameObject currentGazeTarget;
	GameObject lastGazeTarget;
	public float progressThreshold = 0.5f;
	public float continueThreshold = 1.5f;
	public float deltaProgress;
	public float deltaContinue;


	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if (!isETEnabled) {return;}
		currentGazeTarget = TobiiAPI.GetFocusedObject ();
		deltaProgress += Time.deltaTime;
		deltaContinue += Time.deltaTime;
	
		if (deltaContinue >= continueThreshold) {
			if (currentGazeTarget == lastGazeTarget) {
				SendClickedMessage();
			}
			deltaContinue = 0;
			return;

		}
		if (deltaProgress >= progressThreshold) {
			if (currentGazeTarget == lastGazeTarget) {
				UpdateProgressStatus ();
			}
			deltaProgress = 0;
			return;
		}
		lastGazeTarget = currentGazeTarget;
	}

	void SendClickedMessage(){
		print ("click");
		//send clicked message for vessels
		//send clicked message for ui aas well?
		if (currentGazeTarget.CompareTag ("vessel")) 
			currentGazeTarget.GetComponent<Vessel> ().OnMouseDown ();
		if (currentGazeTarget.CompareTag ("node")) 
			currentGazeTarget.GetComponent<Node> ().OnMouseDown ();
		if (currentGazeTarget.CompareTag ("ui"))
			currentGazeTarget.GetComponent<SceneLoader> ().LoadLevel ();
		if(currentGazeTarget.CompareTag("exit"))
			currentGazeTarget.GetComponent<SceneLoader>().Quit();



	}

	void UpdateProgressStatus(){
		print ("progress");
	}

	void SimulateMouseClick(){
		
	}
}
