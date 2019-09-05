using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// シーン管理クラス
public class SceneManager : MonoBehaviour
{
	// シーン読み込み
	public void LoadScene(string sceneName,LoadSceneMode mode = LoadSceneMode.Single)
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, mode);
	}

	// シーン読み込み
	public void LoadScene(int sceneIndex,LoadSceneMode mode = LoadSceneMode.Single)
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex, mode);
	}

	// フェードを挟んでシーン読み込み
	public void FadeToLoadScene(string sceneName,LoadSceneMode mode = LoadSceneMode.Single)
	{
		var fade = ApplicationManager.Fade;

		fade.AddFadeDoneEvent(
			() =>
			{
				fade.RemoveAllFadeDoneEvent();
				LoadScene(sceneName, mode);
				fade.Play(false);
			});

		ApplicationManager.Fade.Play(true);
	}

	// フェードを挟んでシーン読み込み
	public void FadeToLoadScene(int sceneIndex,LoadSceneMode mode = LoadSceneMode.Single)
	{
		var fade = ApplicationManager.Fade;

		fade.AddFadeDoneEvent(
			() =>
			{
				fade.RemoveAllFadeDoneEvent();
				LoadScene(sceneIndex, mode);
				fade.Play(false);
			});

		ApplicationManager.Fade.Play(true);
	}

	// 非同期でシーン読み込み
	public AsyncOperation LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
	{
		return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, mode);
	}

	// 非同期でシーン読み込み
	public AsyncOperation LoadSceneAsync(int sceneIndex, LoadSceneMode mode = LoadSceneMode.Single)
	{
		return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneIndex, mode);
	}

}
