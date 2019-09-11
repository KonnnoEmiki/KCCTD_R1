using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class Supply : MonoBehaviour
{
    public static int HIT;

    private void Start()
    {
        HIT = 0;
    }

    private void Update()
    {
        if (HIT == 1)
            HIT = 0;
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