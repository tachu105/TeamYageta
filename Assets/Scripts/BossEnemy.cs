﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    [SerializeField] private float walkSpeed = 1; //歩行速度
    [SerializeField] private float turnSpeed = 20;
    [SerializeField] private int shootCount = 10;
    [SerializeField] private GameObject[] barrages;
    [SerializeField] private GameObject destroyEffect;
    private bool isAction = false;

    [SerializeField] private GameObject bulletPrefab;
    private const float BULLET_CHARGE_TIME = 3f;
    private const float BULLET_SIZE = 0.5f;
    private const float JUMP_CHARGE_TIME = 3f;

    Animator animator;
    AudioSource ASource;

    private bool isActive = false;
    private bool isWalking = false;
    private bool isRanning = false;
    private bool isJumping = false;
    private Vector3 playerPos;
    private Vector3 direction;

    [SerializeField] private AudioClip AudioExplosion;

    void Start()
    {
        animator = GetComponent<Animator>();
        ASource = GetComponent<AudioSource>();
        if (hp <= 0) hp = 5000;
        direction = this.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSleeping) return;

        if (searchArea.IsDetected())
        {
            if (!isActive)
            {
                isActive = true;
                animator.SetBool("isActive", true);
                Sleep(5f);
            }
            playerPos = searchArea.currentTartget.transform.position;
            if(!isAction)LookAtTarget(playerPos);
            float distance = (playerPos - transform.position).sqrMagnitude;
            if (isJumping) Move(playerPos);
            else if (distance > Mathf.Pow(10f, 2f))
            {
                Move(playerPos);
            }
            else
            {
                StopMove();
            }
            Action();
        }
        else
        {
            StopMove();
        }
    }

    public override void Action()
    {
        if (isAction || isSleeping) return;
        switch (Random.Range(0, barrages.Length))
        {
            case 0:
                Barrage barrage = Instantiate(barrages[0], transform.position + Vector3.up, Quaternion.identity).GetComponent<Barrage>();
                barrage.transform.localScale = this.transform.localScale;
                barrage.parent = this.gameObject;
                barrage.Shoot();
                Sleep(5f);
                break;
            case 1:
                break;
        }
    }

    public override void Damage(Bullet bullet, HitArea area)
    {
        hp -= (int)(bullet.damage * area.damageRate);
        if (hp <= 0) Dead();
    }

    public override void Dead()
    {
        Instantiate(destroyEffect, transform.position, transform.rotation);
        animator.SetTrigger("Hit");
        ASource.clip = AudioExplosion;
        ASource.Play();
    }

    private void StartWalk()
    {
        isRanning = false;
        animator.SetBool("isRunning", false);
        isWalking = true;
        animator.SetBool("isWalking", true);
    }

    private void StartRun()
    {
        isWalking = false;
        animator.SetBool("isWalking", false);
        isRanning = true;
        animator.SetBool("isRunning", true);
    }

    private void StopMove()
    {
        isWalking = false;
        animator.SetBool("isWalking", false);
        isRanning = false;
        animator.SetBool("isRunning", false);
    }

    private void Move(Vector3 targetPosition)
    {
        float speed;
        if (isWalking) speed = walkSpeed;
        else if (isRanning) speed = walkSpeed * 2f;
        else if (isJumping) speed = walkSpeed * 4f;
        else return;

        //左右回転
        Vector3 dir = targetPosition - this.transform.position;
        float angle = Vector3.SignedAngle(this.transform.forward, dir, Vector3.up);
        if (Mathf.Abs(angle) > turnSpeed)
        {
            angle = angle > 0f ? turnSpeed : -turnSpeed;
            this.transform.localEulerAngles += Vector3.up * angle * Time.deltaTime;
        }

        transform.position += (transform.forward * speed * Time.deltaTime);
    }

    private void Jump()
    {
        isAction = true;
        StartCoroutine(JumpCoroutine());
    }

    void EndOfRunJump()
    {
        isAction = false;
        isJumping = false;
        StopMove();
        //Barrage barrage = Instantiate(jumpBarrage, this.transform.position + Vector3.up, Quaternion.identity).GetComponent<Barrage>();
        //barrage.parent = this.gameObject;
        //barrage.Shoot();
        Sleep(5f);
    }

    private void LookAtTarget(Vector3 targetPosition)
    {
        //左右回転
        Vector3 dir = targetPosition - this.transform.position;
        float angle = Vector3.SignedAngle(this.transform.forward, dir, Vector3.up);
        if (Mathf.Abs(angle) > turnSpeed * Time.deltaTime)
        {
            angle = angle > 0f ? turnSpeed : -turnSpeed;
            this.transform.localEulerAngles += Vector3.up * angle * Time.deltaTime;
        }
        else this.transform.localEulerAngles += Vector3.up * angle;
    }

    private IEnumerator ShootBullet()
    {
        /*isAction = true;
        int count = 0;

        Bullet[] bullets = new Bullet[ports.Length];
        float[] time = new float[ports.Length];
        for (int i = 0; i < time.Length; i++) time[i] = i * (BULLET_CHARGE_TIME / ports.Length);

        while (count < shootCount)
        {
            for (int i = 0; i < time.Length; i++)
            {
                if (time[i] == 0 && count + ports.Length <= shootCount)
                {
                    bullets[i] = Instantiate(bulletPrefab, ports[i].position, Quaternion.identity).GetComponent<Bullet>();
                    bullets[i].parent = this.gameObject;
                }
                if (bullets[i])
                {
                    float scale = Mathf.Lerp(0f, BULLET_SIZE, time[i] / BULLET_CHARGE_TIME);
                    bullets[i].transform.localScale = Vector3.one * scale;
                    bullets[i].transform.position = ports[i].position;
                }
                time[i] += Time.deltaTime;
                if (time[i] > BULLET_CHARGE_TIME)
                {
                    if (bullets[i])
                    {
                        bullets[i].dir = ports[i].forward;
                        bullets[i] = null;
                        count++;
                    }
                    time[i] = 0f;
                }
            }
            yield return new WaitForEndOfFrame();
        }
        isAction = false;
        StopMove();
        Sleep(3f);*/
        yield return null;
    }

    private IEnumerator JumpCoroutine()
    {
        /*float time = 0f;
        Vector3 rot = body.transform.localEulerAngles;
        while (time < JUMP_CHARGE_TIME)
        {
            float spinVal = Mathf.Lerp(0f, 360f * 3f, time / JUMP_CHARGE_TIME);
            body.transform.localEulerAngles = rot + (Vector3.right * spinVal);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        isJumping = true;
        animator.SetTrigger("Jump");*/
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(1f);
        EndOfRunJump();
    }
}
