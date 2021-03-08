using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : Enemy
{
    [SerializeField] private float turnSpeed = 10; //振り向き速度
    [SerializeField] private Transform port;
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject turretBase;
    [SerializeField] private GameObject destroyEffect;
    private bool isAction = false;

    [SerializeField] private GameObject bulletPrefab;
    private float totalAngle = 360f;
    private const float ATTACK_ANGLE = 90f;
    private const float BULLET_CHARGE_TIME = 3f;
    private const float BULLET_SIZE = 0.5f;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        if (hp <= 0) hp = 100;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = GameManager.instance.SEVolume;

        if (searchArea.IsDetected())
        {
            LookAtTarget(Camera.main.transform.position);
            Action();
        }
    }

    public override void Action()
    {
        if (isAction || isSleeping) return;
        if(totalAngle < ATTACK_ANGLE)
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
        Destroy(this.gameObject);
    }

    private void LookAtTarget(Vector3 targetPosition)
    {
        totalAngle = 0f;
        //左右回転
        Vector3 direction = targetPosition - turretBase.transform.position;
        float angle = Vector3.SignedAngle(turretBase.transform.forward, direction, Vector3.up);
        totalAngle += Mathf.Abs(angle);
        if (Mathf.Abs(angle) > turnSpeed * Time.deltaTime)
        {
            angle = angle > 0f ? turnSpeed : -turnSpeed;
            turretBase.transform.localEulerAngles += Vector3.up * angle * Time.deltaTime;
        }
        else turretBase.transform.localEulerAngles += Vector3.up * angle;
        //上下回転
        direction = targetPosition - gun.transform.position;
        angle = Vector3.SignedAngle(gun.transform.right, direction, gun.transform.forward);
        totalAngle += Mathf.Abs(angle);
        if (Mathf.Abs(angle) > 90f) return;
        //Debug.Log("To : " + angle);
        if (Mathf.Abs(angle) > turnSpeed * Time.deltaTime)
        {
            angle = angle > 0f ? turnSpeed : -turnSpeed;
            gun.transform.localEulerAngles += Vector3.forward * angle * Time.deltaTime;
        }
        else gun.transform.localEulerAngles += Vector3.forward * angle;
    }

    private IEnumerator ShootBullet()
    {
        isAction = true;
        Bullet bullet = Instantiate(bulletPrefab, port.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.parent = this.gameObject;
        float time = 0f;
        while(time < BULLET_CHARGE_TIME)
        {
            float scale = Mathf.Lerp(0f, BULLET_SIZE, time / BULLET_CHARGE_TIME);
            bullet.transform.localScale = Vector3.one * scale;
            bullet.transform.position = port.position;
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        bullet.dir = port.forward;
        isAction = false;
        Sleep(5f);
    }

}
