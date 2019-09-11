using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class Supply : MonoBehaviour
{
    public static int HIT;

    private void Update()
    {
        if (HIT == 1)
            this.gameObject.SetActive(false);
    }

    void OnTriggerStay(Collider hit)
    {
        if (hit.CompareTag("master"))
        {
            HIT = 1;
        }
    }
}