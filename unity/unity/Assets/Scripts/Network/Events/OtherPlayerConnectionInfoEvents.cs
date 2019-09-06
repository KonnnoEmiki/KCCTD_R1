using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 他のプレイヤー関連のネットワークイベントクラス

// ルーム内に自身以外の誰かが入室した時のイベント
public class OtherPlayerConnectedEvent : NetworkEvent
{
	public MonobitEngine.MonobitPlayer m_NewPlayer = null; // 入室したプレイヤー

	public override int GetEventType()
	{
		return (int)NetworkEventType.OtherPlayerConnected;
	}

	public override string GetName()
	{
		return this.GetType().Name;
	}
}

// ルーム内の自身以外の誰かがルームを退室した時のイベント
public class OtherPlayerDisconnectedEvent : NetworkEvent
{
	public MonobitEngine.MonobitPlayer m_OtherPlayer = null; // 切断したプレイヤー
	
	public override int GetEventType()
	{
		return (int)NetworkEventType.OtherPlayerDisconnected;
	}

	public override string GetName()
	{
		return this.GetType().Name;
	}
}