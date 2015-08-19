using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseInputManager : MonoBehaviour {
	
	public Color defaultCol, hoverCol, pressCol;

	//Text gameobjects
	public Text 
		resumeGame,
		loadLastCheckpoint,
		exitGame;

	// Use this for initialization
	void Start () {
		defaultCol = Color.white;
		hoverCol = Color.red;
		pressCol = Color.green;
	
	}

	public void Hover()
	{
		if (this.name == "Resume")
		{
			resumeGame.color = hoverCol;
		}
		if (gameObject.name == "Reload")
		{
			loadLastCheckpoint.color = hoverCol;
		}
		if (gameObject.name == "Exit")
		{
			exitGame.color = hoverCol;
		}

	}

	public void DefaultCol()
	{
		if (this.name == "Resume")
		{
			resumeGame.color = defaultCol;
		}
		if (gameObject.name == "Reload")
		{
			loadLastCheckpoint.color = defaultCol;
		}
		if (gameObject.name == "Exit")
		{
			exitGame.color = defaultCol;
		}
	}


}
