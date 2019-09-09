using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{

    public GameObject bulletPrefab;
    public float shotSpeed;
    public int shotCount = 6;
    public float starttime;
    public float now;
    private float shotInterval;
    public Text shellLabel;

    void start()
    {
        shellLabel.text = "玉：6";
    }

    void Update()
    {
        Transform myTransform = this.transform.parent;
        Vector3 pos = myTransform.position;
        now = Time.time;
        if (Input.GetKey(KeyCode.Mouse0))
        {

            shotInterval += 1;

            if (shotInterval % 5 == 0 && shotCount > 0)
            {
                shotCount -= 1;
                shellLabel.text = "玉：" + shotCount;

                GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position, Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, 0));
                Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
                bulletRb.AddForce(transform.forward * shotSpeed);

                //射撃されてから3秒後に弾のオブジェクトを破壊する.

                Destroy(bullet, 3.0f);
            }

        }
        else if (pos.x>=-1&&pos.x<=1&&pos.z>=-1&&pos.z<=1&&starttime<=now-5)
        {
            starttime = Time.time;
            shotCount = 6;
            shellLabel.text = "玉：" + shotCount;
        }

    }
}