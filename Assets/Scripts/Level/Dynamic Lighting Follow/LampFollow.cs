using UnityEngine;
using System.Collections;

public class LampFollow : MonoBehaviour {

	public Transform p1_Sorek;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


		if (Player.rightFaced == false){
			this.transform.position = new Vector3 (p1_Sorek.transform.position.x - 0.6f, p1_Sorek.transform.position.y + 0.4f,0.0f);
		}
		else{
			this.transform.position = new Vector3 (p1_Sorek.transform.position.x + 0.6f,p1_Sorek.transform.position.y + 0.4f,0.0f);
		}



	}
}
