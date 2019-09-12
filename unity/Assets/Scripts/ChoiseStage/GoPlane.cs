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

    /*private void Update()
    {
        if (NetworkGUI.roommaster == false)
            this.gameObject.SetActive(false);
        if (NetworkGUI.roommaster == true)
            this.gameObject.SetActive(true);
    }*/

    [MunRPC]
    private void Start()
    {var roomData = MonobitEngine.MonobitNetwork.room;
        if (GM.stageselect == 1)
        {
            

            //if (monobitView.isMine == true && NetworkGUI.roommaster == true)
            {
                // if (MonobitEngine.MonobitNetwork.isHost == true)
                monobitView.RPC("stagechange1", MonobitEngine.MonobitTargets.All, null);
            }
        }
    }

    // トリガーとの接触時に呼ばれるコールバック
    [MunRPC]
    void OnTriggerEnter(Collider hit)
    { var roomData = MonobitEngine.MonobitNetwork.room;
        // 接触対象はmasterタグですか？
        if (hit.CompareTag("master"))
        {
           

            // if (monobitView.isMine == true && NetworkGUI.roommaster == true)
            {
                //   if (MonobitEngine.MonobitNetwork.isHost == true)
                monobitView.RPC("stagechange1", MonobitEngine.MonobitTargets.All, null);
            }
        }
    }

    [MunRPC]
    private void stagechange1()
    {
        GM.stageselect = 1;
        ForestStage.gameObject.SetActive(false);
        Plane.gameObject.SetActive(false);
        Stage.gameObject.SetActive(true);
    }
}