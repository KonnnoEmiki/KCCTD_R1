using UnityEngine;
using System.Collections;
// ★追加
using UnityEngine.UI;

public class ShotShell : MonoBehaviour
{

    public GameObject shellPrefab;
    public float shotSpeed;
    public AudioClip shotSound;
    public int shotCount;

    // ★追加
    public Text shellLabel;

    void Start()
    {

        // ★追加
        shellLabel.text = "玉：" + shotCount;
    }

    void Update()
    {

        if (Input.GetButtonDown("Fire1"))
        {

            if (shotCount < 1)
                return;

            Shot();

            AudioSource.PlayClipAtPoint(shotSound, transform.position);

            shotCount -= 1;

            // ★追加
            shellLabel.text = "玉：" + shotCount;
        }

    }

    public void Shot()
    {

        GameObject shell = Instantiate(shellPrefab, transform.position, Quaternion.identity) as GameObject;

        Rigidbody shellRigidbody = shell.GetComponent<Rigidbody>();

        shellRigidbody.AddForce(transform.forward * shotSpeed);

        Destroy(shell, 2.0f);
    }
}