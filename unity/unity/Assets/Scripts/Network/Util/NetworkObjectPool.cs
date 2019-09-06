using UnityEngine;
using System.Collections.Generic;

// MonobitEngine用のオブジェクトプールクラス
public class NetworkObjectPool : MonoBehaviour, MonobitEngine.IMunPrefabPool
{
	[System.Serializable]
	public class PoolObjectData
	{
		[AssetPath]
		public string m_PrefabPath;
		public int m_MaxCount;
	}

	[SerializeField]
	private List<PoolObjectData> m_OriginPrefabDataList = new List<PoolObjectData>();

	// Instatiate 生成元となるゲームオブジェクト（プレハブ）
	private Dictionary<string,GameObject> m_Origin = new Dictionary<string,GameObject>();

	private Dictionary<string, List<GameObject>> m_PoolLists = new Dictionary<string, List<GameObject>>(); // オブジェクトプールとして登録されたインスタンスの実体
	private Dictionary<string, GameObject> m_PoolObjectParents = new Dictionary<string, GameObject>();
	private Dictionary<int, GameObject> m_InstanceIDToObject = new Dictionary<int, GameObject>();
	private Dictionary<int, string> m_InstanceIDToPrefabPath = new Dictionary<int, string>();

	void Awake()
	{
		foreach(var originData in m_OriginPrefabDataList)
		{
			var prefab = CachedResources.Load<GameObject>(originData.m_PrefabPath);
			m_Origin.Add( originData.m_PrefabPath, prefab);
			var poolList = new List<GameObject>();
			m_PoolLists.Add(originData.m_PrefabPath,poolList);
			var parent = new GameObject("ObjectPool_" + originData.m_PrefabPath);
			DontDestroyOnLoad(parent);
			m_PoolObjectParents.Add(originData.m_PrefabPath, parent);

			// maxCount 分だけ、非アクティブかつシーン遷移で破棄されないインスタンスを生成する
			for (int i = 0; i < originData.m_MaxCount; i++)
			{
				var obj = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
				if (obj != null)
				{
					GameObject.DontDestroyOnLoad(obj); // シーン遷移で破棄させない
					obj.SetActive(false); // 非アクティブ設定
					obj.transform.SetParent(parent.transform);
					poolList.Add(obj);
					var instanceID = obj.GetInstanceID();
					m_InstanceIDToObject.Add(instanceID, obj);
					m_InstanceIDToPrefabPath.Add(instanceID, originData.m_PrefabPath);
				}
			}
		}	
	}

	// オブジェクト動的生成のカスタマイズ
	public GameObject Instantiate(string prefabPath, Vector3 position, Quaternion rotation)
	{
		// インスタンスを非アクティブからアクティブに変更
		GameObject go = null;

		bool find = m_Origin.ContainsKey(prefabPath);

		if (find)
		{
			foreach (GameObject obj in m_PoolLists[prefabPath])
			{
				if (obj.activeSelf) continue;
				go = obj;
				go.SetActive(true);
				go.transform.SetParent(null);
				break;
			}
			if(go == null)
			{
				var prefab = m_Origin[prefabPath];
				go = GameObject.Instantiate(prefab,position,rotation);

				GameObject.DontDestroyOnLoad(go); // シーン遷移で破棄させない
				var poolList = m_PoolLists[prefabPath];
				poolList.Add(go);
				var instanceID = go.GetInstanceID();
				m_InstanceIDToObject.Add(instanceID, go);
				m_InstanceIDToPrefabPath.Add(instanceID, prefabPath);
				return go;
			}
		}
		else
		{
			var prefab = CachedResources.Load<GameObject>(prefabPath);
			go = GameObject.Instantiate(prefab, position, rotation);
			return go;
		}
		

		// アクティブに変更したインスタンスの transform 情報を設定
		if (go != null)
		{
			go.transform.position = position;
			go.transform.rotation = rotation;
		}
		return go;
	}

	// オブジェクト動的破棄のカスタマイズ
	public void Destroy(GameObject obj)
	{
		MonobitEngine.MonobitView view = obj.GetComponent<MonobitEngine.MonobitView>();
		var instanceID = obj.GetInstanceID();
		if (view != null  && m_InstanceIDToObject.ContainsKey(instanceID))
		{
			// 自身のオブジェクトであれば非アクティブ化
			obj.SetActive(false);
			var path = m_InstanceIDToPrefabPath[obj.GetInstanceID()];
			if (m_PoolObjectParents.ContainsKey(path))
			{
				var parent = m_PoolObjectParents[path];
				if(parent != null)
					obj.transform.SetParent(parent.transform);
			}
		}
		else
		{
			// 他クライアントからのオブジェクトであれば破棄
			GameObject.Destroy(obj);

		}
	}

	// 明示的なデストラクタ
	public void DestroyAll()
	{
		foreach(var poolList in m_PoolLists)
		{
			foreach (GameObject obj in poolList.Value)
			{
				MonobitEngine.MonobitNetwork.Destroy(obj);
				GameObject.Destroy(obj);
			}
		}
		foreach (var parent in m_PoolObjectParents)
			GameObject.Destroy(parent.Value);
		m_PoolLists.Clear();
		m_PoolObjectParents.Clear();
		m_InstanceIDToObject.Clear();
		m_InstanceIDToPrefabPath.Clear();
	}
}
