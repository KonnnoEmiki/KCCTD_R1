using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : class, new()
{
	private static T s_Instance = new T();
	public static T Instance
	{
		get { return s_Instance;}
	}

	protected Singleton()
	{
	}
}

// シングルトン
public class MonoBehaviour<T>
	: MonoBehaviour
	  where T : MonoBehaviour
{
	public bool m_DontDestoryOnLoad = true;
	
	private static T s_Instance = null;
	public static T Instance
	{
		get
		{
			if(s_Instance == null)
			{
				GameObject o = new GameObject(typeof(T).Name);
				s_Instance = o.AddComponent<T>();
				if((s_Instance as MonoBehaviour<T>).m_DontDestoryOnLoad)
					DontDestroyOnLoad(o);
			}
			return s_Instance;
		}
	}

	public static bool HasInstance { get { return s_Instance != null; } }
	
	protected virtual void Awake()
	{
		if(s_Instance == null)
		{
			s_Instance = this as T;
			if ((s_Instance as MonoBehaviour<T>).m_DontDestoryOnLoad)
				DontDestroyOnLoad(this);
			return;
		}
		if (this == s_Instance) return;
		// this != Instance
		Destroy(this);
	}

	protected virtual void OnDestroy()
	{
		if(this == s_Instance)
			s_Instance = null;
	}
}

namespace MonobitEngine
{
	// シングルトン(MonobitEngine.MonoBehaviour版)
	public class SingletonMonoBehaviour<T>
		: MonobitEngine.MonoBehaviour
		  where T : MonobitEngine.MonoBehaviour
	{
		public bool m_DontDestoryOnLoad = true;

		private static T s_Instance = null;
		public static T Instance
		{
			get
			{
				if (s_Instance == null)
				{
					GameObject o = new GameObject(typeof(T).Name);
					s_Instance = o.AddComponent<T>();
					if ((s_Instance as SingletonMonoBehaviour<T>).m_DontDestoryOnLoad)
						DontDestroyOnLoad(o);
				}
				return s_Instance;
			}
		}

		public static bool HasInstance { get { return s_Instance != null; } }

		protected virtual void Awake()
		{
			if (s_Instance == null)
			{
				s_Instance = this as T;
				if ((s_Instance as SingletonMonoBehaviour<T>).m_DontDestoryOnLoad)
					DontDestroyOnLoad(this);
				return;
			}
			if (this == s_Instance) return;
			// this != Instance
			Destroy(this);
		}

		protected virtual void OnDestroy()
		{
			if (this == s_Instance)
				s_Instance = null;
		}
	}
}
