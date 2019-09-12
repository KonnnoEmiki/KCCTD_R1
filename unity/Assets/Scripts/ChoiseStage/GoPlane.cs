using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoPlane : MonobitEngine.MonoBehaviour
{
    [SerializeField]
    private GameObject Plane = null;
    [SerializeField]
    private GameObject Stage = null;
    [SerializeField]
    private GameObject ForestStage = null;

    [MunRPC]
    void Start()
    {
        var roomData = MonobitEngine.MonobitNetwork.room;
        if (GM.stageselect == 3)
            if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                if (MonobitEngine.MonobitNetwork.isHost == true)
                    monobitView.RPC("stagechange3", MonobitEngine.MonobitTargets.All, null);
    }

    [MunRPC]
    void OnTriggerEnter(Collider hit)
    {
        if (hit.CompareTag("master"))
        {
            var roomData = MonobitEngine.MonobitNetwork.room;

            if (monobitView.isMine == true && NetworkGUI.roommaster == true)
            {
                if (MonobitEngine.MonobitNetwork.isHost == true)
                    monobitView.RPC("stagechange3", MonobitEngine.MonobitTargets.All, null);
            }
        }
    }

    [MunRPC]
    private void stagechange3()
    {
        GM.stageselect = 3;
        Stage.gameObject.SetActive(true);
        Plane.gameObject.SetActive(false);
        ForestStage.gameObject.SetActive(false);
    }
}