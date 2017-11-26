using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoaderData : MonoBehaviour {

	public bool vsAI = true;
	public bool fromFile = true;

	GameObject[] sc;

	// Use this for initialization
	void Start () {
		sc = GameObject.FindGameObjectsWithTag ("ui");
	}
	
	// Update is called once per frame
	void Update () {
		SceneLoader left1 = new SceneLoader ();
		SceneLoader right1 = new SceneLoader ();
		SceneLoader left2 = new SceneLoader ();
		SceneLoader right2 = new SceneLoader ();
		foreach (GameObject s in sc) {
			var a = s.GetComponent<SceneLoader> ();
			if (a.isData) {
				if (s.name == "AI") {
					left1 = a;
				} else if (s.name == "Player") {
					right1 = a;
				} else if (s.name == "premade") {
					left2 = a;
				} else if (s.name == "random") {
					right2 = a;
				}
			}
		}	

		if (left1.selected && right1.selected) {
			left1.selected = false;
			right1.selected = false;
		} else {
			if (left1.selected) {
				vsAI = true;
			} else if (right1.selected) {
				vsAI = false;
			}
		}

		if (left2.selected && right2.selected) {
			left2.selected = false;
			right2.selected = false;
		}else {
			if (left2.selected) {
				fromFile = true;
			} else if (right2.selected) {
				fromFile = false;
			}
		}

	}
}
