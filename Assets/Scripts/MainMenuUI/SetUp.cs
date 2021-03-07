using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUp : MonoBehaviour
{
    GameObject main;
    // Start is called before the first frame update
    void Start()
    {
        main = GameObject.Find("GameObject");
    }

    // Update is called once per frame
    void Update()
    {
        if(this.pressed)
        main.transform.position = Vector3.forward * Mathf.Lerp(-365.6289f, 0f, Time.deltaTime);
    }
}
