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
    void OnTriggerEnter(Collider hit)
    {
        // 接触対象はPlayerタグですか？
        if (hit.CompareTag("Player"))
            if (RoomManager.IsHost == true)
                if (monobitView.isMine == true)
                {
            GM.stageselect = 2;
            ForestStage.gameObject.SetActive(true);
            Plane.gameObject.SetActive(false);
            Stage.gameObject.SetActive(false);
            }
    }
}