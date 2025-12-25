using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coba : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject mpickup = MissilePool.instance.GetFromPool("pickup");
            mpickup.transform.position = new Vector3(1, 1, 1);

            mpickup.SetActive(true);


        }
    }
}
