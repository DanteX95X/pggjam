using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TobiActivator : MonoBehaviour {


	void Awake () {
		#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
			gameObject.SetActive(true);
		#else
			gameObject.SetActive(false);
		#endif
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
