using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() { }
    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * 1500 * Time.fixedDeltaTime;
    }      //赤字の部分だけ追加しました
}