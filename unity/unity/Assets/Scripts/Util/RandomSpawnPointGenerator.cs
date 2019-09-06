using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ランダムなスポーン地点生成用クラス
public class RandomSpawnPointGenerator : MonoBehaviour
{
	[SerializeField, Tag]
	private string m_SpawnAreaTag = string.Empty;

	[SerializeField]
	private Vector3 m_MinArea;
	[SerializeField]
	private Vector3 m_MaxArea;

	private void Awake()
	{
		for(int i = 0; i < 3; i++)
		{
			if (m_MinArea[i] > m_MaxArea[i])
				m_MinArea[i] = m_MaxArea[i];
			else if (m_MaxArea[i] < m_MinArea[i])
				m_MaxArea[i] = m_MinArea[i];
		}
	}

	// スポーン地点生成
	public Vector3 GenerateSpawnPoint()
	{
		Vector3 result = Vector3.zero;
		for(int i = 0; i < 3; i++)
			result[i] = Random.Range(m_MinArea[i], m_MaxArea[i]);
		if (string.IsNullOrEmpty(m_SpawnAreaTag))
		{
			RaycastHit hit;
			if(Physics.Raycast(Vector3.up * 100 + result, Vector3.down,out hit))
				result = hit.point;
		}
		else
		{
			var hits = Physics.RaycastAll(Vector3.up * 100 + result, Vector3.down);
			float maxHeght = float.MinValue;
			foreach(var hit in hits)
			{
				if(hit.collider.gameObject.tag == m_SpawnAreaTag)
				{
					if (maxHeght >= hit.point.y)
						continue;
					
					result = hit.point;
					maxHeght = hit.point.y;
					
				}
			}
		}

		return result;
	}

}
