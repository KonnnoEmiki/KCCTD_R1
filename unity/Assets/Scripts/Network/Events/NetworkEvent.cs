using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NetworkEventType
{
	None = 0,

	ConnectToServerFailed,				// サーバーへの接続が確立しなかった
	MonobitMaxConnectionReached,		// 同時接続可能なユーザー数に達しサーバーへ接続できなかった
	ConnectedToServer,					// サーバーへの接続が確立し認証処理も終了した(通信可能になった)
	JoinedLobby,						// 自身がロビーに入室した
	CreatedRoom,						// 自身がルーム作成に成功した
	CreateRoomFailed,					// 自身がルーム作成に失敗した
	JoinedRoom,							// 自身がルームに入室した
	JoinRoomFailed,						// 自身がルームへの入室に失敗した
	MonobitRandomJoinFailed,			// 自身がランダム入室に失敗した
	ConnectionFail,						// サーバー接続確立後に何らかの原因で切断された
	CustomAuthenticationFailed,         // カスタム認証処理に失敗した(カスタム認証を有効にしている場合)
	LeftRoom,							// 自身がルームから退室した
	LeftLobby,							// 自身がロビーから退室した
	DisconnectedFromServer,				// サーバーから回線を切断した

	OtherPlayerConnected,				// ルーム内に自身以外の誰かが入室した
	OtherPlayerDisconnected,            // ルーム内の自身以外の誰かがルームを退室した

	UpdatedSearchPlayers,				// SearchPlayers() のリクエストでMonobitNetwork.SearchPlayerList が更新された
	ReceivedRoomListUpdate,				// ルームの一覧がサーバによって更新された
	HostChenged,						// ルームのホストが切り替わった
	MonobitCustomRoomParamerersChenged,	// ルームパラメータが変更された
	MonobitPlayerParametersChenged,		// ルーム内のプレイヤーのパラメータが変更された
	
}

// ネットワーク関連のイベントクラスの基底クラス
public abstract class NetworkEvent : EventBase
{
	public bool m_Handled = false;
}

// ネットワークイベントの処理の実行管理役
public class NetworkEventDispatcher : EventDispatcher<bool>
{
	NetworkEvent m_Event = null;

	public NetworkEventDispatcher(NetworkEvent e)
	{
		m_Event = e;
	}

	// EventTypeとコンストラクタで渡されたイベントの型が一致していればfuncを実行
	public override bool Dispatch<EventType>(EventFnction<EventType> func)
	{
		if(m_Event.GetType() == typeof(EventType))
		{
			m_Event.m_Handled = func(m_Event as EventType);
			return true;
		}
		return false;
	}
}
