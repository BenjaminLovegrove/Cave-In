using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum InterpolationState { STOPPED, RESET, ONCE, };
public delegate void OnEndCallback();

public class SplineInterpolator : MonoBehaviour
{
	internal class SplineNode
	{
		internal Vector3 Point;
		internal Quaternion Rot;
		internal float Time;
		internal Vector2 EaseIO;

		internal SplineNode(Vector3 p, Quaternion q, float t, Vector2 io) { Point = p; Rot = q; Time = t; EaseIO = io; }
		internal SplineNode(SplineNode o) { Point = o.Point; Rot = o.Rot; Time = o.Time; EaseIO = o.EaseIO; }
	}

    //Members
	private List<SplineNode> m_nodes = new List<SplineNode>();
	private InterpolationState m_state = InterpolationState.STOPPED;
	private bool m_rotations;

    float m_currentTime;
    int m_currentIndex = 1;

	private OnEndCallback mOnEndCallback;

	void Awake()
	{
		Reset();
	}

	public void StartInterpolation(OnEndCallback endCallback, bool bRotations)
	{
		if (m_state != InterpolationState.RESET)
			throw new System.Exception("First reset, add points and then call here");

		m_state = InterpolationState.ONCE;
		m_rotations = bRotations;
		mOnEndCallback = endCallback;

		SetInput();
	}

	public void Reset()
	{
		m_nodes.Clear();
		m_state = InterpolationState.RESET;
		m_currentIndex = 1;
		m_currentTime = 0;
		m_rotations = false;
	}

	public void AddPoint(Vector3 pos, Quaternion quat, float timeInSeconds, Vector2 easeInOut)
	{
		if (m_state != InterpolationState.RESET)
			throw new System.Exception("Cannot add points after start");

		m_nodes.Add(new SplineNode(pos, quat, timeInSeconds, easeInOut));
	}

	private void SetInput()
	{
		if (m_nodes.Count < 2)
			throw new System.Exception("Invalid number of points");

		if (m_rotations)
		{
			for (int c = 1; c < m_nodes.Count; c++)
			{
				SplineNode node = m_nodes[c];
				SplineNode prevNode = m_nodes[c - 1];

				// Always interpolate using the shortest path -> Selective negation
				if (Quaternion.Dot(node.Rot, prevNode.Rot) < 0)
				{
					node.Rot.x = -node.Rot.x;
					node.Rot.y = -node.Rot.y;
					node.Rot.z = -node.Rot.z;
					node.Rot.w = -node.Rot.w;
				}
			}
		}

		m_nodes.Insert(0, m_nodes[0]);
		m_nodes.Add(m_nodes[m_nodes.Count - 1]);
	}

	private void Update()
	{
		if (m_state == InterpolationState.RESET || m_state == InterpolationState.STOPPED || m_nodes.Count < 4)
			return;

		m_currentTime += Time.deltaTime;

		// We advance to next point in the path
		if (m_currentTime >= m_nodes[m_currentIndex + 1].Time)
		{
			if (m_currentIndex < m_nodes.Count - 3)
			{
				m_currentIndex++;
			}
			else
			{
				m_state = InterpolationState.STOPPED;

				// We stop right in the end point
				transform.position = m_nodes[m_nodes.Count - 2].Point;

				if (m_rotations)
					transform.rotation = m_nodes[m_nodes.Count - 2].Rot;

				// We call back to inform that we are ended
				if (mOnEndCallback != null)
					mOnEndCallback();
			}
		}

		if (m_state != InterpolationState.STOPPED)
		{
			// Calculates the t param between 0 and 1
			float param = (m_currentTime - m_nodes[m_currentIndex].Time) / (m_nodes[m_currentIndex + 1].Time - m_nodes[m_currentIndex].Time);

			// Smooth the param
			param = MathUtils.Ease(param, m_nodes[m_currentIndex].EaseIO.x, m_nodes[m_currentIndex].EaseIO.y);

			transform.position = GetHermiteInternal(m_currentIndex, param);

			if (m_rotations)
			{
				transform.rotation = GetSquad(m_currentIndex, param);
			}
		}
	}

	Quaternion GetSquad(int idxFirstPoint, float t)
	{
		Quaternion Q0 = m_nodes[idxFirstPoint - 1].Rot;
		Quaternion Q1 = m_nodes[idxFirstPoint].Rot;
		Quaternion Q2 = m_nodes[idxFirstPoint + 1].Rot;
		Quaternion Q3 = m_nodes[idxFirstPoint + 2].Rot;

		Quaternion T1 = MathUtils.GetSquadIntermediate(Q0, Q1, Q2);
		Quaternion T2 = MathUtils.GetSquadIntermediate(Q1, Q2, Q3);

		return MathUtils.GetQuatSquad(t, Q1, Q2, T1, T2);
	}

	public Vector3 GetHermiteInternal(int idxFirstPoint, float t)
	{
		float t2 = t * t;
		float t3 = t2 * t;

		Vector3 P0 = m_nodes[idxFirstPoint - 1].Point;
		Vector3 P1 = m_nodes[idxFirstPoint].Point;
		Vector3 P2 = m_nodes[idxFirstPoint + 1].Point;
		Vector3 P3 = m_nodes[idxFirstPoint + 2].Point;

		float tension = 0.5f;	// 0.5 equivale a catmull-rom

		Vector3 T1 = tension * (P2 - P0);
		Vector3 T2 = tension * (P3 - P1);

		float Blend1 = 2 * t3 - 3 * t2 + 1;
		float Blend2 = -2 * t3 + 3 * t2;
		float Blend3 = t3 - 2 * t2 + t;
		float Blend4 = t3 - t2;

		return Blend1 * P1 + Blend2 * P2 + Blend3 * T1 + Blend4 * T2;
	}

	public Vector3 GetHermiteAtTime(float timeParam)
	{
		if (timeParam >= m_nodes[m_nodes.Count - 2].Time)
			return m_nodes[m_nodes.Count - 2].Point;

		int c;
		for (c = 1; c < m_nodes.Count - 2; c++)
		{
			if (m_nodes[c].Time > timeParam)
				break;
		}

		int idx = c - 1;
		float param = (timeParam - m_nodes[idx].Time) / (m_nodes[idx + 1].Time - m_nodes[idx].Time);
		param = MathUtils.Ease(param, m_nodes[idx].EaseIO.x, m_nodes[idx].EaseIO.y);

		return GetHermiteInternal(idx, param);
	}
}