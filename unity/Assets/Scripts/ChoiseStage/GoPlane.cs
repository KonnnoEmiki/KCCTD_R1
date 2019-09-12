﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoPlane : MonobitEngine.MonoBehaviour
{
    [SerializeField]
    private GameObject Plane = null;
    [SerializeField]
    private GameObject Stage = null;
    [SerializeField]
    private GameObject ForestStage = null;

    public Text StageLabel;

    [MunRPC]
    void Update()
    {
        var roomData = MonobitEngine.MonobitNetwork.room;
        if (NetworkGUI.stageselect == 1)
            if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                if (MonobitEngine.MonobitNetwork.isHost == true)
                    monobitView.RPC("stagechange1", MonobitEngine.MonobitTargets.All, null);
    }

    [MunRPC]
    void OnTriggerEnter(Collider hit)
    {
        if (hit.CompareTag("master"))
        {
            var roomData = MonobitEngine.MonobitNetwork.room;

            if (monobitView.isMine == true && NetworkGUI.roommaster == true)
                if (MonobitEngine.MonobitNetwork.isHost == true)
                    monobitView.RPC("stagechange1", MonobitEngine.MonobitTargets.All, null);
        }
    }

    [MunRPC]
    private void stagechange1()
    {
        NetworkGUI.stageselect = 1;
        Stage.gameObject.SetActive(true);
        Plane.gameObject.SetActive(false);
        ForestStage.gameObject.SetActive(false);
        StageLabel.text = "Plane";
    }
}