using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPortal : Bullet
{
    private ShootBulletObject shootObject;

    void Start()
    {
        shootObject = GetComponent<ShootBulletObject>();
    }

    public override void Shoot(Vector3 dir)
    {
        base.Shoot(dir);
        shootObject.StartShoot();
    }
}
