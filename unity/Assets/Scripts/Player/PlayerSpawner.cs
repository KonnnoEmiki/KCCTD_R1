using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RandomSpawnPointGenerator))]
public class PlayerSpawner : MonoBehaviour
{
	[SerializeField,AssetPath]
	private string m_SpawnPrefab = string.Empty;
    [SerializeField, AssetPath]
    private string m_SpawnPrefab1 = string.Empty;
    [SerializeField, AssetPath]
    private string m_SpawnPrefab2 = string.Empty;


    void Update()
	{
		if (MonobitEngine.MonobitNetwork.inRoom == false)
			return;

        if (NetworkGUI.Charaselect == 0)
            SpawnPrefab();
        if (NetworkGUI.Charaselect == 1)
            SpawnPrefab1();
        if (NetworkGUI.Charaselect == 2)
            SpawnPrefab2();
        GM.first = false;
		Destroy(this);
	}

	private void SpawnPrefab()
	{
		var spawnPointGenerator = GetComponent<RandomSpawnPointGenerator>();
		if (string.IsNullOrEmpty(m_SpawnPrefab) == false && spawnPointGenerator != null)
			MonobitEngine.MonobitNetwork.Instantiate(m_SpawnPrefab, spawnPointGenerator.GenerateSpawnPoint(), Quaternion.identity, 0);	
	}
    private void SpawnPrefab1()
    {
        var spawnPointGenerator = GetComponent<RandomSpawnPointGenerator>();
        if (string.IsNullOrEmpty(m_SpawnPrefab1) == false && spawnPointGenerator != null)
            MonobitEngine.MonobitNetwork.Instantiate(m_SpawnPrefab1, spawnPointGenerator.GenerateSpawnPoint(), Quaternion.identity, 0);
    }
    private void SpawnPrefab2()
    {
        var spawnPointGenerator = GetComponent<RandomSpawnPointGenerator>();
        if (string.IsNullOrEmpty(m_SpawnPrefab2) == false && spawnPointGenerator != null)
            MonobitEngine.MonobitNetwork.Instantiate(m_SpawnPrefab2, spawnPointGenerator.GenerateSpawnPoint(), Quaternion.identity, 0);
    }
}
