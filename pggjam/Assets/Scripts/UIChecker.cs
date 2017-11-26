using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChecker : MonoBehaviour {
	List<GameObject> buttons;
	CleanUIForCanvas canvaUI;
	// Use this for initialization
	void Awake(){
		canvaUI = FindObjectOfType<CleanUIForCanvas> ();
		buttons = new List<GameObject>();
		buttons.AddRange(GameObject.FindGameObjectsWithTag ("ui"));
		buttons.Add(GameObject.FindGameObjectWithTag ("exit"));
	}

	void Start () {

		//foreach (var go in buttons) {
		//	print (go.name);
		//}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool IsGazeInsideElement(Vector2 gaze, out GameObject button){
		button = null;
		foreach (var go in buttons) {
			if (canvaUI.CurrentBounds.Contains(gaze)) {
				button = go;
				return true;
			}
		}
		return false;
	}
}
