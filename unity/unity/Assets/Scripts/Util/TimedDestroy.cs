using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 指定時間経過後にDestroyを呼ぶクラス(オブジェクトプール対応)
public class TimedDestroy : MonoBehaviour
{
	public float m_DestroyTime;
	public bool m_IsPoolObject = false;

	private void Start()
	{
		Invoke("DoDestroy", m_DestroyTime);
	}

	void DoDestroy()
    {
		if (m_IsPoolObject)
		{
			Destroy(this);
			ObjectPool.Instance.DestroyObject(gameObject);
		}
		else
			Destroy(gameObject);
		
    }
}
