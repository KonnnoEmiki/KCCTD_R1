using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RandomSpawnPointGenerator))]
public class PlayerSpawner : MonoBehaviour
{
	[SerializeField,AssetPath]
	private string m_SpawnPrefab = string.Empty;

	void Update()
	{
		if (MonobitEngine.MonobitNetwork.inRoom == false)
			return;
		
		SpawnPrefab();
		Destroy(this);
	}

	private void SpawnPrefab()
	{
		var spawnPointGenerator = GetComponent<RandomSpawnPointGenerator>();
		if (string.IsNullOrEmpty(m_SpawnPrefab) == false && spawnPointGenerator != null)
			MonobitEngine.MonobitNetwork.Instantiate(m_SpawnPrefab, spawnPointGenerator.GenerateSpawnPoint(), Quaternion.identity, 0);	
	}

}
