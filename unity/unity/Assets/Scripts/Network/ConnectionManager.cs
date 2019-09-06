using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 接続管理クラス
public class ConnectionManager : MonobitEngine.ObservableMonoBehaviour<NetworkEvent>
{
	private NetworkDefines.ConnectionState m_State = NetworkDefines.ConnectionState.Disconnect;
	public NetworkDefines.ConnectionState ConnectionState
	{
		get { return m_State; }
	}

	private bool m_IsDisconnectRequest = false;
	public bool IsDisconnectRequest
	{
		get { return m_IsDisconnectRequest; }
	}

	private bool m_IsFindSearchPlayers = false;
	public bool IsFIndSearchPlayers
	{
		get { return m_IsFindSearchPlayers; }
	}

	private string[] m_LastSerchPlayerNames = null;


	public ConnectionManager()
	{
		ChengeObservable(new NetworkEventObservable());
	}

	// サーバーへ接続
	public void ConnectToServer(string playerName,string gameVersion = "ver 1.0",Hashtable customAuthData = null)
	{
		if (MonobitEngine.MonobitNetwork.isConnect)
			return;
		MonobitEngine.MonobitNetwork.autoJoinLobby = true;
		MonobitEngine.MonobitNetwork.playerName = playerName;
		MonobitEngine.MonobitNetwork.ConnectServer(gameVersion,customAuthData);
		m_State = NetworkDefines.ConnectionState.Disconnect;
	}

	// サーバーから切断
	public void DisconnectFromServer()
	{
		if(MonobitEngine.MonobitNetwork.isConnect == false)
			return;
		m_IsDisconnectRequest = true;
		if (MonobitEngine.MonobitNetwork.inRoom)
			MonobitEngine.MonobitNetwork.LeaveRoom();
		else if (MonobitEngine.MonobitNetwork.inLobby)
			MonobitEngine.MonobitNetwork.LeaveLobby();
	}

	// プレイヤー検索
	public void SearchPlayers(string[] searchPlayerNames)
	{
		m_IsFindSearchPlayers = false;
		MonobitEngine.MonobitNetwork.SearchPlayers(searchPlayerNames);
		m_LastSerchPlayerNames = searchPlayerNames;
	}

	// 検索対象のプレイヤーが見つかったか確認
	private void CheckFindSearchPlayers()
	{
		if (m_LastSerchPlayerNames == null || m_LastSerchPlayerNames.Length < 1)
			return;
		foreach (var searchPlayerName in m_LastSerchPlayerNames)
		{
			var result = MonobitEngine.MonobitNetwork.SearchPlayerList.Find(player => player.playerName == searchPlayerName);

			if (result != null)
			{
				m_IsFindSearchPlayers = true;
				break;
			}
		}
		m_LastSerchPlayerNames = null;
	}

	#region 接続コールバック関数

	// 接続失敗
	public void OnConnectToServerFailed(MonobitEngine.DisconnectCause cause)
	{
		NetworkEvent e = new ConnectToServerFailedEvent { m_Cause = cause };
		NotifyObservers(e);
	}

	// ルーム満員
	public void OnMonobitMaxConnectionReached()
	{
		NetworkEvent e = new MonobitMaxConnectionReachedEvent { };
		NotifyObservers(e);
	}

	// サーバーへ接続された
	public void OnConnectedToServer()
	{
		m_State = NetworkDefines.ConnectionState.Connect;

		NetworkEvent e = new ConnectedToServerEvent { };
		NotifyObservers(e);
	}

	// ロビーへ入った
	public void OnJoinedLobby()
	{
		NetworkEvent e = new JoinedLobbyEvent { };
		NotifyObservers(e);
	}

	// フレンドリストが更新された
	public void OnUpdatedSearchPlayers()
	{
		CheckFindSearchPlayers();
		NetworkEvent e = new UpdatedSearchPlayersEvent { };
		NotifyObservers(e);
	}

	// ルーム一覧が更新された
	public void OnReceivedRoomListUpdate()
	{
		NetworkEvent e = new ReceivedRoomListUpdateEvent { };
		NotifyObservers(e);
	}

