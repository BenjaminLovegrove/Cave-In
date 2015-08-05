using UnityEngine;
using System.Collections;

public class ReadyStateInputText : MonoBehaviour {

	public GameObject readyTextController;
	public GameObject readyTextKeyboard;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	if (PlayerV2.keyboardActive)
		{
			readyTextController.SetActive(false);
			readyTextKeyboard.SetActive(true);
		}
		else if (!Player.keyboardActive)
		{
			readyTextController.SetActive(true);
			readyTextKeyboard.SetActive(false);
		}

	}
}
