using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ゲーム管理クラス
public class GameManager : MonoBehaviour<GameManager>,IObserver<PlayerEvent>,IObserver<NetworkEvent>
{
	[SerializeField]
	private GameUIManager m_GameUIManager = null;

	[SerializeField,SceneName]
	private string m_OutGameSceneName;

	[SerializeField]
	private MeshCollider m_AreaCollider = null;
	public MeshCollider AreaCollider
	{
		get { return m_AreaCollider; }
	}

	private bool m_IsGameSet = false;
	public static bool IsGameSet // 判定等でよく使うのでstaticで用意
	{
		get
		{
			if (HasInstance)
				return Instance.m_IsGameSet;
			// インスタンスが無ければとりあえずtrueを返す
			return true;
		}
	}

	// 球発射台最大スポーン数
	[SerializeField]
	private int m_MaxNumSpawnBallLaunchers = 10;

	// 球発射台最大スポーンインターバル
	[SerializeField]
	private float m_BallLauncherSpawnInterval = 60;

	// ゲームスタートから初回球発射台スポーンまでのオフセット
	[SerializeField]
	private float m_BallLauncherSpawnStartOffsetTime = 2;

	[SerializeField]
	private float m_FinishGameOffsetTime = 5;

	private bool m_IsOnNetworkError = false;

	private int m_NumSpawnBallLaunchers = 0;

	private NetworkObjectPool m_PrefabPool = null;

	void Start()
	{
		m_PrefabPool = GetComponent<NetworkObjectPool>();
		MonobitEngine.MonobitNetwork.ObjectPool = m_PrefabPool;
		NetworkManager.Instance.AddNetworkEventObserver(this);
		PlayerManager.Instance.AddObserver(this);
	}

	// ネットワーク関連イベント受信用
	public void OnNotify(Observable<NetworkEvent> observer, NetworkEvent e)
	{
		NetworkEventDispatcher dispatcher = new NetworkEventDispatcher(e);
		dispatcher.Dispatch<HostChengedEvent>(OnChangedHost);
		dispatcher.Dispatch<OtherPlayerDisconnectedEvent>(OnDisconnectOtherPlayer);
	}

	// プレイヤー関連イベント受信用
	public void OnNotify(Observable<PlayerEvent> observer, PlayerEvent e)
	{
		EventDispatcher dispatcher = new EventDispatcher(e);
		if (RoomManager.IsHost)
			dispatcher.Dispatch<OnAllPlayerSpawnCompletedEvent>(OnAllPlayerSpawnCompleted);
		dispatcher.Dispatch<OnDownPlayerEvent>(OnDownPlayer);
	}

	// ホストが変わった
	private bool OnChangedHost(HostChengedEvent e)
	{
		if (m_IsGameSet) return false;
		if (m_IsOnNetworkError) return false;
		// ホストがゲーム終了
		m_GameUIManager.OnNetworkError("Host Player is Disconected");
		StartCoroutine(DelayFinishGame(m_FinishGameOffsetTime));
		m_IsOnNetworkError = true;
		return false;
	}

	// 誰かが切断した
	private bool OnDisconnectOtherPlayer(OtherPlayerDisconnectedEvent e)
	{
		if (m_IsGameSet) return false;
		if (m_IsOnNetworkError) return false;
		if (RoomManager.IsHost == false)
			return false;
		if (RoomManager.NumPlayers > 1)
			return false;

		// 自分以外誰も居ないならゲーム終了
		m_GameUIManager.OnNetworkError("Disconected Other Players");
		StartCoroutine(DelayFinishGame(m_FinishGameOffsetTime));
		m_IsOnNetworkError = true;
		return false;

	}

	// 指定時間待ってゲーム終了
	private IEnumerator DelayFinishGame(float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		FinishGame();
	}

	// ゲーム終了
	private void FinishGame()
	{
		ApplicationManager.CursorMgr.SetDefaultSettings();
		ApplicationManager.SceneMgr.FadeToLoadScene(m_OutGameSceneName);
		NetworkManager.Instance.LeaveRoom();
		NetworkGUI.Instance.OnInGameScene(); // OutGameへ遷移
	}

	// 全プレイヤーがスポーンした
	private void OnAllPlayerSpawnCompleted()
	{
		StartCoroutine(DelaySpawnBallLauncher());
	}

	// 指定時間毎にBallLauncherを生成
	private IEnumerator IntervalSpawnBallLauncher(float time)
	{
		while (m_IsGameSet == false)
		{
			yield return new WaitForSeconds(time);
			BallLauncherSpawner.Instance.SpawnLauncher();
			m_NumSpawnBallLaunchers++;
			if (m_NumSpawnBallLaunchers >= m_MaxNumSpawnBallLaunchers)
				yield break; // 最大数スポーン完了
		}
	}

	// 一定時間待ってから最初のBallLauncher生成
	private IEnumerator DelaySpawnBallLauncher()
	{
		yield return new WaitForSeconds(m_BallLauncherSpawnStartOffsetTime);

		BallLauncherSpawner.Instance.SpawnLauncher();
		StartCoroutine(IntervalSpawnBallLauncher(m_BallLauncherSpawnInterval));
	}

	// プレイヤーが倒れた
	private void OnDownPlayer()
	{
		if (PlayerManager.Instance.NumSurvivingPlayers > 1)
			return;

		m_IsGameSet = true;
		// カーソル設定をデフォルトへ
		ApplicationManager.CursorMgr.SetDefaultSettings();
		// 残り一人ならアニメーション再生
		StartCoroutine(NotifyGameSet());
	}
	
	// 決着が付いた事を通知
	private IEnumerator NotifyGameSet()
	{
		yield return new WaitForSeconds(1);
		PlayerManager.Instance.OnGameSet();
		m_GameUIManager.OnGameSet();

		yield return new WaitForSeconds(m_FinishGameOffsetTime);
		FinishGame();
		NetworkGUI.Instance.OnInGameScene();
	}

	protected override void OnDestroy()
	{
#if UNITY_EDITOR
		if (Application.isPlaying == false) return;
#endif
		base.OnDestroy();
		m_PrefabPool.DestroyAll();
		Destroy(m_PrefabPool);
		MonobitEngine.MonobitNetwork.ObjectPool = null;
	}

}
