using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RandomSpawnPointGenerator))]
public class PlayerSpawner : MonoBehaviour
{
	[SerializeField,AssetPath]
	private string m_SpawnPrefab0 = string.Empty;
    [SerializeField, AssetPath]
    private string m_SpawnPrefab1 = string.Empty;
    [SerializeField, AssetPath]
    private string m_SpawnPrefab2 = string.Empty;


    void Update()
	{
		if (MonobitEngine.MonobitNetwork.inRoom == false)
			return;
        if (NetworkGUI.CharaSelect == 0)
            SpawnPrefab0();
        else if (NetworkGUI.CharaSelect == 1)
            SpawnPrefab1();
        else if (NetworkGUI.CharaSelect == 2)
            SpawnPrefab2();
        GM.first = false;
		Destroy(this);
	}

	private void SpawnPrefab0()
	{
		var spawnPointGenerator = GetComponent<RandomSpawnPointGenerator>();
		if (string.IsNullOrEmpty(m_SpawnPrefab0) == false && spawnPointGenerator != null)
			MonobitEngine.MonobitNetwork.Instantiate(m_SpawnPrefab0, spawnPointGenerator.GenerateSpawnPoint(), Quaternion.identity, 0);	
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
