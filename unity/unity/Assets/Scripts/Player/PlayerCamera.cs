using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerCamera : MonoBehaviour,IObserver<bool>,IObserver<PlayerEvent>
{
	private string m_DefaultCameraInputXAxisName;
	private string m_DefaultCameraInputYAxisName;

	// 三人称視点カメラ
	[SerializeField]
	Cinemachine.CinemachineFreeLook m_ThirdPersonCamera = null;
	// デスカメラ
	[SerializeField]
	Cinemachine.CinemachineFreeLook m_DeathCamera = null;
	
	private Player m_Player = null;

	void Start()
	{
		m_Player = GetComponent<Player>();

		if (m_Player.monobitView.isMine == false) return;

		if (m_ThirdPersonCamera == null) return;
		m_DefaultCameraInputXAxisName = m_ThirdPersonCamera.m_XAxis.m_InputAxisName;
		m_DefaultCameraInputYAxisName = m_ThirdPersonCamera.m_YAxis.m_InputAxisName;

		ApplicationManager.CursorMgr.AddObserver(this);
		PlayerManager.Instance.AddObserver(this);
	}
	
	void OnDestroy()
	{
		// PlayerCameraコンポーネントが削除されたらCinemachineのカメラコンポーネントも不要なので削除
		if (m_ThirdPersonCamera != null) Destroy(m_ThirdPersonCamera);
		if (m_DeathCamera != null) Destroy(m_DeathCamera);
	}

	// カメラ有効化 or 無効化
	public void SetActiveCamera(bool enable)
	{
		if (m_ThirdPersonCamera == null) return;
		m_ThirdPersonCamera.gameObject.SetActive(enable);
	}

	// カメラ操作有効化 or 無効化
	public void SetActiveControll(bool enable)
	{
		var activeCam = m_Player.IsDown ? m_DeathCamera : m_ThirdPersonCamera;

		if (enable == false)
		{
			activeCam.m_XAxis.m_InputAxisName = string.Empty;
			activeCam.m_YAxis.m_InputAxisName = string.Empty;
			// 無効化する前の入力値が残って動きが止まらないので手動でリセット
			activeCam.m_XAxis.m_InputAxisValue = 0;
			activeCam.m_YAxis.m_InputAxisValue = 0;
		}
		else
		{
			activeCam.m_XAxis.m_InputAxisName = m_DefaultCameraInputXAxisName;
			activeCam.m_YAxis.m_InputAxisName = m_DefaultCameraInputYAxisName;
		}
	}

	// カーソルがロックされた
	public void OnNotify(Observable<bool> observer, bool isCursorLocked)
	{
		if (m_Player == null) return;
		if (m_Player.monobitView.isMine == false) return;
		SetActiveControll(isCursorLocked);
	}

	// プレイヤー関連イベント発生
	public void OnNotify(Observable<PlayerEvent> observer, PlayerEvent e)
	{
		EventDispatcher dispatcher = new EventDispatcher(e);
		dispatcher.Dispatch<OnDownPlayerEvent>(OnDownPlayer);
	}

	// プレイヤーが倒れた
	private void OnDownPlayer(OnDownPlayerEvent e)
	{
		// 自身の親オブジェクトのプレイヤーじゃなければ
		if (e.m_DownPlayer != m_Player) return;

		// カメラ切替
		// カメラが無効化されていればその状態を引き継ぐ
		m_DeathCamera.gameObject.SetActive(m_ThirdPersonCamera.gameObject.activeSelf);
		m_DeathCamera.m_XAxis.Value = m_ThirdPersonCamera.m_XAxis.Value;
		m_DeathCamera.m_YAxis.Value = m_ThirdPersonCamera.m_YAxis.Value;

		m_ThirdPersonCamera.gameObject.SetActive(false);
	}
}
