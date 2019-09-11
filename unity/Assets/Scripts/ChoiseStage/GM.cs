using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonobitEngine.MonoBehaviour
{
    public static int stageselect = 0;
    public static int gamemode = 0;

    private int stageNo = 0;

    [SerializeField]
    private GameObject host = null;
    [SerializeField]
    private GameObject Canan = null;
    [SerializeField]
    private GameObject ForestStage = null;
    [SerializeField]
    private GameObject PlaneStage = null;

    public static bool first = true;

    [MunRPC]
    private void Update()
    {
        if (NetworkGUI.roommaster == true && first == true)
            host.gameObject.tag = "master";
        if (first == false)
            host.gameObject.tag = "Player";
        var roomData = MonobitEngine.MonobitNetwork.room;
        if (monobitView.isMine == true && NetworkGUI.roommaster == true)
        {
            if (MonobitEngine.MonobitNetwork.isHost == true)
                monobitView.RPC("stagechange", MonobitEngine.MonobitTargets.All, null);
        }
        if (NetworkGUI.gs == true) Destroy(gameObject);
    }

    [MunRPC]
    private void stagechange()
    {
        if (stageNo != stageselect)
        {
            stageNo = stageselect;
            if (stageselect == 1)
            {
                Canan.gameObject.SetActive(false);
                PlaneStage.gameObject.SetActive(true);
                ForestStage.gameObject.SetActive(false);
            }
            if (stageselect == 2)
            {
                ForestStage.gameObject.SetActive(true);
                PlaneStage.gameObject.SetActive(false);
                Canan.gameObject.SetActive(false);
            }
        }
    }

}