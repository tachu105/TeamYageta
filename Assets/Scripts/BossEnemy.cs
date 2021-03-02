using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    [SerializeField] private float walkSpeed = 1; //歩行速度
    [SerializeField] private float turnSpeed = 10;
    [SerializeField] private int shootCount = 10;
    [SerializeField] private GameObject[] barrages;
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject deathBlow;
    [SerializeField] private GameObject destroyEffect;
    private bool isAction = false;
    private float originLife;

    [SerializeField] private GameObject bulletPrefab;
    private const float BULLET_CHARGE_TIME = 3f;
    private const float BULLET_SIZE = 0.5f;
    private const float JUMP_CHARGE_TIME = 3f;
    private const float SHIELD_TIME = 6f;
    private const float DEATH_BLOW_CHARGE_TIME = 15f;

    Animator animator;
    AudioSource ASource;
    
    private bool isActive = false;
    private bool isTurn = false;
    private bool isAngry = false;
    private Vector3 playerPos;
    private Vector3 direction;

    [SerializeField] private AudioClip AudioExplosion;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("Fight_Idle_1");
        ASource = GetComponent<AudioSource>();
        if (hp <= 0) hp = 5000;
        direction = this.transform.forward;
        originLife = hp;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAngry && hp < originLife * 0.3f)
        {
            isAngry = true;
            StartCoroutine(DeathBlowCoroutine());
        }
        if (isSleeping) return;

        if (searchArea.IsDetected())
        {
            if (!isActive)
            {
                isActive = true;
                animator.SetTrigger("Intimidate_1");
                Sleep(3f);
            }
            if (!isAction)
            {
                playerPos = searchArea.currentTartget.transform.position;
                LookAtTarget(playerPos);
            }
            if(!isTurn) Action();
        }
        else
        {
            animator.SetTrigger("Eat_Cycle_1");
        }
    }

    public override void Action()
    {
        if (isAction || isSleeping) return;
        isAction = true;
        for (int i = 0; i < (isAngry? 2f : 1f ); i++)
        {
            ShootBarrage();
        }
        isAction = false;
    }

    private void ShootBarrage()
    {
        int attackNumber = Random.Range(0, barrages.Length + (isAngry ? 0 : 1));
        Barrage barrage;
        switch (attackNumber)
        {
            case 0:
                barrage = Instantiate(barrages[0], transform.position + Vector3.up * 1.5f, Quaternion.identity).GetComponent<Barrage>();
                barrage.transform.localScale = this.transform.localScale;
                barrage.parent = this.gameObject;
                animator.SetTrigger("Attack_1");
                barrage.Shoot();
                Sleep(isAngry ? 1f : 5f);
                break;
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                barrage = Instantiate(barrages[attackNumber], transform.position, Quaternion.identity).GetComponent<Barrage>();
                barrage.parent = this.gameObject;
                barrage.ShootRandom();
                animator.SetTrigger("Attack_5");
                Sleep(isAngry ? 1f : 5f);
                break;
            case 6:
                StartCoroutine(SheildCoroutine());
                Sleep(1f);
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

    private void LookAtTarget(Vector3 targetPosition)
    {
        //左右回転
        Vector3 dir = targetPosition - this.transform.position;
        float angle = Vector3.SignedAngle(this.transform.forward, dir, Vector3.up);
        if (Mathf.Abs(angle) > 45f)
        {
            if (!isTurn)
            {
                animator.SetTrigger("Walk_Cycle_2");
                isTurn = true;
            }
            angle = angle > 0f ? turnSpeed : -turnSpeed;
            this.transform.localEulerAngles += Vector3.up * angle * Time.deltaTime;
        }
        else
        {
            if (isTurn)
            {
                animator.SetTrigger("Fight_Idle_1");
                isTurn = false;
            }
        }
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

    private IEnumerator DeathBlowCoroutine()
    {
        isAction = true;
        float time = 0f;
        float beforeTime = 0f;
        float attackInterval = 0.75f;
        GameObject obj = Instantiate(deathBlow, this.transform.position, Quaternion.identity);
        float baseScale = obj.transform.localScale.x;
        while(time < DEATH_BLOW_CHARGE_TIME)
        {
            float scale = Mathf.Lerp(0f, baseScale, time / DEATH_BLOW_CHARGE_TIME);
            obj.transform.localScale = Vector3.one* scale;
            time += Time.deltaTime;
            if(time > beforeTime + attackInterval)
            {
                ShootBarrage();
                beforeTime = time;
            }
            yield return new WaitForEndOfFrame();
        }
        obj.GetComponent<Bullet>().dir = Vector3.down;
        isAction = false;
    }

    private IEnumerator SheildCoroutine()
    {
        float time = 0;
        GameObject obj = Instantiate(shield, this.transform.position, Quaternion.identity, this.transform);
        float scale = obj.transform.localScale.x;
        while(time < 1f)
        {
            obj.transform.localScale = Vector3.one * Mathf.Lerp(0f, scale, time);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        time = 0f;
        while(time < SHIELD_TIME)
        {
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        time = 0f;
        while (time < 1f)
        {
            obj.transform.localScale = Vector3.one * Mathf.Lerp(scale, 0f, time);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(obj);
    }
}
