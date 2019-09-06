using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// アプリケーション管理クラス
public class ApplicationManager : MonoBehaviour<ApplicationManager>
{
	private Fade m_Fade = null;
	private CursorManager m_CursorManager = null;
	private SceneManager m_SceneManager = null;

	public static Fade Fade
	{
		get
		{
			if (HasInstance) return Instance.m_Fade;
			return null;
		}
	}

	public static CursorManager CursorMgr
	{
		get
		{
			if (HasInstance) return Instance.m_CursorManager;
			return null;
		}
	}

	public static SceneManager SceneMgr
	{
		get
		{
			if (HasInstance) return Instance.m_SceneManager;
			return null;
		}
	}


	void Start()
    {
		m_CursorManager = GetComponent<CursorManager>();
		m_SceneManager = GetComponent<SceneManager>();

		m_Fade = GetComponentInChildren<Fade>();
    }
}
