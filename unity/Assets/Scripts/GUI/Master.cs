using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour
{
    public GameObject Menu;
    private bool MenuOn = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E) && MenuOn == true)
        {
            Menu.SetActive(false);
            MenuOn = false;
            //startcoroutine("Delay");
            return;
        }
        if (Input.GetKey(KeyCode.E) && MenuOn == false)
        {
            Menu.SetActive(true);
            MenuOn = true;
            //startcoroutine("Delay");
            return;
        }
    }

   /* IEnumerator Delay()
    {
        yield return new WaitForSeconds(1.0f);
    }*/
}
