using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// サーバーとの接続,切断、ルーム作成,入室,退室、ゲーム開始のUI表示用クラス
public class NetworkGUI : MonobitEngine.SingletonMonoBehaviour<NetworkGUI>,IObserver<NetworkEvent>
{
	private string m_PlayerName = "Player 1";
	private string m_RoomName = "Room 1";
	private string m_SearchPlayerName = "";

	private static readonly string OutGameSceneName = "OutGame";
	private static readonly string InGameSceneName = "InGame";

	private bool m_IsInGameScene = false;
	private bool m_OnPushLeftOrDisconnectButton = false;

	private static readonly int BaseGUIWidth = 75;

    public static bool gs = false;

    public static bool roommaster = true;

    private bool gsf=true;

    private void Start()
	{
		NetworkManager.Instance.AddNetworkEventObserver(this);
	}

	private void OnGUI()
	{
		if (MonobitEngine.MonobitNetwork.isConnect == false)
		{
			OnGui_Connect();
			return;
		}
		
		if (MonobitEngine.MonobitNetwork.inRoom == false)
		{
			OnGui_CreateRoom();
			OnGui_SearchPlayer();
			OnGui_SearchPlayerResult();
			OnGui_ChooseRoom();
			OnGui_Disconnect();
		}
		else if(m_IsInGameScene == false)
		{
			OnGui_StartGame();
			OnGui_LeaveRoom();
		}
		else
		{
			// ゲーム中にカーソルロックが外れていれば退室用UI表示
			if (ApplicationManager.CursorMgr.IsCursorLocked == false && GameManager.IsGameSet == false)
				OnGui_LeaveRoom();
		}
		
	}

	// サーバーへの接続用GUI
	private void OnGui_Connect()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label("Player Name");
		m_PlayerName = GUILayout.TextField(m_PlayerName, GUILayout.Width(BaseGUIWidth * 2));

		if (GUILayout.Button("Connect to Server", GUILayout.Width(BaseGUIWidth * 2)))
		{
			if (string.IsNullOrEmpty(m_PlayerName) == false)
				NetworkManager.Instance.ConnectToServer(m_PlayerName);
		}

