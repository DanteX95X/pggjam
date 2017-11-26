using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class SceneLoader : MonoBehaviour {

	public string name;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadLevel(){
		EditorSceneManager.LoadScene (name);
		print ("ting");
	}

	public void Quit(){
		#if UNITY_EDITOR
		print("quit");
		#endif
		Application.Quit ();
	}
}
