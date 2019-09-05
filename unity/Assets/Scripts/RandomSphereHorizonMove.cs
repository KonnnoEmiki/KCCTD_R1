using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 球面上のランダムな位置に移動させるクラス
public class RandomSphereHorizonMove : MonoBehaviour
{
	private bool m_IsMove = false;
	public bool IsMove { get { return m_IsMove; } }

	// 球面上のランダムな位置に移動
	public void Move(Vector3 areaCenter,float areaRadius,float moveTime, bool slerp = false)
	{
		if (m_IsMove) return;

		Vector3 from = transform.position;
		Vector3 to = areaCenter;
		
		Vector3 spherePos = Random.onUnitSphere;
		spherePos.y = Mathf.Abs(spherePos.y);
		to += spherePos * areaRadius;
	
		if (slerp)
			StartCoroutine(Slerp(from, to, moveTime));
		else
			StartCoroutine(Lerp(from, to, moveTime));
	}

	// 線形補間ver
	IEnumerator Lerp(Vector3 from,Vector3 to,float lerpTime)
	{
		m_IsMove = true;
		float timer = 0;
		while(timer < lerpTime)
		{
			transform.position = Vector3.Lerp(from, to, timer / lerpTime);
			timer += Time.deltaTime;
			yield return null;
		}
		transform.position = Vector3.Lerp(from, to, 1);
		m_IsMove = false;
	}

	// 球面補間ver
	IEnumerator Slerp(Vector3 from, Vector3 to, float lerpTime)
	{
		m_IsMove = true;
		float timer = 0;
		while (timer < lerpTime)
		{
			transform.position = Vector3.Slerp(from, to, timer / lerpTime);
			timer += Time.deltaTime;
			yield return null;
		}
		transform.position = Vector3.Lerp(from, to, 1);
		m_IsMove = false;

	}
}
