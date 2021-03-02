using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBarrage : Barrage
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] int bulletCount = 8;
    [SerializeField] float distance = 3f;
    [SerializeField] Vector3 targetShift = Vector3.zero;
    protected override void SetUp()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            Vector3 dir = new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), Random.Range(-1f, 1f));
            GameObject bullet = Instantiate(bulletPrefab, this.transform);
            bullet.transform.localPosition = dir * Random.Range(distance, 2f * distance);
            Vector3 shift = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 2f + targetShift;
            bullet.transform.forward = (Camera.main.transform.position + shift - bullet.transform.position).normalized;
        }
    }
}
