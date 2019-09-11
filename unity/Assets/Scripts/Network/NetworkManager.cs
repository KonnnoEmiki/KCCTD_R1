using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ネットワーク全体の管理クラス
[RequireComponent(typeof(ConnectionManager),typeof(RoomManager))]
public class NetworkManager : MonoBehaviour<NetworkManager>
{
	[SerializeField]
	private ConnectionManager m_ConnectionManager = null;
	[SerializeField]
	private RoomManager m_RoomManager = null;
	
	// 検索したプレイヤーが見つかったか
	public bool IsFindSearchPlayer
	{
		get { return m_ConnectionManager == null ? false : m_ConnectionManager.IsFIndSearchPlayers;}
	}

	[SerializeField]
	private int m_UpdateStreamRate = 10;
	[SerializeField]
	private int m_RPCSendRate = 20;

	protected override void Awake()
	{
		base.Awake();
		UnityEngine.SceneManagement.SceneManager.sceneUnloaded += SceneUnloaded;

		if (m_ConnectionManager == null)
			m_ConnectionManager = GetComponent<ConnectionManager>();
		if (m_RoomManager == null)
			m_RoomManager = GetComponent<RoomManager>();

		// 同期頻度設定
		MonobitEngine.MonobitNetwork.updateStreamRate = m_UpdateStreamRate;
		// RPCの送信頻度設定
		MonobitEngine.MonobitNetwork.sendRate = m_RPCSendRate;
	}

	// 通知対象へ追加
	public void AddNetworkEventObserver(IObserver<NetworkEvent> observer)
	{
		if (m_ConnectionManager == null) return;
		m_ConnectionManager.AddObserver(observer);
	}

	// 通知対象から除外
	public void RemoveNetworkEventObserver(IObserver<NetworkEvent> observer)
	{
		if (m_ConnectionManager == null) return;
		m_ConnectionManager.RemoveObserver(observer);
	}

	// サーバーへ接続
	public void ConnectToServer(string playerName)
	{
		if (m_ConnectionManager == null) return;
		m_ConnectionManager.ConnectToServer(playerName);
	}

	// サーバーから切断
	public void DisconnectFromServer()
	{
		if (m_ConnectionManager == null) return;

		m_ConnectionManager.DisconnectFromServer();
	}

	// ルーム作成
	public void CreateRoom(string roomName)
	{
		if (m_RoomManager == null) return;
        Player.LifeCount = Player.Stamina;

        m_RoomManager.CreateRoom(roomName);
	}

	// ルームへ入室
	public void JoinRoom(string roomName)
	{
		if (m_RoomManager == null) return;
        Player.LifeCount = Player.Stamina;

		m_RoomManager.JoinRoom(roomName);
	}

	// ルームから退室
	public void LeaveRoom()
	{
		if (m_RoomManager == null) return;

		m_RoomManager.LeaveRoom();
	}

	// 切断リクエスト済みか
	public bool IsDisconnectRequest()
	{
		if (m_ConnectionManager == null) return false;
		return m_ConnectionManager.IsDisconnectRequest;
	}
	
	// プレイヤー検索
	public void SearchPlayers(string[] searchPlayerNames)
	{
		m_ConnectionManager.SearchPlayers(searchPlayerNames);
	}

	// シーンアンロード時
	void SceneUnloaded(Scene scene)
	{
		m_ConnectionManager.RemoveMissingObservables();
	}

}
