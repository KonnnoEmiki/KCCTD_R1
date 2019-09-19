using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonobitEngine.MonoBehaviour
{

    [SerializeField]
    private GameObject hostA = null;
    [SerializeField]
    private GameObject hostB = null;
    [SerializeField]
    private GameObject hostC = null;
    [SerializeField]
    private GameObject Item = null;
    [SerializeField]
    private GameObject Trap = null;
    [SerializeField]
    private GameObject TPS = null;
    [SerializeField]
    private GameObject choise = null;
    [SerializeField]
    private GameObject Stage = null;
    [SerializeField]
    private GameObject ForestStage = null;
    [SerializeField]
    private GameObject ShrineStage = null;
    [SerializeField]
    private GameObject SkyStage = null;
    [SerializeField]
    private GameObject Plane = null;
    [SerializeField]
    private GameObject flagTp = null;
    [SerializeField]
    private GameObject flagB = null;
    [SerializeField]
    private GameObject flagI = null;
    [SerializeField]
    private GameObject flagTr = null;

    public static bool first = false;

    private bool start = true;

    private void Start()
    {
        hostA.gameObject.tag = "Player";
        hostB.gameObject.tag = "Player";
        hostC.gameObject.tag = "Player";
    }

    [MunRPC]
    private void Update()
    {
        if (NetworkGUI.roommaster == true && first == true)
        {
            hostA.gameObject.tag = "master";
            hostB.gameObject.tag = "master";
            hostC.gameObject.tag = "master";
        }
        if (first == false && NetworkGUI.gs == false&& NetworkGUI.roommaster == true)
        {
            hostA.gameObject.tag = "Phantom";
            hostB.gameObject.tag = "Phantom";
            hostC.gameObject.tag = "Phantom";
        }

        if (NetworkGUI.gs == true && start == true)
                    monobitView.RPC("kill", MonobitEngine.MonobitTargets.All, null);

        var roomData = MonobitEngine.MonobitNetwork.room;
        
        {

            //Stage
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
            if (NetworkGUI.stageselect == 3)
                if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                    if (MonobitEngine.MonobitNetwork.isHost == true)
                        monobitView.RPC("stagechange3", MonobitEngine.MonobitTargets.All, null);
            if (NetworkGUI.stageselect == 4)
                if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                    if (MonobitEngine.MonobitNetwork.isHost == true)
                        monobitView.RPC("stagechange4", MonobitEngine.MonobitTargets.All, null);

            //BallLuncher
            if (NetworkGUI.Ballflag == false)
                if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                    if (MonobitEngine.MonobitNetwork.isHost == true)
                        monobitView.RPC("Balloff", MonobitEngine.MonobitTargets.All, null);
            if (NetworkGUI.Ballflag == true)
                if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                    if (MonobitEngine.MonobitNetwork.isHost == true)
                        monobitView.RPC("Ballon", MonobitEngine.MonobitTargets.All, null);
            //Item
            if (NetworkGUI.Itemflag == false)
                if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                    if (MonobitEngine.MonobitNetwork.isHost == true)
                        monobitView.RPC("Itemoff", MonobitEngine.MonobitTargets.All, null);
            if (NetworkGUI.Itemflag == true)
                if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                    if (MonobitEngine.MonobitNetwork.isHost == true)
                        monobitView.RPC("Itemon", MonobitEngine.MonobitTargets.All, null);

            //Trap
            if (NetworkGUI.Trapflag == false)
                if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                    if (MonobitEngine.MonobitNetwork.isHost == true)
                        monobitView.RPC("Trapoff", MonobitEngine.MonobitTargets.All, null);
            if (NetworkGUI.Trapflag == true)
                if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                    if (MonobitEngine.MonobitNetwork.isHost == true)
                        monobitView.RPC("Trapon", MonobitEngine.MonobitTargets.All, null);

            //TPS
            if (NetworkGUI.TPSflag == false)
                if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                    if (MonobitEngine.MonobitNetwork.isHost == true)
                        monobitView.RPC("TPSoff", MonobitEngine.MonobitTargets.All, null);
            if (NetworkGUI.TPSflag == true)
                if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                    if (MonobitEngine.MonobitNetwork.isHost == true)
                        monobitView.RPC("TPSon", MonobitEngine.MonobitTargets.All, null);

        }
    }

    [MunRPC]
    private void kill()
    {
        GameObject[] tagobjs1 = GameObject.FindGameObjectsWithTag("Phantom");
        foreach (GameObject obj in tagobjs1)
        {
            obj.gameObject.tag = "Player";
        }
        GameObject[] tagobjs2 = GameObject.FindGameObjectsWithTag("Phantom");
        foreach (GameObject obj in tagobjs2)
        {
            Destroy(obj);
        }
        GameObject[] tagobjs = GameObject.FindGameObjectsWithTag("master");
        foreach (GameObject obj in tagobjs)
        {
            Destroy(obj);
        }
        choise.gameObject.SetActive(false);
        start = false;
    }
    

    [MunRPC]
    private void stagechange0()
    {
        Debug.Log("stage0");
        NetworkGUI.stageselect = 0;
        Stage.gameObject.SetActive(false);
        Plane.gameObject.SetActive(true);
        ForestStage.gameObject.SetActive(false);
        ShrineStage.gameObject.SetActive(false);
        SkyStage.gameObject.SetActive(false);
    }

    [MunRPC]
    private void stagechange1()
    {
        Debug.Log("stage1");
        NetworkGUI.stageselect = 1;
        Stage.gameObject.SetActive(true);
        Plane.gameObject.SetActive(false);
        ForestStage.gameObject.SetActive(false);
        ShrineStage.gameObject.SetActive(false);
        SkyStage.gameObject.SetActive(false);
    }

    [MunRPC]
    private void stagechange2()
    {
        Debug.Log("stage2");
        NetworkGUI.stageselect = 2;
        ForestStage.gameObject.SetActive(true);
        Plane.gameObject.SetActive(false);
        Stage.gameObject.SetActive(false);
        ShrineStage.gameObject.SetActive(false);
        SkyStage.gameObject.SetActive(false);
    }

    [MunRPC]
    private void stagechange3()
    {
        Debug.Log("stage3");
        NetworkGUI.stageselect = 3;
        ForestStage.gameObject.SetActive(false);
        Plane.gameObject.SetActive(false);
        Stage.gameObject.SetActive(false);
        ShrineStage.gameObject.SetActive(true);
        SkyStage.gameObject.SetActive(false);
    }

    [MunRPC]
    private void stagechange4()
    {
        Debug.Log("stage4");
        NetworkGUI.stageselect = 4;
        ForestStage.gameObject.SetActive(false);
        Plane.gameObject.SetActive(false);
        Stage.gameObject.SetActive(false);
        ShrineStage.gameObject.SetActive(false);
        SkyStage.gameObject.SetActive(true);
    }


    [MunRPC]
    private void Balloff()
    {
        NetworkGUI.Ballflag = false;
        flagB.gameObject.SetActive(false);
    }

    [MunRPC]
    private void Ballon()
    {
        NetworkGUI.Ballflag = true;
        flagB.gameObject.SetActive(true);
    }

    [MunRPC]
    private void Itemoff()
    {
        NetworkGUI.Itemflag = false;
        Item.gameObject.SetActive(false);
        flagI.gameObject.SetActive(false);

    }

    [MunRPC]
    private void Itemon()
    {
        NetworkGUI.Itemflag = true;
        Item.gameObject.SetActive(true);
        flagI.gameObject.SetActive(true);
    }

    [MunRPC]
    private void Trapoff()
    {
        NetworkGUI.Trapflag = false;
        Trap.gameObject.SetActive(false);
        flagTr.gameObject.SetActive(false);

    }

    [MunRPC]
    private void Trapon()
    {
        NetworkGUI.Trapflag = true;
        Trap.gameObject.SetActive(true);
        flagTr.gameObject.SetActive(true);
    }

    [MunRPC]
    private void TPSoff()
    {
        NetworkGUI.TPSflag = false;
        TPS.gameObject.SetActive(false);
        flagTp.gameObject.SetActive(false);
    }

    [MunRPC]
    private void TPSon()
    {
        NetworkGUI.TPSflag = true;
        TPS.gameObject.SetActive(true);
        flagTp.gameObject.SetActive(true);
    }
}