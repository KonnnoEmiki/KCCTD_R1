using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// サーバーで管理している情報に変更があった時のイベント

// SearchPlayers() のリクエストでMonobitNetwork.SearchPlayerList が更新された時のイベント
public class UpdatedSearchPlayersEvent : NetworkEvent
{
	public override int GetEventType()
	{
		return (int)NetworkEventType.UpdatedSearchPlayers;
	}

	public override string GetName()
	{
		return this.GetType().Name;
	}
}

// ルームの一覧がサーバによって更新された時のイベント
public class ReceivedRoomListUpdateEvent : NetworkEvent
{
	public override int GetEventType()
	{
		return (int)NetworkEventType.ReceivedRoomListUpdate;
	}

	public override string GetName()
	{
		return this.GetType().Name;
	}
}

// ルームのホストが切り替わった時のイベント
public class HostChengedEvent : NetworkEvent
{
	public MonobitEngine.MonobitPlayer m_NewHost; // 新しいホスト

	public override int GetEventType()
	{
		return (int)NetworkEventType.HostChenged;
	}

	public override string GetName()
	{
		return this.GetType().Name;
	}
}

// ルームパラメータが変更された時のイベント
public class MonobitCustomRoomParamerersChengedEvent : NetworkEvent
{
	public Hashtable m_PeopertiesThatChanged;

	public override int GetEventType()
	{
		return (int)NetworkEventType.MonobitCustomRoomParamerersChenged;
	}

	public override string GetName()
	{
		return this.GetType().Name;
	}
}

// ルーム内のプレイヤーのパラメータが変更された時のイベント
public class MonobitPlayerParametersChengedEvent : NetworkEvent
{
	public object[] m_PlayerAndUpdateProps;

	public override int GetEventType()
	{
		return (int)NetworkEventType.MonobitPlayerParametersChenged;
	}

	public override string GetName()
	{
		return this.GetType().Name;
	}
}
