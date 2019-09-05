using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤー関係のイベントクラス

public enum PlayerEventType
{
	Spawn,
	AllPlayerSpawnCompleted,
	Down
}

// プレイヤー関係のイベントクラスの基底クラス
public abstract class PlayerEvent : EventBase
{
}

// スポーン時のイベント
public class OnSpawnPlayerEvent : PlayerEvent
{
	public Player m_SpawnPlayer = null; // スポーンしたプレイヤー

	public OnSpawnPlayerEvent(Player spawnPlayer)
	{
		m_SpawnPlayer = spawnPlayer;
	}

	public override int GetEventType()
	{
		return (int)PlayerEventType.Spawn;
	}

	public override string GetName()
	{
		return "OnSpawnPlayerEvent";
	}
}

// ルーム内の全プレイヤーがスポーンした時のイベント
public class OnAllPlayerSpawnCompletedEvent : PlayerEvent
{
	public override int GetEventType()
	{
		return (int)PlayerEventType.AllPlayerSpawnCompleted;
	}

	public override string GetName()
	{
		return "OnAllPlayerSpawnCompletedEvent";
	}
}

// プレイヤーが倒れた時のイベント
public class OnDownPlayerEvent : PlayerEvent
{
	public Player m_DownPlayer = null; // 倒れたプレイヤー

	public OnDownPlayerEvent(Player downPlayer)
	{
		m_DownPlayer = downPlayer;
	}

	public override int GetEventType()
	{
		return (int)PlayerEventType.Down;
	}

	public override string GetName()
	{
		return "OnDownPlayerEvent";
	}
}
