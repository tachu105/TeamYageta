using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class　Bullet : MonoBehaviour
{
    // 弾丸の速度
    public float damage = 10f;
    public float speed = 10f;
    public Vector3 dir = Vector3.zero;


    private void Start()
    {
        if (dir == Vector3.zero) dir = this.transform.forward;
    }
    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(dir * speed*Time.deltaTime);
        Destroy(this.gameObject, 5f);
    }

    //着弾時処理
    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                break;
            case "Enemy":
                HitArea hitArea = collision.gameObject.GetComponent<HitArea>();
                Enemy enemy = hitArea.enemy;
                enemy.Damage(this, hitArea);
                break;
            default:
                break;
        }

    }
}
