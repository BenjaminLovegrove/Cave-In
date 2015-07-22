using UnityEngine;
using System.Collections;

public class CandleFollow : MonoBehaviour {

	public Transform p2_Hickory;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
		if (Player.rightFaced == false){
			this.transform.position = new Vector3 (p2_Hickory.transform.position.x - 0.5f, p2_Hickory.transform.position.y + 1.0f,0.0f);
		}
		else{
			this.transform.position = new Vector3 (p2_Hickory.transform.position.x + 0.5f,p2_Hickory.transform.position.y + 1.0f,0.0f);
		}
	}
}
