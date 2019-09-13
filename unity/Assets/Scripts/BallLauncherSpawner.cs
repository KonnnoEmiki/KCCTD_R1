using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 // BallLauncherのスポナー
public class BallLauncherSpawner : MonoBehaviour<BallLauncherSpawner>
{
	[SerializeField, AssetPath]
	private string m_LauncherPrefabPath;

	public void SpawnLauncher()
	{
        if (NetworkGUI.gs == true && NetworkGUI.Ballflag == true)
        {
            Vector3 spherePos = Random.onUnitSphere;
            var areaRadius = GameManager.Instance.AreaCollider.bounds.size.x * 0.5f * 0.9f;
            spherePos *= areaRadius;
            spherePos.y = Mathf.Abs(spherePos.y);
            Quaternion rotation = Quaternion.LookRotation(-spherePos);

            MonobitEngine.MonobitNetwork.Instantiate(m_LauncherPrefabPath, spherePos, rotation, 0);
        }
	}
}
