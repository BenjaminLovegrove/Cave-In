using UnityEngine;
using System.Collections;

public class CaveInKillBox : MonoBehaviour {
	public GameObject rockPreTestFall;
	private bool rockfalling = false;
	GameObject[] players;
	Animator anim;
	public float caveInNumber;
	public GameObject fallingRockPre;


	// Use this for initialization
	void Start () {

		anim = GetComponent<Animator> ();

		if (caveInNumber == 1){
			anim.speed = Intro.ci1difficulty;
		} else if (caveInNumber == 2){
			anim.speed = Intro.ci2difficulty;
		}

		players = GameObject.FindGameObjectsWithTag ("Player");
		foreach (GameObject playerTarg in players){
			playerTarg.SendMessage("CaveInStart", this.gameObject, SendMessageOptions.DontRequireReceiver);
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		Camera.main.SendMessage ("DoExplosionShake");

		if (!rockfalling){
			StartCoroutine(SpawnRock());
		}
		Instantiate (fallingRockPre, this.transform.position, Quaternion.identity);



	}

	void FixedUpdate()
	{

	}

	public IEnumerator SpawnRock()
	{
		rockfalling = true;
		Instantiate(rockPreTestFall, gameObject.transform.position, rockPreTestFall.transform.rotation);
		yield return new WaitForSeconds (0.5f);
		rockfalling = false;
	}

//	void OnTriggerEnter(Collider col)
//	{
//		if (col.gameObject.tag == "Player")
//		{
//			col.SendMessage("Crushed", true);
//		}
//	}

	public void KillMe(){
		players [0].SendMessage ("CaveInStarted", false);
		players [1].SendMessage ("CaveInStarted", false);
		Destroy (this.gameObject);
	}

	public void StartCaveIn(){
		players[0].SendMessage ("CaveInStart", this.gameObject,SendMessageOptions.DontRequireReceiver); 
		players[1].SendMessage ("CaveInStart", this.gameObject,SendMessageOptions.DontRequireReceiver);
		players[2].SendMessage ("CaveInStart", this.gameObject,SendMessageOptions.DontRequireReceiver);
	}


}
