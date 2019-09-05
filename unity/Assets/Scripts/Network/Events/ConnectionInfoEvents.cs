using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// サーバーへの接続関連のイベントクラス

// サーバーへの接続が確立しなかった時のイベント
public class ConnectToServerFailedEvent : NetworkEvent
{
	public MonobitEngine.DisconnectCause m_Cause; // エラーコード

	public override int GetEventType()
	{
		return (int)NetworkEventType.ConnectToServerFailed;
	}

	public override string GetName()
	{
		return this.GetType().Name;
	}
}

// 同時接続可能なユーザー数に達しサーバーへ接続できなかった時のイベント
public class MonobitMaxConnectionReachedEvent : NetworkEvent
{
	public override int GetEventType()
	{
		return (int)NetworkEventType.MonobitMaxConnectionReached;
	}

	public override string GetName()
	{
		return this.GetType().Name;
	}
}

// サーバーへの接続が確立し認証処理も終了した(通信可能になった)時のイベント
public class ConnectedToServerEvent : NetworkEvent
{
	public override int GetEventType()
	{
		return (int)NetworkEventType.ConnectedToServer;
	}

	public override string GetName()
	{
		return this.GetType().Name;
	}
}

// 自身がロビーに入室した時のイベント
public class JoinedLobbyEvent : NetworkEvent
{
	public override int GetEventType()
	{
		return (int)NetworkEventType.JoinedLobby;
	}

	public override string GetName()
	{
		return this.GetType().Name;
	}
}

// 自身がルーム作成に成功した時のイベント
public class CreatedRoomEvent : NetworkEvent
{
	public override int GetEventType()
	{
		return (int)NetworkEventType.CreatedRoom;
	}

	public override string GetName()
	{
		return this.GetType().Name;
	}
}

// 自身がルーム作成に失敗した時のイベント
public class CreateRoomFailedEvent : NetworkEvent
{
	public object[] m_CodeAndMsg;

	public override int GetEventType()
	{
		return (int)NetworkEventType.CreateRoomFailed;
	}

	public override string GetName()
	{
		return this.GetType().Name;
	}
}

// 自身がルームに入室した時のイベント
public class JoinedRoomEvent : NetworkEvent
{
	public override int GetEventType()
	{
		return (int)NetworkEventType.JoinedRoom;
	}

	public override string GetName()
	{
		return this.GetType().Name;
	}
}

// 自身がルームへの入室に失敗した時のイベント
public class JoinRoomFailedEvent : NetworkEvent
{
	public object[] m_CodeAndMsg = null;

	public override int GetEventType()
	{
		return (int)NetworkEventType.JoinRoomFailed;
	}

	public override string GetName()
	{
		return this.GetType().Name;
	}
}

// 自身がランダム入室に失敗した時のイベント
public class MonobitRandomJoinFailedEvent : NetworkEvent
{
	public object[] m_CodeAndMsg = null;

	public override int GetEventType()
	{
		return (int)NetworkEventType.MonobitRandomJoinFailed;
	}

	public override string GetName()
	{
		return this.GetType().Name;
	}
}

// サーバー接続確立後に何らかの原因で切断された時のイベント
public class ConnectionFailEvent : NetworkEvent
{
	public MonobitEngine.DisconnectCause m_Cause; // エラーコード

	public override int GetEventType()
	{
		return (int)NetworkEventType.ConnectionFail;
	}

	public override string GetName()
	{
		return this.GetType().Name;
	}
}

// カスタム認証処理に失敗した(カスタム認証を有効にしている場合)時のイベント
public class CustomAuthenticationFailedEvent : NetworkEvent
{
	public string m_RawData = null;

	public override int GetEventType()
	{
		return (int)NetworkEventType.CustomAuthenticationFailed;
	}

	public override string GetName()
	{
		return this.GetType().Name;
	}
}

// 自身がルームから退室した時のイベント
public class LeftRoomEvent : NetworkEvent
{
	public override int GetEventType()
	{
		return (int)NetworkEventType.LeftRoom;
	}

	public override string GetName()
	{
		return this.GetType().Name;
	}
}

// 自身がロビーから退室した時のイベント
public class LeftLobbyEvent : NetworkEvent
{
	public override int GetEventType()
	{
		return (int)NetworkEventType.LeftLobby;
	}

	public override string GetName()
	{
		return this.GetType().Name;
	}
}

// サーバーから回線を切断した時のイベント
public class DisconnectedFromServerEvent : NetworkEvent
{
	public override int GetEventType()
	{
		return (int)NetworkEventType.DisconnectedFromServer;
	}

	public override string GetName()
	{
		return this.GetType().Name;
	}
}
