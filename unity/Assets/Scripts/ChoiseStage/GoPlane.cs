using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoPlane : MonoBehaviour
{
    [SerializeField]
    private GameObject Stage = null;
    [SerializeField]
    private GameObject ForestStage = null;
    [SerializeField]
    private GameObject Plane = null;
    // トリガーとの接触時に呼ばれるコールバック
    void OnTriggerEnter(Collider hit)
    {
        // 接触対象はPlayerタグですか？
        if (hit.CompareTag("Player"))
            if (RoomManager.IsHost == true)
            {
            GM.stageselect = 1;
            Stage.gameObject.SetActive(true);
            Plane.gameObject.SetActive(false);
            ForestStage.gameObject.SetActive(false);
            }
    }
}