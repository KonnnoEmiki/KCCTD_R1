using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class Supply : MonobitEngine.MonoBehaviour
{

    void OnTriggerStay(Collider hit)
    {
        if (PlayerController.Flag)
            if (PlayerController.shotCount < 6)
            {
                PlayerController.shotCount = 6;
                monobitView.RPC("Activefalse", MonobitEngine.MonobitTargets.All, null);
            }
    }

    [MunRPC]
    private void Activefalse()
    {
        this.gameObject.SetActive(false);
    }
}