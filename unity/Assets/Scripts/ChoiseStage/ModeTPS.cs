using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeTPS : MonoBehaviour
{
    [SerializeField]
    private GameObject Object = null;
    // Update is called once per frame
    void Update()
    {
        if (NetworkGUI.TPSflag == true)
            Object.gameObject.SetActive(true);
        else if (NetworkGUI.TPSflag == false)
            Object.gameObject.SetActive(false);
    }
}
