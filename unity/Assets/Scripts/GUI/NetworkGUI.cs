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

    public static bool roommaster = false;

    public static int stageselect = 0;

    public static int gamemode = 0;

    public static bool Ballflag = true;
    public static bool Trapflag = true;
    public static bool TPSflag = true;
    public static bool Itemflag = true;

    private bool gsf = true;

    public GUIStyle button;
    public GUIStyle Label;
    public GUIStyle TagLabel;
    public GUIStyle TextField;
    public GUIStyle window;

    private void Start()
	{
		NetworkManager.Instance.AddNetworkEventObserver(this);
	}

	private void OnGUI()
	{
		if (MonobitEngine.MonobitNetwork.isConnect == false)
		{
            GUILayout.BeginVertical(window, GUILayout.Width(BaseGUIWidth * 8));

            OnGui_Connect();

            GUILayout.EndVertical();
            return;
		}
		
		if (MonobitEngine.MonobitNetwork.inRoom == false)
		{
            GUILayout.BeginVertical(window, GUILayout.Width(BaseGUIWidth * 10));
            OnGui_CreateRoom();
			OnGui_SearchPlayer();
			OnGui_SearchPlayerResult();
			OnGui_ChooseRoom();
			OnGui_Disconnect();
            GUILayout.EndVertical();
        }
		else if(m_IsInGameScene == false)
		{
            GUILayout.BeginVertical(window, GUILayout.Width(BaseGUIWidth * 8));
            OnGui_StartGame();
            OnGui_ChooseStage();
            GUILayout.Space(10);
            OnGui_LeaveRoom();
            GUILayout.EndVertical();
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
        GUILayout.FlexibleSpace();
        GUILayout.Label("Player Name",Label, GUILayout.Width(BaseGUIWidth * 2));
		m_PlayerName = GUILayout.TextField(m_PlayerName, TextField, GUILayout.Width(BaseGUIWidth*4));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Connect to Server", button, GUILayout.Width(BaseGUIWidth *4)))
		{
			if (string.IsNullOrEmpty(m_PlayerName) == false)
				NetworkManager.Instance.ConnectToServer(m_PlayerName);
		}
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

	// サーバーから切断用GUI
	private void OnGui_Disconnect()
	{
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Disconnect from Server", button, GUILayout.Width(BaseGUIWidth * 5)))
		{
			m_OnPushLeftOrDisconnectButton = true;
			NetworkManager.Instance.DisconnectFromServer();
		}
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

	private void OnGui_StartGame()
	{
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        var roomData = MonobitEngine.MonobitNetwork.room;
		string playerInfo = "(" + roomData.playerCount + "/" + ((roomData.maxPlayers == 0) ? "-" : roomData.maxPlayers.ToString()) + ")";
		GUILayout.Label("Num Players : " + roomData.name + playerInfo, Label, GUILayout.Width(BaseGUIWidth * 3));

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        if (RoomManager.IsHost == false) // ゲームを開始出来るのはホストのみに制限
		{
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUILayout.Label("Waiting for host to start...", Label, GUILayout.Width(BaseGUIWidth * 3));
            roommaster = false;

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return;
		}

        // ルーム内に自分しか居なければ他のプレイヤーを待つ
        /*if(MonobitEngine.MonobitNetwork.room.playerCount <= 1)
		{
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUILayout.Label("Waiting for other player...", Label, GUILayout.Width(BaseGUIWidth * 3));

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            return;
		}*/

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Game Start", button, GUILayout.Width(BaseGUIWidth * 3)))
        {
            monobitView.RPC("LoadInGameScene", MonobitEngine.MonobitTargets.OthersBuffered);
			roomData.visible = false; // ゲーム開始後はルームが他プレイヤーから見えないように
			LoadInGameScene();
		}
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }

    //ステージ選択用GUI
    private void OnGui_ChooseStage()
    {

        if (RoomManager.IsHost == true)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(50);
            GUILayout.Label("SelectStage", TagLabel);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Loby", button, GUILayout.Width(BaseGUIWidth * 2)))
                stageselect = 0;
            if (GUILayout.Button("Plane", button, GUILayout.Width(BaseGUIWidth * 2)))
                stageselect = 1;
            if (GUILayout.Button("Forest", button, GUILayout.Width(BaseGUIWidth * 2)))
                stageselect = 2;
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Shrine", button, GUILayout.Width(BaseGUIWidth * 2)))
                stageselect = 3;
            if (GUILayout.Button("Sky", button, GUILayout.Width(BaseGUIWidth * 2)))
                stageselect = 4;
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }


    // ルームから退室用GUI
    private void OnGui_LeaveRoom()
	{
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Leave from Room", button, GUILayout.Width(BaseGUIWidth * 4)))
		{
			m_OnPushLeftOrDisconnectButton = false;
            OnInGameScene();
            NetworkManager.Instance.LeaveRoom();
		}

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

	// ルーム作成用GUI
	private void OnGui_CreateRoom()
    {
        roommaster = true;
        gs = false;
        GUILayout.BeginHorizontal();
        GUILayout.Space(50);
        GUILayout.Label("Create Room", TagLabel);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
		GUILayout.Label("Room Name", Label, GUILayout.Width(BaseGUIWidth * 3));
		m_RoomName = GUILayout.TextField(m_RoomName, TextField, GUILayout.Width(BaseGUIWidth * 4));

		if (GUILayout.Button("Create", button))
		{
			if (string.IsNullOrEmpty(m_RoomName) == false)
				NetworkManager.Instance.CreateRoom(m_RoomName);
		}
        GUILayout.Space(50);
        GUILayout.EndHorizontal();
	}

	// 他プレイヤー検索用GUI
	private void OnGui_SearchPlayer()
	{
        GUILayout.BeginHorizontal();
        GUILayout.Space(50);
        GUILayout.Label("Search Player", TagLabel, GUILayout.Width(BaseGUIWidth * 3));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
		GUILayout.Label("Player Name : ", Label, GUILayout.Width(BaseGUIWidth * 3));
		m_SearchPlayerName = GUILayout.TextField(m_SearchPlayerName, TextField, GUILayout.Width(BaseGUIWidth * 4));
		string serachButtonName = "Search";
		if (GUILayout.Button(serachButtonName, button))
		{
			if (string.IsNullOrEmpty(m_SearchPlayerName) == false)
				NetworkManager.Instance.SearchPlayers(m_SearchPlayerName.Split(','));
		}
        GUILayout.Space(50);
		GUILayout.EndHorizontal();
	}

	// 他プレイヤー検索結果表示用GUI
	private void OnGui_SearchPlayerResult()
	{
		if (NetworkManager.Instance.IsFindSearchPlayer == false) return;

		GUILayout.Label("Search Result", Label);
		foreach(var player in MonobitEngine.MonobitNetwork.SearchPlayerList)
		{
			if(player.connect == false)
			{
				GUILayout.Label(player.playerName + " is Offline", Label, GUILayout.Width(BaseGUIWidth * 3));
				continue;
			}

			if(player.inRoom == false)
			{
				GUILayout.Label(player.playerName + " is not in Room", Label, GUILayout.Width(BaseGUIWidth * 3));
				continue;
			}

			GUILayout.Label(player.playerName + " is in" + player.roomName, Label, GUILayout.Width(BaseGUIWidth * 3));

			if (GUILayout.Button("Join", button))
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

        GUILayout.BeginHorizontal();
        GUILayout.Space(50);
        GUILayout.Label("Choose Room", TagLabel, GUILayout.Width(BaseGUIWidth * 3));
        GUILayout.EndHorizontal();
        foreach (var roomData in roomDataList)
		{
			if (roomData.playerCount >= roomData.maxPlayers)
				continue;

			GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            // ルーム情報表示
            string roomName = roomData.name;
			string playerInfo = "(" + roomData.playerCount + "/" + ((roomData.maxPlayers == 0) ? "-" : roomData.maxPlayers.ToString()) + ")";
			GUILayout.Label("Room Name: " + roomName + playerInfo, Label, GUILayout.Width(BaseGUIWidth * 3));

			if (GUILayout.Button("Join", button, GUILayout.Width(BaseGUIWidth * 3)))
				NetworkManager.Instance.JoinRoom(roomData.name); // 入室
            GUILayout.FlexibleSpace();
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
