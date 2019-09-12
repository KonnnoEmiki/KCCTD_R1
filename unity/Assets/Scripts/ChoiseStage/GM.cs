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
    private GameObject Stage = null;
    [SerializeField]
    private GameObject ForestStage = null;
    [SerializeField]
    private GameObject Plane = null;

    public static bool first = true;

    void Start()
    {
        stageselect = 0;
        first = true;
    }

    [MunRPC]
    private void Update()
    {
        if (NetworkGUI.roommaster == true && first == true)
            host.gameObject.tag = "master";
        if (first == false)
            host.gameObject.tag = "Player";
        if (NetworkGUI.gs == true)
            this.gameObject.SetActive(false);
        if (stageselect == 0)
        {
            var roomData = MonobitEngine.MonobitNetwork.room;

            if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                if (MonobitEngine.MonobitNetwork.isHost == true)
                    monobitView.RPC("stagechange", MonobitEngine.MonobitTargets.All, null);
        }
    }




    [MunRPC]
    private void stagechange()
    {
        GM.stageselect = 0;
        Stage.gameObject.SetActive(false);
        Plane.gameObject.SetActive(true);
        ForestStage.gameObject.SetActive(false);

    }
}