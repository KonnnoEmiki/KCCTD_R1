﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class Supply : MonobitEngine.MonoBehaviour
{

    void OnTriggerStay(Collider hit)
    {
        if (hit.CompareTag("Player") && PlayerController.Flag)
        {
            if (NetworkGUI.stageselect == 4) PlayerController.shotCount = 10;
            else PlayerController.shotCount = 6;
            monobitView.RPC("Activefalse", MonobitEngine.MonobitTargets.All, null);
            PlayerController.Flag = false;
        }
    }

    [MunRPC]
    private void Activefalse()
    {
        this.gameObject.SetActive(false);
    }
}