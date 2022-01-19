﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class　Bullet : MonoBehaviour
{
    // 弾丸の速度
    public float damage = 10f;
    public float speed = 10f;
    [SerializeField] protected float range = 20f;
    [SerializeField] protected string[] throughTags;
    [SerializeField] protected GameObject HitEffect;
    [SerializeField] protected GameObject breakEffect;
    [SerializeField] protected AudioClip shootSound;
    [SerializeField] protected AudioClip hitSound;
    [SerializeField] protected AudioClip destroySound;
    [SerializeField] private GameObject damageText;
    private bool isShoot = false;
    private float totalLength = 0f;
    private AudioSource audioSource;
    public GameObject parent;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume *= GameManager.instance.SEVolume;
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        if (!isShoot) return;
        float moveVal = speed * Time.deltaTime;
        this.transform.position += (this.transform.forward * moveVal);
        totalLength += moveVal;
        if (totalLength > range) Destroy(this.gameObject);
    }

    public void Shoot()
    {
        Shoot(this.transform.forward);
    }

    public virtual void Shoot(Vector3 dir)
    {
        if (isShoot) return;
        isShoot = true;
            
        if (!audioSource) audioSource = GetComponent<AudioSource>();
        if (shootSound) audioSource.PlayOneShot(shootSound);

        this.transform.forward = dir;
    }

    public virtual void Stop()
    {
        if (!isShoot) return;
        isShoot = false;
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
                if (hitSound) audioSource.PlayOneShot(hitSound);
                HitEnemy(collider.gameObject);
                break;
            case "Bullet":
                HitBullet(collider.gameObject);
                break;
            case "Barrier":
                HitBarrier(collider.gameObject);
                break;
            default:
                if (destroySound) audioSource.PlayOneShot(destroySound);
                HitOther(collider.gameObject);
                break;
        }
    }

    protected virtual void HitPlayer(GameObject obj)
    {
        Player.instance.Hp -= (int)this.damage;
        if(HitEffect)Instantiate(HitEffect, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    protected virtual void HitEnemy(GameObject obj)
    {
        HitArea hitArea = obj.GetComponent<HitArea>();
        if (!hitArea) return; 
        Enemy enemy = hitArea.enemy;
        if (enemy.GetHP() < 0f) return;
        enemy.Damage(this, hitArea);
        DamageText text = Instantiate(damageText, this.transform.position - Camera.main.transform.forward , Quaternion.identity).GetComponent<DamageText>();
        text.ShowDamage(this.damage, hitArea);
        if(HitEffect)Instantiate(HitEffect, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    protected virtual void HitBullet(GameObject obj)
    {
    }

    protected virtual void HitBarrier(GameObject obj)
    {
        BarrierObject barrier = obj.GetComponent<BarrierObject>();
        barrier.Damage(this.damage);
        if (HitEffect) Instantiate(HitEffect, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    protected virtual void HitOther(GameObject obj)
    {
        if(breakEffect) Instantiate(breakEffect, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}