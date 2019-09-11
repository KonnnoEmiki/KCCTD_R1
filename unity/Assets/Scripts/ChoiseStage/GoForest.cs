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

    private void Update()
    {
        if (NetworkGUI.roommaster == false)
            this.gameObject.SetActive(false);
        if (NetworkGUI.roommaster == true)
            this.gameObject.SetActive(true);
    }

    // トリガーとの接触時に呼ばれるコールバック
    [MunRPC]
    void OnTriggerEnter(Collider hit)
    {
        // 接触対象はmasterタグですか？
        if (hit.CompareTag("master"))
        {
            var roomData = MonobitEngine.MonobitNetwork.room;

            if (monobitView.isMine == true && NetworkGUI.roommaster == true)
            {
                if (MonobitEngine.MonobitNetwork.isHost == true)
                    monobitView.RPC("stagechange2", MonobitEngine.MonobitTargets.All, null);
            }
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