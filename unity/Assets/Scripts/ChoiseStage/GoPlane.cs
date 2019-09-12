﻿using System.Collections;
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
    {
        if (GM.stageselect == 1)
        {
            var roomData = MonobitEngine.MonobitNetwork.room;

            if (monobitView.isMine == true && NetworkGUI.roommaster == true)
            {
                if (MonobitEngine.MonobitNetwork.isHost == true)
                    monobitView.RPC("stagechange1", MonobitEngine.MonobitTargets.All, null);
            }
        }
    }


    [MunRPC]
    void OnTriggerEnter(Collider hit)
    {
        // 接触対象はmasterタグですか？
        if (hit.CompareTag("master"))
        {
            var roomData = MonobitEngine.MonobitNetwork.room;

            if (monobitView.isMine == true && NetworkGUI.roommaster==true)
            {
                if (MonobitEngine.MonobitNetwork.isHost == true)
                    monobitView.RPC("stagechange1", MonobitEngine.MonobitTargets.All, null);
            }
        }
    }

    [MunRPC]
    private void stagechange1()
    {
        GM.stageselect = 1;
        Stage.gameObject.SetActive(true);
        Plane.gameObject.SetActive(false);
        ForestStage.gameObject.SetActive(false);
    }
}