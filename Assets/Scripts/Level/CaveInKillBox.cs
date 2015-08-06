using UnityEngine;
using System.Collections;

public class CaveInKillBox : MonoBehaviour {
	public GameObject rockPreTestFall;
	private bool rockfalling = false;
	GameObject[] players;
	//AudioSource rumbleSFX;
	//float p1caveInDistance;
	//float p2caveInDistance;

	// Use this for initialization
	void Start () {
		//rumbleSFX = GetComponent<AudioSource> ();

		//This is to pass this gameobject to both players for distance checks
		players = GameObject.FindGameObjectsWithTag ("Player");
		foreach (GameObject playerTarg in players){
			playerTarg.SendMessage("CaveInStart", this.gameObject, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!rockfalling){
			StartCoroutine(SpawnRock());
		}

		//p1caveInDistance = Vector3.Distance (transform.position, players [0].transform.position);
		//p2caveInDistance = Vector3.Distance (transform.position, players [1].transform.position);
		/*if (p1caveInDistance < p2caveInDistance){
			rumbleSFX.volume = Mathf.Lerp (1f, 0.2f, (p1caveInDistance / 20));
		} else {
			rumbleSFX.volume = Mathf.Lerp (1f, 0.2f, (p2caveInDistance / 20));
		}*/


	}

	public IEnumerator SpawnRock()
	{
		rockfalling = true;
		Instantiate(rockPreTestFall, gameObject.transform.position, rockPreTestFall.transform.rotation);
		yield return new WaitForSeconds (0.5f);
		rockfalling = false;
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player")
		{
			col.SendMessage("Crushed");
		}
	}

	public void KillMe(){
		Destroy (this.gameObject);
	}

}
