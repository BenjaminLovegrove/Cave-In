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
	public Color defaultColor;
	private float lightCurrDim;

	//public Light light;
	
	// Use this for initialization
	void Start () {
		
	}
	
	void FixedUpdate()
	{
		defaultColor = new Color(202f/255.0f,138f/255.0f,40f/255.0f);
	}
	
	// Update is called once per frame
	void Update () {
		replenishTimer -= Time.deltaTime;

		//print (defaultColor.ToString());

		//to see these variables in the inspector
		inspectorDiminish = diminishAmt;
		inspectorReplenish = replenishTimer;

		//change lantern light
		if (diminishAmt > maxDiminish / 2.0f && diminishAmt < maxDiminish)
		{
			caveLightFlicker = Random.Range(minRange/(diminishAmt * minRange),maxRange/(diminishAmt * maxRange));
			//light.color = new Color(202f / (diminishAmt),138f / (diminishAmt),40f/ (diminishAmt),0f);
			lightCurrDim = Mathf.MoveTowards(lightCurrDim, diminishAmt , 0.05f * Time.deltaTime);
			light.color = (defaultColor * ((1.0f - lightCurrDim)*2.0f));

		}
		else if (diminishAmt <= maxDiminish / 2.0f && diminishAmt < maxDiminish)
		{
			caveLightFlicker = Random.Range(minRange,maxRange);
			light.color = defaultColor;
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
