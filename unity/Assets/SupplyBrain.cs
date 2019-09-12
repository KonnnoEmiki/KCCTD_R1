using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class SupplyBrain : MonoBehaviour
{
    public float HIT;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        var obj = transform.Find("cartridge").gameObject;
        if (!obj.activeInHierarchy)
        {
            StartCoroutine("sleep");
        }
    }

    IEnumerator sleep()
    {
        var obj = transform.Find("cartridge").gameObject;
        yield return new WaitForSeconds(5);
        obj.SetActive(true);
    }
}
