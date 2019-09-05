using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Resourcesのキャシュ有りver
public class CachedResources : MonoBehaviour<CachedResources>
{
	private class LoadAsyncRequest
	{
		public string path;
		public OnLoadedFunc onLoaded;
	}

	private static Dictionary<string,Object> s_ChachedObjects = new Dictionary<string, Object>();
	private static Queue<LoadAsyncRequest> s_LoadAsyncRequests = new Queue<LoadAsyncRequest>();

	public delegate void OnLoadedFunc(Object obj);

	// 読み込み
	public static Object Load(string path)
	{
		if (string.IsNullOrEmpty(path)) return null;

		if (s_ChachedObjects.ContainsKey(path))
			return s_ChachedObjects[path];

		var obj = Resources.Load(path);
		s_ChachedObjects.Add(path, obj);
		return obj;
	}

	// 読み込み
	public static T Load<T>(string path) where T : Object
	{
		if (string.IsNullOrEmpty(path)) return null;

		if (s_ChachedObjects.ContainsKey(path))
			return s_ChachedObjects[path] as T;

		var obj = Resources.Load<T>(path);
		s_ChachedObjects.Add(path,obj);
		return obj;
	}

	// 非同期読み込み
	public static void LoadAsync(string path, OnLoadedFunc func = null)
	{
		if (string.IsNullOrEmpty(path)) return;

		if (s_ChachedObjects.ContainsKey(path))
		{
			if (func != null) func(s_ChachedObjects[path]);
			return;
		}
		s_LoadAsyncRequests.Enqueue(new LoadAsyncRequest { path = path, onLoaded = func });
	}

	// キャシュクリア
	public static void ClearCache()
	{
		s_ChachedObjects.Clear();
	}

	// 非同期読み込み(実装部分)
	private IEnumerator LoadAsyncImpl(string path,OnLoadedFunc func)
	{
		var request = Resources.LoadAsync(path);

		while (request.isDone == false)
			yield return null;

		s_ChachedObjects.Add(path, request.asset);
		func?.Invoke(request.asset);
	}

	void LateUpdate()
	{
		// 非同期での読み込みリクエストがあれば処理
		if (s_LoadAsyncRequests.Count <= 0)
			return;
		foreach(var request in s_LoadAsyncRequests)
			StartCoroutine(LoadAsyncImpl(request.path, request.onLoaded));
		s_LoadAsyncRequests.Clear();
	}

}
