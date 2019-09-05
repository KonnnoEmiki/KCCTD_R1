using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// プレイヤー管理クラス
public class PlayerManager : ObservableMonoBehaviour<PlayerManager,PlayerEvent>,IObserver<NetworkEvent>
{
	// プレイヤー一覧
	private List<Player> m_Players = new List<Player>();
	public List<Player> Players { get { return m_Players; } }
	// 生存プレイヤー一覧
	private List<Player> m_SurvivingPlayers = new List<Player>();
	public List<Player> SurvivingPlayers { get { return m_SurvivingPlayers; } }
	// 生存プレイヤー数
	private int m_NumSurvivingPlayers = 0;
	public int NumSurvivingPlayers { get { return m_NumSurvivingPlayers; } }
	
	// プレイヤーIDからプレイヤーインスタンスへの変換用
	private Dictionary<int, Player> m_PlayerIDToPlayer = new Dictionary<int, Player>();

	void Start()
	{
		NetworkManager.Instance.AddNetworkEventObserver(this);
	}

	// プレイヤーが生成された(Playerクラスから呼び出し)
	public void OnSpawnPlayer(Player spawnPlayer)
	{
		var camera = spawnPlayer.GetComponent<PlayerCamera>();
		
		// 所有権が無ければPlayerCameraは破棄
		if (spawnPlayer.monobitView.isMine == false)
			Destroy(camera);
		else
			camera.SetActiveCamera(true); // 所有権があればカメラ有効化

		// 各種コンテナへ追加
		m_Players.Add(spawnPlayer);
		m_SurvivingPlayers.Add(spawnPlayer);
		m_PlayerIDToPlayer.Add(spawnPlayer.monobitView.ownerId, spawnPlayer);

		++m_NumSurvivingPlayers;

		// 生成されたことを通知
		NotifyObservers(new OnSpawnPlayerEvent(spawnPlayer));

		// 全員のスポーンが完了したことを通知
		if (m_Players.Count == RoomManager.NumPlayers)
			NotifyObservers(new OnAllPlayerSpawnCompletedEvent());
	}

	// プレイヤーが倒れた(Playerクラスから呼び出し)
	public void OnDown(Player downPlayer)
	{
		--m_NumSurvivingPlayers;
		m_SurvivingPlayers.Remove(downPlayer);

		// プレイヤーが倒れたことを通知
		NotifyObservers(new OnDownPlayerEvent(downPlayer));
	}

	// 決着が付いた
	public void OnGameSet()
	{
		foreach (var player in m_Players)
		{
			if (player == null) continue; // 念の為
			player.OnGameSet();
		}
	}

	// 一番近いプレイヤーを取得
	public Player GetNearestPlayer(Vector3 pos)
	{
		// ※プレイヤーが大量にいる場合はこの検索方法はよろしくない

		Player nearestPlayer = null;
		float minDist = float.MaxValue;
		foreach(var player in m_Players)
		{
			// 念の為nullチェック
			if (player == null) continue;
			// 倒れているなら 
			if (player.IsDown) continue;
			
			// 距離比較
			float dist = (player.transform.position - pos).magnitude;
			if(dist < minDist)
			{
				minDist = dist;
				nearestPlayer = player;
			}
		}

		return nearestPlayer;
	}

	// ネットワーク関連のイベント受信
	public void OnNotify(Observable<NetworkEvent> observer, NetworkEvent e)
	{
		NetworkEventDispatcher dispatcher = new NetworkEventDispatcher(e);
		dispatcher.Dispatch<OtherPlayerDisconnectedEvent>(OnDisconnectedOtherPlayer);
	}
	
	// 誰かが切断した
	public bool OnDisconnectedOtherPlayer(OtherPlayerDisconnectedEvent e)
	{
		// 管理対象のプレイヤーか
		if(m_PlayerIDToPlayer.ContainsKey(e.m_OtherPlayer.ID) == false)
			return false;

		// ゲーム終了時の切断か
		if (GameManager.IsGameSet)
			return false;

		// 切断されたプレイヤーは管理下から除外
		var disconnectedPlayer = m_PlayerIDToPlayer[e.m_OtherPlayer.ID];
		m_Players.Remove(disconnectedPlayer);
		m_SurvivingPlayers.Remove(disconnectedPlayer);
		--m_NumSurvivingPlayers;

		return false;
	}

}
