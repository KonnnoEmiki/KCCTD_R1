using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ビルボードクラス
public class Billboard : MonoBehaviour
{
	[SerializeField]
	private bool m_UseLateUpdate = false;

	void Start()
	{
		MathUtil.BillboardMainCam(transform);
	}

	void Update()
    {
		if(m_UseLateUpdate == false)
			MathUtil.BillboardMainCam(transform);
    }

	void LateUpdate()
	{
		if (m_UseLateUpdate)
			MathUtil.BillboardMainCam(transform);
	}
}
