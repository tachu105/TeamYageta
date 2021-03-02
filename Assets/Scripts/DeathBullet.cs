using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBullet : Bullet
{
    [SerializeField] Barrage childBarrage;
    [SerializeField] Barrage childBarrageSub;
    private void Start()
    {
        StartCoroutine(ChildCoroutine());
    }

    protected override void HitOther(GameObject obj)
    {
        Instantiate(breakEffect, new Vector3(transform.position.x, 0f, transform.position.z), Quaternion.identity);
        Barrage barrage = Instantiate(childBarrage, new Vector3(transform.position.x, 1f, transform.position.z), Quaternion.identity).GetComponent<Barrage>();
        barrage.Shoot();
        Destroy(this.gameObject, 3f);
    }

    private IEnumerator ChildCoroutine()
    {
        while (true)
        {
            float time = 0f;
            while (time < 1f)
            {
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            Barrage barrage = Instantiate(childBarrageSub, transform.position, Quaternion.identity).GetComponent<Barrage>();
            barrage.Shoot();
        }
    }
}