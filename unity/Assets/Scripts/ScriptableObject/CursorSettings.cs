using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// カーソル設定変更時に使用(CursorManagerにて)

[System.Serializable]
public class CursorData
{
	public bool m_Visible;
	public CursorLockMode m_LockMode;
}

public class CursorSettings : ScriptableObject
{
	public CursorData m_CursorData;
}