	// ルームが作成された
	public void OnCreatedRoom()
	{
		NetworkEvent e = new CreatedRoomEvent { };
		NotifyObservers(e);
	}

	// ルームの作成に失敗した時
	public void OnCreateRoomFailed(object[] codeAndMsg)
	{
		NetworkEvent e = new CreateRoomFailedEvent { m_CodeAndMsg = codeAndMsg };
		NotifyObservers(e);
	}

	// ルームへ入った
	public void OnJoinedRoom()
	{
	
		NetworkEvent e = new JoinedRoomEvent { };
		NotifyObservers(e);
	}

	// ルームの入室に失敗した
	public void OnJoinRoomFailed(object[] codeAndMsg)
	{
		NetworkEvent e = new JoinRoomFailedEvent { m_CodeAndMsg = codeAndMsg };
		NotifyObservers(e);
	}

	// ルームのランダム入室に失敗した
	public void OnMonobitRandomJoinFailed(object[] codeAndMsg)
	{
		NetworkEvent e = new MonobitRandomJoinFailedEvent { m_CodeAndMsg = codeAndMsg };
		NotifyObservers(e);
	}

	// ルームに誰かが入室して
	public void OnOtherPlayerConnected(MonobitEngine.MonobitPlayer newPlayer)
	{
		NetworkEvent e = new OtherPlayerConnectedEvent { m_NewPlayer = newPlayer };
		NotifyObservers(e);
	}

	// ホストが変更された
	public void OnHostChanged(MonobitEngine.MonobitPlayer newHost)
	{
		NetworkEvent e = new HostChengedEvent { m_NewHost = newHost };
		NotifyObservers(e);

	}

	// ルームのプロパティが変更されたとき
	public void OnMonobitCustomRoomParametersChanged(Hashtable peopertiesThatChanged)
	{
		NetworkEvent e = new MonobitCustomRoomParamerersChengedEvent
		{
			m_PeopertiesThatChanged = peopertiesThatChanged
		};
		NotifyObservers(e);

	}

	// プレイヤーのプロパティが変更されたとき
	public void OnMonobitPlayerParametersChanged(object[] playerAndUpdateProps)
	{
		NetworkEvent e = new MonobitPlayerParametersChengedEvent
		{
			m_PlayerAndUpdateProps = playerAndUpdateProps
		};
		NotifyObservers(e);
	}

	// サーバへの接続に失敗したとき
	public void OnConnectionFail(MonobitEngine.DisconnectCause cause)
	{
		NetworkEvent e = new ConnectionFailEvent { m_Cause = cause };
		NotifyObservers(e);
	}

	// カスタム認証に失敗したとき
	public void OnCustomAuthenticationFailed(string rawData)
	{
		NetworkEvent e = new CustomAuthenticationFailedEvent { m_RawData = rawData };
		NotifyObservers(e);
	}

	// ルーム内の誰かが退室したとき
	public void OnOtherPlayerDisconnected(MonobitEngine.MonobitPlayer otherPlayer)
	{
		NetworkEvent e = new OtherPlayerDisconnectedEvent { m_OtherPlayer = otherPlayer };
		NotifyObservers(e);
	}

	// ルームから退室した
	public void OnLeftRoom()
	{
		if (m_IsDisconnectRequest)
			MonobitEngine.MonobitNetwork.LeaveLobby();
		NetworkEvent e = new LeftRoomEvent { };
		NotifyObservers(e);
	}

	// ロビーから退室した
	public void OnLeftLobby()
	{
		if (m_IsDisconnectRequest)
			MonobitEngine.MonobitNetwork.DisconnectServer();
		NetworkEvent e = new LeftLobbyEvent { };
		NotifyObservers(e);
	}

	// サーバーから切断された
	public void OnDisconnectedFromServer()
	{
		m_State = NetworkDefines.ConnectionState.Disconnect;
		m_IsDisconnectRequest = false;
		NetworkEvent e = new DisconnectedFromServerEvent { };
		NotifyObservers(e);
	}

	#endregion

}
