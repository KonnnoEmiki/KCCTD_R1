using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class Supply : MonoBehaviour
{
    public static int HIT;
    private static bool a = false;
    
    void OnTriggerStay(Collider hit)
    {
        if (hit.CompareTag("master")) HIT = 1;
    }
}