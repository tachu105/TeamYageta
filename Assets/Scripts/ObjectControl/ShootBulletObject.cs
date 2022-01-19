using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBulletObject : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootInterval = 0.5f; //発射間隔
    [SerializeField] private int shootLimit = 20;       //発射限界
    [SerializeField] private GameObject disapperEffect;

    private bool isShooting = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartShoot()
    {
        if (isShooting) return;
        isShooting = true;
        StartCoroutine(ShootCoroutine());
    }

    public void StopShoot()
    {
        if (isShooting) return;
        isShooting = false;
        StopAllCoroutines();
    }

    private IEnumerator ShootCoroutine()
    {
        if (shootLimit == 0)
        {
            while (true)
            {
                Bullet bullet = Instantiate(bulletPrefab, this.transform.position, Quaternion.identity).GetComponent<Bullet>();
                bullet.transform.forward = this.transform.forward;
                bullet.Shoot(this.transform.forward);
                yield return new WaitForSeconds(shootInterval);
            }
        }
        else
        {
            for (int i = 0; i < shootLimit; i++)
            {
                Bullet bullet = Instantiate(bulletPrefab, this.transform.position, Quaternion.identity).GetComponent<Bullet>();
                bullet.transform.forward = this.transform.forward;
                bullet.Shoot(this.transform.forward);
                yield return new WaitForSeconds(shootInterval);
            }
            if (disapperEffect) Instantiate(disapperEffect, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
