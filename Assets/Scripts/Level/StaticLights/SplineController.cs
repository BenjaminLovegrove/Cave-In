using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Splines/Spline Controller")]
[RequireComponent(typeof(SplineInterpolator))]
[RequireComponent(typeof(LineRenderer))]
public class SplineController : MonoBehaviour
{
    public List<GameObject> points = new List<GameObject>();    //The spline points

    public int vertexCount = 100;                               //Number of vertices to use for rendering/moving along the spline. (Editor/Play Mode)

    public float duration = 10;                                 //Duration for spline movement.
    public bool showPointsInPlayMode = true;                    //Show points in play mode for debugging.

    //Members
    private SplineInterpolator m_splineInterpolator;            //The spline interpolator instance.
    private Transform[] m_transforms;                           //The transforms of our spline points.

	void OnDrawGizmos()
	{
        //Convert our points into an array of transforms.
		Transform[] trans = GetTransforms();
		if (trans.Length < 2)
        {
			return;
        }

        //Set up our interpolator instance.
		SplineInterpolator splineInterpolator = GetComponent(typeof(SplineInterpolator)) as SplineInterpolator;
		SetupSplineInterpolator(splineInterpolator, trans);
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
		m_transforms = GetTransforms();

        if (m_transforms.Length < 2)
        {
            return;
        }

        //Set up our interpolator instance.
        this.m_splineInterpolator = GetComponent(typeof(SplineInterpolator)) as SplineInterpolator;
        SetupSplineInterpolator(this.m_splineInterpolator, m_transforms);
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

	void SetupSplineInterpolator(SplineInterpolator interp, Transform[] trans)
	{
		interp.Reset();

		float step = duration / (trans.Length - 1);

		int c;
		for (c = 0; c < trans.Length; c++)
		{
            interp.AddPoint(trans[c].position, trans[c].rotation, step * c, new Vector2(0, 1));
		}
	}


	/// <summary>
	/// Returns point transforms. Ordered by their index in the point list.
	/// </summary>
	Transform[] GetTransforms()
	{
        if(points.Count > 0)
        {
            List<Transform> transforms = new List<Transform>();

            foreach(GameObject point in points)
            {
                transforms.Add(point.transform);
            }

            return transforms.ToArray();
        }

		return null;
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
		if (m_transforms.Length > 0)
		{
			SetupSplineInterpolator(this.m_splineInterpolator, m_transforms);
			this.m_splineInterpolator.StartInterpolation(null, true);
		}
	}
}