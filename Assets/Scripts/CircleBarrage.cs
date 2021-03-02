using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleBarrage : Barrage
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] int bulletCount = 8;
    [SerializeField] float distance = 3f;
    [SerializeField] float spinSpeed = 0f;

    private void Update()
    {
        this.transform.eulerAngles += Vector3.up * spinSpeed * Time.deltaTime;
        if (this.transform.childCount == 0) Destroy(this.gameObject);
    }
    protected override void SetUp()
    {
        for(int i = 0; i < bulletCount; i++)
        {
            float angle = (float)i / (float)bulletCount * 360f * Mathf.Deg2Rad;
            float posX = Mathf.Sin(angle);
            float posZ = Mathf.Cos(angle);
            GameObject bullet = Instantiate(bulletPrefab, this.transform);
            bullet.transform.localPosition = new Vector3(posX, 0f, posZ) * distance;
            bullet.transform.forward = (bullet.transform.position - this.transform.position).normalized;
        }
    }
}
