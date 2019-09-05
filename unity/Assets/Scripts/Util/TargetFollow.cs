using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 座標を指定したターゲットと同じ座標に設定するクラス
public class TargetFollow : MonoBehaviour
{
	public Transform m_Target = null;

    void LateUpdate()
    {
		if (m_Target == null) return;
		transform.position = m_Target.position;
		transform.rotation = m_Target.rotation;
    }
}
