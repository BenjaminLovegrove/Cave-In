using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Splines/Spline Controller")]
[RequireComponent(typeof(SplineInterpolator))]
[RequireComponent(typeof(LineRenderer))]
public class SplineController : MonoBehaviour
{
    public GameObject splineObjectInst; //The spline object to move.

    private GameObject splineObject;

    public List<GameObject> points = new List<GameObject>();    //The spline points

    public int vertexCount = 100;                               //Number of vertices to use for rendering/moving along the spline. (Editor/Play Mode)

    public float duration = 10;                                 //Duration for spline movement.

    private bool triggered = false;

	public bool test = false;

    //Members
    private SplineInterpolator m_splineInterpolator;            //The spline interpolator instance.                          //The transforms of our spline points.
	
    void OnDrawGizmos()
    {
        //Convert our points into an array of transforms.
        if (points.Count < 2)
        {
            return;
        }

        //Set up our interpolator instance.
        SplineInterpolator splineInterpolator = GetComponent(typeof(SplineInterpolator)) as SplineInterpolator;
        SetupSplineInterpolator(splineInterpolator, points);
        splineInterpolator.StartInterpolation(null, false);

        //Get our LineRenderer instance, and draw the spline in editor.
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetVertexCount(vertexCount);

        for (int c = 0; c < vertexCount; c++)
        {
            float currTime = c * duration / vertexCount;
            Vector3 currPos = splineInterpolator.GetHermiteAtTime(currTime);

            lineRenderer.SetPosition(c, currPos);
        }
    }

	void Update()
	{
		if(test)
		{
			Trigger();
			test = false;
		}
	}

    public void Trigger()
    {
        if (triggered)
        {
            return;
        }

		triggered = true;

        if (points.Count < 2)
        {
            return;
        }

        //Set up our interpolator instance.
        this.m_splineInterpolator = GetComponent(typeof(SplineInterpolator)) as SplineInterpolator;

        SetupSplineInterpolator(this.m_splineInterpolator, points);
        this.m_splineInterpolator.StartInterpolation(null, false);

        //Get our LineRenderer instance, and draw the spline in play mode.
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetVertexCount(vertexCount);

        for (int c = 0; c < vertexCount; c++)
        {
            float currTime = c * duration / vertexCount;
            Vector3 currPos = this.m_splineInterpolator.GetHermiteAtTime(currTime);

            lineRenderer.SetPosition(c, currPos);
        }

        //Begin movement along the spline.
        FollowSpline();
    }

    void SetupSplineInterpolator(SplineInterpolator interp, List<GameObject> gameObjects)
    {
        interp.Reset(splineObject);

        float step = duration / (gameObjects.Count - 1);

        for (int i = 0; i < gameObjects.Count; i++)
        {
            Transform trans = gameObjects[i].transform;
			Vector3 offset = new Vector3(trans.position.x, trans.position.y + 0.4f, trans.position.z);
            interp.AddPoint(gameObjects[i], offset, trans.rotation, step * i, new Vector2(0, 1));
        }
    }

    /// <summary>
    /// Starts moving this GameObject along the spline.
    /// </summary>
    void FollowSpline()
    {
        if (points.Count > 0)
        {
			//Create our GameObject
			splineObject = (GameObject) Instantiate(splineObjectInst, transform.position, transform.rotation);
			splineObject.transform.parent = this.transform;
			m_splineInterpolator.splineObject = splineObject;

            SetupSplineInterpolator(this.m_splineInterpolator, points);

            this.m_splineInterpolator.StartInterpolation(delegate(){
                Destroy (splineObject);
            }, true);

            points[0].gameObject.SendMessage("OnSplinePointReached");
        }
    }
}
