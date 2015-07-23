using UnityEngine;
using System.Collections;

public class CaveInKillBox : MonoBehaviour {
	public GameObject rockPreTestFall;
	private bool rockfalling = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!rockfalling){
			StartCoroutine(SpawnRock());
		}
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
			Debug.Log("Player was crushed by rocks");
		}
	}
}
