using UnityEngine;
using System.Collections;

public class CaveInDestroyOnLoad : MonoBehaviour {

	public float desTimer = 5f;
	public bool deathNotice = false;

	// Use this for initialization
	void Start () {
		Destroy(gameObject, desTimer);
	}
	
	// Update is called once per frame
	void OnCollisionEnter (Collision col) {
		if (col.gameObject.tag == "Player")
		{
			col.gameObject.SendMessage ("Crushed", SendMessageOptions.DontRequireReceiver);
			StartCoroutine(NotifyOfDeath());
		}
	}

	void OnGUI()
	{
		if (deathNotice)
		{
			GUI.Label(new Rect(Screen.width/ 2,Screen.height/2,Screen.width,Screen.height),"You Were Crushed By The Cave In!");
		}
	}
	
	IEnumerator NotifyOfDeath()
	{
		deathNotice = true;
		yield return new WaitForSeconds (5.0f);
		deathNotice = false;
		
		
	}
}
