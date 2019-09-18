using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeItem : MonoBehaviour
{
    [SerializeField]
    private GameObject Object = null;
    // Update is called once per frame
    void Update()
    {
        if (NetworkGUI.Itemflag == true)
            Object.gameObject.SetActive(true);
        else if (NetworkGUI.Itemflag == false)
            Object.gameObject.SetActive(false);
    }
}