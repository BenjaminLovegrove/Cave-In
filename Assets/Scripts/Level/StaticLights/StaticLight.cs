using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class StaticLight : MonoBehaviour
{
    //Construction
    public LightType lightType;

    //Destruction
    [Range(0.01f, 300.0f)]
    public float aliveTime;

    private bool isTriggered;
    private bool destroying;

	// Initialization
	void Awake ()
    {
        isTriggered = false;
        destroying = false;
        GetComponent<ParticleSystem>().Stop();
	}

    public void Trigger()
    {
        isTriggered = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        bool destroy = false;

	    switch(lightType)
        {
            case LightType.DELAY:
            {
                aliveTime -= Time.deltaTime;

                if(aliveTime <= 0)
                {
                    destroy = true;
                }
                break;
            }
            case LightType.TRIGGER:
            {
                if(isTriggered)
                {
                    destroy = true;
                }
                break;
            }
            case LightType.TRIGGER_DELAY:
            {
                if (isTriggered)
                {
                    aliveTime -= Time.deltaTime;

                    if (aliveTime <= 0)
                    {
                        destroy = true;
                    }
                }
                break;
            }
        }

        if(destroy && !destroying)
        {
            destroying = true;

            //Destroy The Light
            GetComponent<Light>().enabled = false;

            //Play particle, then destroy this GameObject
            ParticleSystem dest = GetComponent<ParticleSystem>();
            dest.Play();
            Destroy(this.gameObject, dest.duration);
        }
	}
}
