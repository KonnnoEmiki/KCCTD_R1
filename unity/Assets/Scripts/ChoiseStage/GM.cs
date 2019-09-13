using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonobitEngine.MonoBehaviour
{

    [SerializeField]
    private GameObject host = null;
    [SerializeField]
    private GameObject TPS = null;
    [SerializeField]
    private GameObject Item = null;
    [SerializeField]
    private GameObject Trap = null;
    [SerializeField]
    private GameObject choise = null;
    [SerializeField]
    private GameObject Stage = null;
    [SerializeField]
    private GameObject ForestStage = null;
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

    public static bool first = true;

    private bool start = true;

    [MunRPC]
    private void Update()
    {
        if (NetworkGUI.roommaster == true && first == true)
            host.gameObject.tag = "master";
        if (first == false)
            host.gameObject.tag = "Player";
        if (NetworkGUI.gs == true && start == true)
        {
            choise.gameObject.SetActive(false);
            GameObject[] tagobjs = GameObject.FindGameObjectsWithTag("master");
            foreach (GameObject obj in tagobjs)
            {
                Destroy(obj);
            }
            start = false;
            if (NetworkGUI.roommaster == false)
            {
                GameObject[] tagobjs1 = GameObject.FindGameObjectsWithTag("Player");
                foreach (GameObject obj in tagobjs1)
                {
                    Destroy(obj);
                }
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

        if (NetworkGUI.Ballflag == false)
            if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                if (MonobitEngine.MonobitNetwork.isHost == true)
                    monobitView.RPC("Balloff", MonobitEngine.MonobitTargets.All, null);
        if (NetworkGUI.Ballflag == true)
            if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                if (MonobitEngine.MonobitNetwork.isHost == true)
                    monobitView.RPC("Ballon", MonobitEngine.MonobitTargets.All, null);

        if (NetworkGUI.gs == true)
        {
            if (NetworkGUI.Itemflag == false)
                if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                    if (MonobitEngine.MonobitNetwork.isHost == true)
                        monobitView.RPC("Itemoff", MonobitEngine.MonobitTargets.All, null);
            if (NetworkGUI.Itemflag == true)
                if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                    if (MonobitEngine.MonobitNetwork.isHost == true)
                        monobitView.RPC("Itemon", MonobitEngine.MonobitTargets.All, null);

            if (NetworkGUI.Trapflag == false)
                if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                    if (MonobitEngine.MonobitNetwork.isHost == true)
                        monobitView.RPC("Trapoff", MonobitEngine.MonobitTargets.All, null);
            if (NetworkGUI.Trapflag == true)
                if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                    if (MonobitEngine.MonobitNetwork.isHost == true)
                        monobitView.RPC("Trapon", MonobitEngine.MonobitTargets.All, null);
            
            if (NetworkGUI.TPSflag == false)
                if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                    if (MonobitEngine.MonobitNetwork.isHost == true)
                        monobitView.RPC("TPSoff", MonobitEngine.MonobitTargets.All, null);
            if (NetworkGUI.Trapflag == true)
                if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                    if (MonobitEngine.MonobitNetwork.isHost == true)
                        monobitView.RPC("TPSon", MonobitEngine.MonobitTargets.All, null);
        }
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