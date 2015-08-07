﻿using UnityEngine;
using System.Collections;

public class LevelKillBox : MonoBehaviour {

	public Transform p1TelePos, p2TelePos;
	bool deathNotice = false;

	void Awake()
	{
		p1TelePos = GameObject.Find("P1TelePos").transform;
		p2TelePos = GameObject.Find("P2TelePos").transform;
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.layer == 9)
		{
			col.gameObject.transform.position = p1TelePos.position;
			StartCoroutine(NotifyOfDeath());
		}
		if (col.gameObject.layer == 10)
		{
			StartCoroutine(NotifyOfDeath());
			col.gameObject.transform.position = p2TelePos.position;
		}
	}

	void OnGUI()
	{
		if (deathNotice)
		{
			GUI.Label(new Rect(Screen.width/ 2,Screen.height/2,Screen.width,Screen.height),"Rest Player Position After Falling!");
		}
	}

	IEnumerator NotifyOfDeath()
	{
		deathNotice = true;
		yield return new WaitForSeconds (5.0f);
		deathNotice = true;


	}
}
