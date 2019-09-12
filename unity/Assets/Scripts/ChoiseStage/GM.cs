using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GM : MonobitEngine.MonoBehaviour
{

    [SerializeField]
    private GameObject host = null;
    [SerializeField]
    private GameObject BallLuncher = null;
    [SerializeField]
    private GameObject choise = null;
    [SerializeField]
    private GameObject Stage = null;
    [SerializeField]
    private GameObject ForestStage = null;
    [SerializeField]
    private GameObject Plane = null;

    public static bool first = true;

    [MunRPC]
    private void Update()
    {
        if (NetworkGUI.roommaster == true && first == true)
            host.gameObject.tag = "master";
        if (first == false)
            host.gameObject.tag = "Player";
        if (NetworkGUI.gs == true)
        {
            choise.gameObject.SetActive(false);
            GameObject[] tagobjs = GameObject.FindGameObjectsWithTag("master");
            foreach (GameObject obj in tagobjs)
            {
                Destroy(obj);
            }
        }
        var roomData = MonobitEngine.MonobitNetwork.room;
        if (NetworkGUI.stageselect == 0)
            if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                if (MonobitEngine.MonobitNetwork.isHost == true)
                    monobitView.RPC("stagechange0", MonobitEngine.MonobitTargets.All, null);
        if (NetworkGUI.stageselect == 1)
            if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                if (MonobitEngine.MonobitNetwork.isHost == true)
                    monobitView.RPC("stagechange1", MonobitEngine.MonobitTargets.All, null);
        if (NetworkGUI.stageselect == 2)
            if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                if (MonobitEngine.MonobitNetwork.isHost == true)
                    monobitView.RPC("stagechange2", MonobitEngine.MonobitTargets.All, null);
        if (NetworkGUI.gamemode == 0)
            if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                if (MonobitEngine.MonobitNetwork.isHost == true)
                    monobitView.RPC("gamemode0", MonobitEngine.MonobitTargets.All, null);
        if (NetworkGUI.gamemode == 1)
            if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                if (MonobitEngine.MonobitNetwork.isHost == true)
                    monobitView.RPC("gamemode1", MonobitEngine.MonobitTargets.All, null);
    }

    [MunRPC]
    private void stagechange0()
    {
        NetworkGUI.stageselect = 0;
        Stage.gameObject.SetActive(false);
        Plane.gameObject.SetActive(true);
        ForestStage.gameObject.SetActive(false);
    }

    [MunRPC]
    private void stagechange1()
    {
        NetworkGUI.stageselect = 1;
        Stage.gameObject.SetActive(true);
        Plane.gameObject.SetActive(false);
        ForestStage.gameObject.SetActive(false);
    }

    [MunRPC]
    private void stagechange2()
    {
        NetworkGUI.stageselect = 2;
        ForestStage.gameObject.SetActive(true);
        Plane.gameObject.SetActive(false);
        Stage.gameObject.SetActive(false);
    }

    [MunRPC]
    private void gamemode0()
    {
        NetworkGUI.gamemode = 0;
        BallLuncher.gameObject.SetActive(true);
    }

    [MunRPC]
    private void gamemode1()
    {
        NetworkGUI.gamemode = 1;
        BallLuncher.gameObject.SetActive(false);
    }

}