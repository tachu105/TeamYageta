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
    [SerializeField] protected float range = 20f;
    [SerializeField] protected string[] throughTags;
    [SerializeField] protected GameObject HitEffect;
    [SerializeField] protected GameObject breakEffect;
    private float totalLength = 0f;
    public GameObject parent;

    private void Start()
    {
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        if (dir == Vector3.zero) return;
        float moveVal = speed * Time.deltaTime;
        this.transform.Translate(dir * moveVal);
        totalLength += moveVal;
        if (totalLength > range) Destroy(this.gameObject);
    }

    //着弾時処理
    private void OnTriggerEnter(Collider collider)
    {
        if (throughTags.Contains<string>(collider.gameObject.tag)) return;

        switch (collider.gameObject.tag)
        {
            case "Player":
                HitPlayer(collider.gameObject);
                break;
            case "Enemy":
                HitEnemy(collider.gameObject);
                break;
            case "Bullet":
                HitBullet(collider.gameObject);
                break;
            default:
                HitOther(collider.gameObject);
                break;
        }
    }

    protected virtual void HitPlayer(GameObject obj)
    {

    }

    protected virtual void HitEnemy(GameObject obj)
    {
        HitArea hitArea = obj.GetComponent<HitArea>();
        if (!hitArea) return; 
        Enemy enemy = hitArea.enemy;
        enemy.Damage(this, hitArea);
        Instantiate(HitEffect, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    protected virtual void HitBullet(GameObject obj)
    {
        Bullet otherBullet = obj.GetComponent<Bullet>();
        if (!otherBullet.parent || otherBullet.parent == this.parent) return;
    }

    protected virtual void HitOther(GameObject obj)
    {
        //Debug.Log("Hit on " + collision.gameObject.name);
        Instantiate(breakEffect, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
