using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class SceneLoader : MonoBehaviour {

	public new string name;
	public bool selected = false;
	public bool isData = false;
	Button but;
	Color basecol;

	// Use this for initialization
	void Start () {
		but = GetComponent<Button> ();
		basecol = but.colors.normalColor;
	}
	
	// Update is called once per frame
	void Update () {
		if(selected){
			SetSelectedColor ();
		} else {
			SetUnselectedColor ();
		}
	}

	void SetSelectedColor(){
		Color col = Color.magenta;
		ColorBlock cb = but.colors;
		cb.normalColor = col;
		but.colors = cb;
	}

	void SetUnselectedColor(){
		ColorBlock cb = but.colors;
		cb.normalColor = basecol;
		but.colors = cb;
	}

	public void LoadLevel(){
		SceneManager.LoadScene (name);
		print ("ting");
	}

	public void Quit(){
		#if UNITY_EDITOR
		print("quit");
		#endif
		Application.Quit ();
	}

	public void IsSelected(){
		selected = !selected;
	}

	public void LoadCustomLevel(){

	}
}

			//use later?? button.pointercostra => zamiast raycastingu