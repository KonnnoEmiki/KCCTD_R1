using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class modeButtle : MonobitEngine.MonoBehaviour
{
    [SerializeField]
    private GameObject BallLuncher = null;

    // トリガーとの接触時に呼ばれるコールバック
    [MunRPC]
    void OnTriggerEnter(Collider hit)
    {
        // 接触対象はPlayerタグですか？
        if (hit.CompareTag("Player"))
            if (RoomManager.IsHost == true)
                if(monobitView.isMine==true)
               {
            GM.gamemode = 2;
            BallLuncher.gameObject.SetActive(false);
            }
    }
}