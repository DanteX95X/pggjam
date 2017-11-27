using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoosePlayerButton : MonoBehaviour 
{
	[SerializeField]
	Sprite[] sprite = null;

	[SerializeField]
	int index;

	void Start () 
	{
		GetComponent<Image>().sprite = sprite[index];// = texture;
	}

	void Update () 
	{
		
	}

	public void ChangeSprite()
	{
		index = (index+1) % sprite.Length;
		GetComponent<Image>().sprite = sprite[index];
	}
}
