using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class modeRun : MonoBehaviour
{
    [SerializeField]
    private GameObject BallLuncher = null;

    // トリガーとの接触時に呼ばれるコールバック
    void OnTriggerEnter(Collider hit)
    {
        // 接触対象はPlayerタグですか？
        if (hit.CompareTag("Player"))
            if (RoomManager.IsHost == true)
            {
            GM.gamemode = 1;
            BallLuncher.gameObject.SetActive(true);
            }
    }
}