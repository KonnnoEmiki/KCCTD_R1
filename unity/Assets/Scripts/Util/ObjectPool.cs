using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// オブジェクトプールクラス
public class ObjectPool : MonoBehaviour<ObjectPool>
{
	protected Dictionary<int, List<GameObject>> m_PoolUnusedObjects = new Dictionary<int, List<GameObject>>();
	protected Dictionary<int, List<GameObject>> m_PoolUsedObjects = new Dictionary<int, List<GameObject>>();
	protected Dictionary<GameObject, int> m_ObjectToPrefabInstanceID = new Dictionary<GameObject, int>();
	
	// プールから未使用のオブジェクト取得(未使用の物が無ければ生成)
	public GameObject Instantiate(GameObject prefab,Vector3 position,Quaternion rotation)
	{
		int key = prefab.GetInstanceID();

		if(m_PoolUnusedObjects.ContainsKey(key) == false)
			m_PoolUnusedObjects.Add(key, new List<GameObject>());

		if (m_PoolUsedObjects.ContainsKey(key) == false)
			m_PoolUsedObjects.Add(key, new List<GameObject>());

		List<GameObject> unusedObjects = m_PoolUnusedObjects[key];
		List<GameObject> usedObjects = m_PoolUsedObjects[key];

		GameObject obj = null;
		for (int i = 0;i<unusedObjects.Count;++i)
		{
			obj = unusedObjects[i];
			if (obj.activeInHierarchy)
				continue;
			obj.transform.SetParent(null);
			obj.transform.position = position;
			obj.transform.rotation = rotation;
			obj.SetActive(true);
			usedObjects.Add(obj);
			unusedObjects.RemoveAt(i);
			return obj;
		}

		obj = CreateInstance(prefab, position, rotation);
		usedObjects.Add(obj);
		m_ObjectToPrefabInstanceID.Add(obj, key);
		return obj;
	}
	
	protected virtual GameObject CreateInstance(GameObject prefab, Vector3 position, Quaternion rotation)
	{
		return Object.Instantiate(prefab, position, rotation);
	}

	// プールへ不要なオブジェクトを返却
	public virtual void DestroyObject(GameObject obj)
	{
		if (m_ObjectToPrefabInstanceID.ContainsKey(obj) == false) return;

		int id = m_ObjectToPrefabInstanceID[obj];
		m_PoolUsedObjects[id].Remove(obj);
		m_PoolUnusedObjects[id].Add(obj);
		obj.SetActive(false);
		obj.transform.SetParent(transform);
	}

	// 未使用のオブジェクト全削除
	public virtual void DestroyUnusedObjects()
	{
		foreach (var Objects in m_PoolUnusedObjects)
		{
			foreach(var obj in Objects.Value)
				Destroy(obj);
			
			Objects.Value.Clear();
		}
		m_ObjectToPrefabInstanceID.Clear();
	}

}


