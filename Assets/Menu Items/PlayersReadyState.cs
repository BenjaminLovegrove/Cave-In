using UnityEngine;
using System.Collections;

public class PlayersReadyState : MonoBehaviour {
	bool p1_Ready = false;
	bool p2_Ready = false;

	void Start(){
		Screen.showCursor = false;
	}

	void Player1Ready()
	{

		if (!p1_Ready)
		{
			//print ("P1 ready");
			p1_Ready = true;
		}
		else
		{
			//print ("P1 NOT ready");
			p1_Ready = false;
		}
	}
	void Player2Ready()
	{

		if (!p2_Ready)
		{
			//print ("P2 ready");
			p2_Ready = true;
		}
		else
		{
			//print ("P2 NOT ready");
			p2_Ready = false;
		}
	}

	void Update()
	{
		if (p1_Ready && p2_Ready)
		{
			StartCoroutine(LoadLevel());
		}
	}

	public IEnumerator LoadLevel()
	{
		CheckpointManager.checkpointSpawn = false;
		yield return new WaitForSeconds(1.0f);
		Application.LoadLevel(1);
	}

}
