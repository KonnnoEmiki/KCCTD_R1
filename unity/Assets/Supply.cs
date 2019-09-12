using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class Supply : MonoBehaviour
{

    void OnTriggerStay(Collider hit)
    {
        if (hit.CompareTag("Player")|| hit.CompareTag("master"))
        {
            PlayerController.shotCount = 6;
            this.gameObject.SetActive(false);
        }
    }
}