using UnityEngine;
using System.Collections;

public enum LightType
{
    DELAY, //Destructs itself x seconds after being instantiated.
	TRIGGER, //Waits to be notified by an external resource before immediatly being destructed
    TRIGGER_DELAY //Waits to be notified by an external resource before starting a timer, and then is destructed.

}
