using UnityEngine;
using System.Collections;

public class CandleFollow : MonoBehaviour {

	public Transform p2_Hickory;
	public PlayerV2 hickoryScr;
	

	// Update is called once per frame
	void Update () {
		
		
		if (hickoryScr.rightFaced == false){
			this.transform.position = new Vector3 (p2_Hickory.transform.position.x - 0.55f, p2_Hickory.transform.position.y + 1.0f,0.0f);
		}
		else{
			this.transform.position = new Vector3 (p2_Hickory.transform.position.x + 0.55f,p2_Hickory.transform.position.y + 1.0f,0.0f);
		}
	}
}
