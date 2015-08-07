using UnityEngine;
using System.Collections;

public class LevelKillBox : MonoBehaviour {

	public Transform p1TelePos, p2TelePos;

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
		}
		if (col.gameObject.layer == 10)
		{
			col.gameObject.transform.position = p2TelePos.position;
		}
	}
}
