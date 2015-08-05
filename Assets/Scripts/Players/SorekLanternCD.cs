using UnityEngine;
using System.Collections;

public class SorekLanternCD : MonoBehaviour {
	private float caveLightFlicker;
	private bool activeFlicker = false;
	public float minRange, maxRange;
	public float flickerTiming;
	
	//for soreks diminishing fire
	public static float diminishAmt = 0.5f;
	public static float maxDiminish = 1.0f;
	public static bool currDiminishing= false;
	public float inspectorDiminish;
	public float inspectorReplenish;

	public static float replenishTimer = 1.0f;
	
	// Use this for initialization
	void Start () {
		
		
	}
	
	void FixedUpdate()
	{

	}
	
	// Update is called once per frame
	void Update () {
		replenishTimer -= Time.deltaTime;


		inspectorDiminish = diminishAmt;
		inspectorReplenish = replenishTimer;

		//change lantern light
		if (diminishAmt > 0.5f && diminishAmt < maxDiminish)
		{
			print ("running");
			caveLightFlicker = Random.Range(minRange/(diminishAmt * minRange),maxRange/(diminishAmt * maxRange));
		}
		else if (diminishAmt <= 0.5f && diminishAmt < maxDiminish)
		{
			caveLightFlicker = Random.Range(minRange,maxRange);
		}

		//max diminish amount catch
		if (diminishAmt >= 1.0f)
		{
			diminishAmt = 1.0f;
		}


		//replenish lantern light
		if (diminishAmt > 0.5f && replenishTimer < 0 && !currDiminishing)
		{
			StartCoroutine(DiminishCD());
			print ("restoring lantern");
		}
		else if (diminishAmt < 0.5)
		{
			diminishAmt = 0.5f;
		}

		//fire actual flickering
		if (!activeFlicker){
			StartCoroutine(Flicker());
		}


	}

	public IEnumerator Flicker ()
	{
		activeFlicker = true;
		this.light.range = caveLightFlicker;
		yield return new WaitForSeconds(flickerTiming);
		activeFlicker = false;
		
	}



	
	public IEnumerator DiminishCD ()
	{
		currDiminishing = true;
		yield return new WaitForSeconds (0.5f);
		diminishAmt -= 0.03f;
		currDiminishing = false;
	}
}
