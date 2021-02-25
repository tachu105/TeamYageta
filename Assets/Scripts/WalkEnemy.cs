using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkEnemy : Enemy
{
    [SerializeField] private float walkSpeed = 1; //歩行速度
    [SerializeField] private float turnSpeed = 20;
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject neck;
    [SerializeField] private GameObject destroyEffect;
    private bool isAction = false;

    [SerializeField] private GameObject bulletPrefab;
    private float totalAngle = 360f;
    private const float ATTACK_ANGLE = 30f;
    private const float BULLET_CHARGE_TIME = 3f;
    private const float BULLET_SIZE = 0.5f;

    Animator animator;
    AudioSource ASource;

    private bool isWalking = false;
    private bool isRanning = false;
    private Vector3 playerPos;
    private Vector3 direction;

    [SerializeField] private AudioClip AudioExplosion;

    void Start()
    {
        animator = GetComponent<Animator>();
        ASource = GetComponent<AudioSource>();
        if (hp <= 0) hp = 100;
        direction = this.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (searchArea.IsDetected())
        {
            playerPos = searchArea.currentTartget.transform.position;
            LookAtTarget(playerPos);
            float distance = (playerPos - transform.position).sqrMagnitude;
            if (distance > Mathf.Pow(10f, 2f))
            {
                if (totalAngle < ATTACK_ANGLE) StartRun();
                else StartWalk();
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
        if (totalAngle < ATTACK_ANGLE)
        {
            StartCoroutine(ShootBullet());
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
        animator.speed = 0.5f;
    }

    private void StartRun()
    {
        isWalking = false;
        animator.SetBool("isWalking", false);
        isRanning = true;
        animator.SetBool("isRunning", true);
        animator.speed = 0.5f;
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
        animator.SetTrigger("Jump");
    }

    private void LookAtTarget(Vector3 targetPosition)
    {
        totalAngle = 0f;
        //左右回転
        Vector3 dir = targetPosition - body.transform.position;
        float angle = Vector3.SignedAngle(body.transform.forward, dir, -body.transform.right);
        totalAngle += Mathf.Abs(angle);
        if (Mathf.Abs(angle) > turnSpeed * Time.deltaTime)
        {
            angle = angle > 0f ? turnSpeed : -turnSpeed;
            body.transform.localEulerAngles += Vector3.right * angle * Time.deltaTime;
        }
        else body.transform.localEulerAngles += Vector3.right * angle;
        //上下回転
        dir = targetPosition - neck.transform.position;
        angle = Vector3.SignedAngle(neck.transform.forward, dir, -neck.transform.up);
        totalAngle += Mathf.Abs(angle);
        if (Mathf.Abs(angle) > 90f) return;
        if (Mathf.Abs(angle) > turnSpeed * Time.deltaTime)
        {
            angle = angle > 0f ? turnSpeed : -turnSpeed;
            neck.transform.localEulerAngles += Vector3.up * angle * Time.deltaTime;
        }
        else neck.transform.localEulerAngles += Vector3.up * angle;
    }

    private IEnumerator ShootBullet()
    {
        /*isAction = true;
        Bullet bullet = Instantiate(bulletPrefab, port.position, Quaternion.identity).GetComponent<Bullet>();
        float time = 0f;
        while (time < BULLET_CHARGE_TIME)
        {
            float scale = Mathf.Lerp(0f, BULLET_SIZE, time / BULLET_CHARGE_TIME);
            bullet.transform.localScale = Vector3.one * scale;
            bullet.transform.position = port.position;
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        bullet.dir = port.forward;
        isAction = false;
        Sleep(5f);*/
        yield return null;
    }
}