		GUILayout.EndHorizontal();
	}

	// サーバーから切断用GUI
	private void OnGui_Disconnect()
	{
		if (GUILayout.Button("Disconnect from Server", GUILayout.Width(BaseGUIWidth * 2)))
		{
			m_OnPushLeftOrDisconnectButton = true;
			NetworkManager.Instance.DisconnectFromServer();
		}
	}

	private void OnGui_StartGame()
	{
		var roomData = MonobitEngine.MonobitNetwork.room;
		string playerInfo = "(" + roomData.playerCount + "/" + ((roomData.maxPlayers == 0) ? "-" : roomData.maxPlayers.ToString()) + ")";
		GUILayout.Label("Num Players : " + roomData.name + playerInfo, GUILayout.Width(BaseGUIWidth * 3));

		if (RoomManager.IsHost == false) // ゲームを開始出来るのはホストのみに制限
		{
			GUILayout.Label("Waiting for host to start...");
			return;
		}

		// ルーム内に自分しか居なければ他のプレイヤーを待つ
		if(MonobitEngine.MonobitNetwork.room.playerCount <= 1)
		{
			GUILayout.Label("Waiting for other player...");
			return;
		}

		if (GUILayout.Button("Game Start", GUILayout.Width(BaseGUIWidth * 2)))
        {
            monobitView.RPC("LoadInGameScene", MonobitEngine.MonobitTargets.OthersBuffered);
			roomData.visible = false; // ゲーム開始後はルームが他プレイヤーから見えないように
			LoadInGameScene();
		}
	}

	// ルームから退室用GUI
	private void OnGui_LeaveRoom()
	{
		if (GUILayout.Button("Leave from Room", GUILayout.Width(BaseGUIWidth * 2)))
		{
			m_OnPushLeftOrDisconnectButton = false;
            OnInGameScene();
            NetworkManager.Instance.LeaveRoom();
		}
	}

	// ルーム作成用GUI
	private void OnGui_CreateRoom()
    {
        roommaster = true;
        gs = false;
		GUILayout.Label("Create Room", new GUIStyle { fontStyle = FontStyle.Bold });
		GUILayout.BeginHorizontal();
		GUILayout.Label("Room Name");
		m_RoomName = GUILayout.TextField(m_RoomName, GUILayout.Width(BaseGUIWidth * 2));

		if (GUILayout.Button("Create", GUILayout.Width(BaseGUIWidth)))
		{
			if (string.IsNullOrEmpty(m_RoomName) == false)
				NetworkManager.Instance.CreateRoom(m_RoomName);
		}
		GUILayout.EndHorizontal();
	}

	// 他プレイヤー検索用GUI
	private void OnGui_SearchPlayer()
	{
		GUILayout.Label("Search Player", new GUIStyle { fontStyle = FontStyle.Bold });
		GUILayout.BeginHorizontal();
		GUILayout.Label("Player Name : ");
		m_SearchPlayerName = GUILayout.TextField(m_SearchPlayerName, GUILayout.Width(BaseGUIWidth * 2));
		string serachButtonName = "Search";
		if (GUILayout.Button(serachButtonName))
		{
			if (string.IsNullOrEmpty(m_SearchPlayerName) == false)
				NetworkManager.Instance.SearchPlayers(m_SearchPlayerName.Split(','));
		}
		GUILayout.EndHorizontal();
	}

	// 他プレイヤー検索結果表示用GUI
	private void OnGui_SearchPlayerResult()
	{
		if (NetworkManager.Instance.IsFindSearchPlayer == false) return;

		GUILayout.Label("Search Result", new GUIStyle { fontStyle = FontStyle.Bold });
		foreach(var player in MonobitEngine.MonobitNetwork.SearchPlayerList)
		{
			if(player.connect == false)
			{
				GUILayout.Label(player.playerName + " is Offline");
				continue;
			}

			if(player.inRoom == false)
			{
				GUILayout.Label(player.playerName + " is not in Room");
				continue;
			}

			GUILayout.Label(player.playerName + " is in" + player.roomName);

			if (GUILayout.Button("Join", GUILayout.Width(BaseGUIWidth * 2)))
				NetworkManager.Instance.JoinRoom(player.roomName);
		}
	}

	// 既存ルームへの入室用GUI
	private void OnGui_ChooseRoom()
	{
        gsf = true;
        var roomDataList = MonobitEngine.MonobitNetwork.GetRoomData();
		if (roomDataList.Length < 1)
			return; // 他にルームが見つからなかった

		GUILayout.Label("Choose Room", new GUIStyle { fontStyle = FontStyle.Bold });
		foreach (var roomData in roomDataList)
		{
			if (roomData.playerCount >= roomData.maxPlayers)
				continue;

			GUILayout.BeginHorizontal();
			// ルーム情報表示
			string roomName = roomData.name;
			string playerInfo = "(" + roomData.playerCount + "/" + ((roomData.maxPlayers == 0) ? "-" : roomData.maxPlayers.ToString()) + ")";
			GUILayout.Label("Room Name: " + roomName + playerInfo, GUILayout.Width(BaseGUIWidth * 3));

			if (GUILayout.Button("Join", GUILayout.Width(BaseGUIWidth)))
				NetworkManager.Instance.JoinRoom(roomData.name); // 入室
			GUILayout.EndHorizontal();
		}
	}

    void Update()
    {
        var roomData = MonobitEngine.MonobitNetwork.room;
        if (roomData.visible == false && gsf == true)
        {
            gs = true;
            gsf = false;
        }
    }

	// InGameSceneロード
	[MunRPC]
	private void LoadInGameScene()
	{
		if (m_IsInGameScene) return;
        ApplicationManager.SceneMgr.FadeToLoadScene(InGameSceneName);
		m_IsInGameScene = true;
	}

	// OutGameSceneロード
	private void LoadOutGameScene()
	{
		// InGame中にDisconnectまたはLeftボタンを押した場合OutGameへ遷移する
		if (m_IsInGameScene == false) return;
		if (m_OnPushLeftOrDisconnectButton == false) return;

		ApplicationManager.SceneMgr.FadeToLoadScene(OutGameSceneName);
		
		m_IsInGameScene = false;
		m_OnPushLeftOrDisconnectButton = false;
	}

	// ネットワーク関連のイベント受信
	public void OnNotify(Observable<NetworkEvent> observer, NetworkEvent notifyObject)
	{
		NetworkEventDispatcher dispatcher = new NetworkEventDispatcher(notifyObject);
		dispatcher.Dispatch<LeftRoomEvent>((e) => { LoadOutGameScene(); return false; });
	}

	// ゲーム終了時,ゲーム中にホストが抜けた場合などに使用
	public void OnOutGameScene()
    {
        LoadInGameScene();
        m_IsInGameScene = false;
        gs = false;
        gsf = true;
	}

    public void OnInGameScene()
    {
        LoadInGameScene();
        m_IsInGameScene = false;
        gs = false;
        gsf = true;
    }

}
