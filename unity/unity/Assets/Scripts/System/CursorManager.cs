using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// カーソル管理クラス
public class CursorManager : ObservableMonoBehaviour<bool>
{
	[SerializeField]
	private CursorSettings m_CursorSettings = null;
	private CursorData m_DefaultCursorSettings = null;

	private bool m_IsDefaultSettings = true;

	public bool IsCursorLocked { get { return Cursor.lockState == CursorLockMode.Locked; } }

	void Awake()
	{
		m_DefaultCursorSettings = new CursorData { m_Visible = Cursor.visible, m_LockMode = Cursor.lockState };
		m_IsDefaultSettings = true;
	}

	// 予め用意したカーソル設定適応
	public void SetCursorSettings()
	{
		if (m_CursorSettings == null) return;

		Cursor.visible = m_CursorSettings.m_CursorData.m_Visible;
		Cursor.lockState = m_CursorSettings.m_CursorData.m_LockMode;
		m_IsDefaultSettings = false;
		NotifyObservers(IsCursorLocked);
	}

	// デフォルトのカーソル設定適応
	public void SetDefaultSettings()
	{
		if (m_DefaultCursorSettings == null) return;

		Cursor.visible = m_DefaultCursorSettings.m_Visible;
		Cursor.lockState = m_DefaultCursorSettings.m_LockMode;
		m_IsDefaultSettings = true;
		NotifyObservers(IsCursorLocked);
	}

	// 現在デフォルトの設定なら予め用意したカーソル設定を
	// 予め用意したカーソル設定ならデフォルトの設定へ切替
	public void ToggleSettings()
	{
		if (m_IsDefaultSettings)
			SetCursorSettings();
		else
			SetDefaultSettings();
	}

}
