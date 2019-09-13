using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoSky : MonobitEngine.MonoBehaviour
{
    [SerializeField]
    private GameObject Plane = null;
    [SerializeField]
    private GameObject Stage = null;
    [SerializeField]
    private GameObject ForestStage = null;
    [SerializeField]
    private GameObject ShrineStage = null;
    [SerializeField]
    private GameObject SkyStage = null;

    [MunRPC]
    void Update()
    {
        var roomData = MonobitEngine.MonobitNetwork.room;
        if (NetworkGUI.stageselect == 4)
            if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                if (MonobitEngine.MonobitNetwork.isHost == true)
                    monobitView.RPC("stagechange4", MonobitEngine.MonobitTargets.All, null);
    }

    [MunRPC]
    void OnTriggerEnter(Collider hit)
    {
        if (hit.CompareTag("master"))
        {
            var roomData = MonobitEngine.MonobitNetwork.room;

            if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                if (MonobitEngine.MonobitNetwork.isHost == true)
                    monobitView.RPC("stagechange4", MonobitEngine.MonobitTargets.All, null);
        }
    }

    [MunRPC]
    private void stagechange4()
    {
        NetworkGUI.stageselect = 4;
        Stage.gameObject.SetActive(false);
        Plane.gameObject.SetActive(false);
        ForestStage.gameObject.SetActive(false);
        ShrineStage.gameObject.SetActive(false);
        SkyStage.gameObject.SetActive(true);
    }
}