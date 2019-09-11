using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoPlane : MonobitEngine.MonoBehaviour
{
    [SerializeField]
    private GameObject Stage = null;
    [SerializeField]
    private GameObject ForestStage = null;
    [SerializeField]
    private GameObject Plane = null;
    // トリガーとの接触時に呼ばれるコールバック
    [MunRPC]
    void OnTriggerEnter(Collider hit)
    {
        // 接触対象はPlayerタグですか？
        if (hit.CompareTag("Player"))
            if (RoomManager.IsHost == true)
                if (monobitView.isMine == true)
                {
                    monobitView.RPC("stagechange1", MonobitEngine.MonobitTargets.All, null);
                }
    }

    [MunRPC]
    void stagechange1()
    {
        GM.stageselect = 1;
        Stage.gameObject.SetActive(true);
        Plane.gameObject.SetActive(false);
        ForestStage.gameObject.SetActive(false);
    }
}