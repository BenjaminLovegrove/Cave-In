using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Splines/Spline Controller")]
[RequireComponent(typeof(SplineInterpolator))]
[RequireComponent(typeof(LineRenderer))]
public class SplineController : MonoBehaviour
{
	public GameObject splineObject; //The spline object to move.

    public List<GameObject> points = new List<GameObject>();    //The spline points

    public int vertexCount = 100;                               //Number of vertices to use for rendering/moving along the spline. (Editor/Play Mode)

    public float duration = 10;                                 //Duration for spline movement.
    public bool showPointsInPlayMode = true;                    //Show points in play mode for debugging.

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

	void Start()
	{
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

        //Debugging: Draw Spline GameObjects in play mode.
        if (!showPointsInPlayMode)
        {
            DisableTransforms();
        }

        else
        {
            EnableTransforms();
        }

	}

	void SetupSplineInterpolator(SplineInterpolator interp, List<GameObject> gameObjects)
	{
		interp.Reset();

		float step = duration / (gameObjects.Count - 1);

		for (int i = 0; i < gameObjects.Count; i++)
		{
			Transform transform = gameObjects[i].transform;
			interp.AddPoint(gameObjects[i], transform.position, transform.rotation, step * i, new Vector2(0, 1));
		}
	}

	/// <summary>
	/// Disables the spline objects.
	/// </summary>
	void DisableTransforms()
	{
        if (points.Count > 1)
        {
            foreach (GameObject point in points)
            {
                point.SetActive(false);
            }
        }
	}

    /// <summary>
    /// Enables the spline objects.
    /// </summary>
    void EnableTransforms()
    {
        if (points.Count > 0)
        {
            foreach (GameObject point in points)
            {
                point.SetActive(true);
            }
        }
    }

	/// <summary>
	/// Starts moving this GameObject along the spline.
	/// </summary>
	void FollowSpline()
	{
		if (points.Count > 0)
		{
			SetupSplineInterpolator(this.m_splineInterpolator, points);
			this.m_splineInterpolator.StartInterpolation(delegate(){
				Destroy (splineObject);
			}, true);
			points[0].gameObject.SendMessage("OnSplinePointReached");
		}
	}
}