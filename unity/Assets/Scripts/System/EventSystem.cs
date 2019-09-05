using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 全イベントクラスの基底クラス
public abstract class EventBase
{
	public abstract int GetEventType();
	public abstract string GetName();
}

// イベントの処理の実行管理役
public class EventDispatcher
{
	protected EventBase m_Event;

	public EventDispatcher(EventBase e)
	{
		m_Event = e;
	}

	public delegate void EventFunction();
	public delegate void EventFunction<EventType>(EventType e);

	// EventTypeとコンストラクタで渡されたイベントの型が一致していればfuncを実行(EventType変数の引数ありver)
	public virtual bool Dispatch<EventType>(EventFunction<EventType> func) where EventType : EventBase
	{
		if(m_Event.GetType() == typeof(EventType))
		{
			func((EventType)m_Event);
			return true;
		}
		return false;
	}
	// EventTypeとコンストラクタで渡されたイベントの型が一致していればfuncを実行(EventType変数の引数無しver)
	public virtual bool Dispatch<EventType>(EventFunction func) where EventType : EventBase
	{
		if (m_Event.GetType() == typeof(EventType))
		{
			func();
			return true;
		}
		return false;
	}
}

// ↑の戻り地ありver(Tは主にboolを想定)
public abstract class EventDispatcher<T>
{
	public delegate T EventFnction<EventType>(EventType e);
	public abstract bool Dispatch<EventType>(EventFnction<EventType> func) where EventType : EventBase;
}