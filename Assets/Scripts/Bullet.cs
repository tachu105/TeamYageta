using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class　Bullet : MonoBehaviour
{
    // 弾丸の速度
    public float speed = 50;
    public Vector3 dir;


    // Start is called before the first frame update
    void Start()
    {
        dir = GameObject.Find("Player").transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(dir * speed*Time.deltaTime);
        Destroy(this.gameObject, 5f);
    }

    
}
