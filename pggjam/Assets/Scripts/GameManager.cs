using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {

	//Gaze Related Parameters
	public bool isETEnabled = true; //will hold info about whether we 
	GameObject currentGazeTarget;
	GameObject lastGazeTarget;
	public float progressThreshold = 0.5f;
	public float continueThreshold = 1.5f;
	public float deltaProgress;
	public float deltaContinue;

	UIChecker checker;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
		UnityEngine.SceneManagement.SceneManager.sceneLoaded += (UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.LoadSceneMode arg1) => {
			checker = GetComponent<UIChecker>();
		};
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
		if (currentGazeTarget) {
			if (currentGazeTarget.CompareTag ("vessel"))
				currentGazeTarget.GetComponent<Vessel> ().OnMouseDown ();
			if (currentGazeTarget.CompareTag ("node"))
				currentGazeTarget.GetComponent<Node> ().OnMouseDown ();
		} else {
			GazePoint pt = TobiiAPI.GetGazePoint ();
			if (!pt.IsValid) {
				return;
			}
			Vector2 gaze = pt.Viewport;
			GameObject obj = GameObject.Find ("dummy");
			if(checker.IsGazeInsideElement(gaze,out obj)){
				if (obj.CompareTag ("ui"))
					obj.GetComponent<SceneLoader> ().LoadLevel ();
				if (obj.CompareTag ("quit"))
					obj.GetComponent<SceneLoader> ().Quit ();
			}
		}
	}

	void SendClickedToUI(){
		//handle UI separately
		PointerEventData pointerData = new PointerEventData (EventSystem.current);
		GazePoint pt = TobiiAPI.GetGazePoint ();
		pointerData.position = pt.Viewport;
		List<RaycastResult> results = new List<RaycastResult> ();
		EventSystem.current.RaycastAll (pointerData, results);
		if(results.Count > 0) {
			GameObject tmp = results [0].gameObject;
			string dbg = "Root Element: {0} \n GrandChild Element: {1}";
			Debug.Log(string.Format(dbg, results[results.Count-1].gameObject.name,results[0].gameObject.name));
			if (tmp.layer == 8) {
				print ("LEEEEEE");
				if (tmp.CompareTag ("ui")) {

					tmp.GetComponent<SceneLoader> ().LoadLevel ();
				}
				if(tmp.CompareTag("exit"))
					tmp.GetComponent<SceneLoader> ().Quit ();
			}
		}
	}

	void UpdateProgressStatus(){
		print ("progress");
	}

	void SimulateMouseClick(){
		
	}
}
