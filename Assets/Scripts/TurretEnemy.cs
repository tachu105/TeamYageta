﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : Enemy
{
    [SerializeField] private float turnSpeed = 10; //振り向き速度
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject turretBase;
    [SerializeField] private GameObject destroyEffect;

    // Start is called before the first frame update
    void Start()
    {
        if (hp <= 0) hp = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (searchArea.IsDetected())
        {
            LookAtTarget(searchArea.currentTartget.transform.position);
        }
    }

    public override void Action()
    {

    }

    public override void Damage(Bullet bullet, HitArea area)
    {
        hp -= (int)(bullet.damage * area.damageRate);
        if (hp <= 0) Dead();
    }

    public override void Dead()
    {
        Instantiate(destroyEffect, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    private void LookAtTarget(Vector3 targetPosition)
    {
        //左右回転
        Vector3 direction = targetPosition - turretBase.transform.position;
        float angle = Vector3.SignedAngle(turretBase.transform.forward, direction, Vector3.up);
        if (Mathf.Abs(angle) > turnSpeed * Time.deltaTime)
        {
            angle = angle > 0f ? turnSpeed : -turnSpeed;
            turretBase.transform.localEulerAngles += Vector3.up * angle * Time.deltaTime;
        }
        else turretBase.transform.localEulerAngles += Vector3.up * angle;
        //上下回転
        direction = targetPosition - gun.transform.position;
        angle = Vector3.SignedAngle(gun.transform.right, direction, Vector3.right);
        Debug.Log(angle + ":" + turnSpeed * Time.deltaTime);
        if (Mathf.Abs(angle) > turnSpeed * Time.deltaTime)
        {
            angle = angle > 0f ? turnSpeed : -turnSpeed;
            Debug.Log(angle * Time.deltaTime);
            gun.transform.localEulerAngles += Vector3.forward * angle * Time.deltaTime;
        }
        else gun.transform.localEulerAngles += Vector3.forward * angle;
    }
}
