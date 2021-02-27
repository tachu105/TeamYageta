using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class　Bullet : MonoBehaviour
{
    // 弾丸の速度
    public float damage = 10f;
    public float speed = 10f;
    public Vector3 dir = Vector3.zero;
    [SerializeField] private float range = 20f;
    [SerializeField] private string[] throughTags;
    [SerializeField] private GameObject HitEffect;
    [SerializeField] private GameObject breakEffect;
    private float totalLength = 0f;
    public GameObject parent;

    private void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (dir == Vector3.zero) return;
        float moveVal = speed * Time.deltaTime;
        this.transform.Translate(dir * moveVal);
        totalLength += moveVal;
        if (totalLength > range) Destroy(this.gameObject);
    }

    //着弾時処理
    private void OnCollisionEnter(Collision collision)
    {
        if (throughTags.Contains<string>(collision.gameObject.tag)) return;

        switch (collision.gameObject.tag)
        {
            case "Player":
                break;
            case "Enemy":
                HitArea hitArea = collision.gameObject.GetComponent<HitArea>();
                Enemy enemy = hitArea.enemy;
                enemy.Damage(this, hitArea);
                Instantiate(HitEffect, this.transform.position, Quaternion.identity);
                Destroy(this.gameObject);
                break;
            case "Bullet":
                Bullet otherBullet = collision.gameObject.GetComponent<Bullet>();
                if (!otherBullet.parent || otherBullet.parent == this.parent) return;
                break;
            default:
                //Debug.Log("Hit on " + collision.gameObject.name);
                Instantiate(breakEffect, this.transform.position, Quaternion.identity);
                Destroy(this.gameObject);
                break;
        }

    }
}
