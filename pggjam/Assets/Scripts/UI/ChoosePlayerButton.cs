using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChoosePlayerButton : MonoBehaviour 
{
	[SerializeField]
	Sprite[] sprites = null;

	[SerializeField]
	int spriteIndex = 0;

	[SerializeField]
	int playerIndex = -1;

	void Start () 
	{
		GetComponent<Image>().sprite = sprites[spriteIndex];// = texture;
	}

	void Update () 
	{
		
	}

	public void ChangeSprite()
	{
		spriteIndex = (spriteIndex+1) % sprites.Length;
		GetComponent<Image>().sprite = sprites[spriteIndex];
		GameManager.controllerTypes[playerIndex] = (Game.ControllerType)spriteIndex;
	}

	public void Play()
	{
		SceneManager.LoadScene("game");
	}

	public void Menu()
	{
		SceneManager.LoadScene("menuScene");
	}
}
