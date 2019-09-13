using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class SupplyBrain : MonoBehaviour
{
    public static bool flag = true;
    public static float Time = 5;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        var obj = transform.Find("cartridge").gameObject;
        if (!obj.activeInHierarchy&&flag)
        {
            flag = false;
            StartCoroutine("sleep");
            Time = 5;
        }
    }

    IEnumerator sleep()
    {
        var obj = transform.Find("cartridge").gameObject;
        yield return new WaitForSeconds(Time);
        obj.SetActive(true);
        flag = true;
    }

}
