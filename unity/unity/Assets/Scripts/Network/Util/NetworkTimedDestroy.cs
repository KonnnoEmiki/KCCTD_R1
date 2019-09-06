using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 設定した時間後にMonobitNetwork.Destoryを実行するクラス
public class NetworkTimedDestroy : MonobitEngine.MonoBehaviour
{
	public float m_DestroyTime;
	public bool m_IsPoolObject = false;

	private void Start()
	{
		if(monobitView.isMine)
			Invoke("DoDestroy", m_DestroyTime);
	}

	void DoDestroy()
	{
		MonobitEngine.MonobitNetwork.Destroy(gameObject);
	}
}
