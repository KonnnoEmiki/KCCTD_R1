using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoForest : MonobitEngine.MonoBehaviour
{
    [SerializeField]
    private GameObject ForestStage = null;
    [SerializeField]
    private GameObject Stage = null;
    [SerializeField]
    private GameObject Plane = null;

    // トリガーとの接触時に呼ばれるコールバック
    [MunRPC]
    void OnTriggerEnter(Collider hit)
    {
        // 接触対象はPlayerタグですか？
        if (hit.CompareTag("Player"))
            if (monobitView.isMine == true)
            {
                    monobitView.RPC("stagechange2", MonobitEngine.MonobitTargets.All, null);
                }
    }

    [MunRPC]
    private void stagechange2()
    {
        GM.stageselect = 2;
        ForestStage.gameObject.SetActive(true);
        Plane.gameObject.SetActive(false);
        Stage.gameObject.SetActive(false);
    }
}