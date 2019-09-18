using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeKiller : MonoBehaviour
{
    [SerializeField]
    private GameObject Object = null;
    // Update is called once per frame
    void Update()
    {
        if (NetworkGUI.Trapflag == true)
            Object.gameObject.SetActive(true);
        else if (NetworkGUI.Trapflag == false)
            Object.gameObject.SetActive(false);
    }
}