using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ボールクラス
[RequireComponent(typeof(Rigidbody),typeof(NetworkTimedDestroy))]
public class Ball : MonobitEngine.MonoBehaviour
{
	[SerializeField]
	private float m_MinDestroyTime;
	[SerializeField]
	private float m_MaxDestroyTime;

	private Rigidbody m_RB = null;
	private NetworkTimedDestroy m_TimedDestroy = null;

	void OnEnable()
	{
		if(m_RB == null)
			m_RB = GetComponent<Rigidbody>();
		m_TimedDestroy = null;
		m_RB.velocity = Vector3.zero;
		m_RB.useGravity = false;
	}

	// 発射
	[MunRPC]
	public void Shot(float speed)
	{
		if(monobitView.isMine)
			monobitView.RPC("Shot", MonobitEngine.MonobitTargets.Others,speed);
		
		m_RB.useGravity = true;
		m_RB.AddForce(transform.forward * speed, ForceMode.Impulse);
	}

	// 削除タイマー開始
	[MunRPC]
	public void StartDestroyTimer()
	{
		if (m_TimedDestroy != null) return;
		if (monobitView.isMine)
			monobitView.RPC("StartDestroyTimer", MonobitEngine.MonobitTargets.Others);
		m_TimedDestroy = gameObject.AddComponent<NetworkTimedDestroy>();
		m_TimedDestroy.m_IsPoolObject = true;
		m_TimedDestroy.m_DestroyTime = Random.Range(m_MinDestroyTime, m_MaxDestroyTime);
	}

}
