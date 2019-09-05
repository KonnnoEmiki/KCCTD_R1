using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FPSログ出力用クラス
public class FpsLogger : MonoBehaviour<FpsLogger>
{
#if UNITY_EDITOR
	int frameCount;
	float prevTime;
	float fps;
	void Start()
	{
		frameCount = 0;
		prevTime = 0.0f;
	}

	void Update()
	{
		frameCount++;
		float time = Time.realtimeSinceStartup - prevTime;

		if (time >= 0.5f)
		{
			fps = frameCount / time;
			Debug.Log(fps);

			frameCount = 0;
			prevTime = Time.realtimeSinceStartup;
		}
	}
#endif
}
