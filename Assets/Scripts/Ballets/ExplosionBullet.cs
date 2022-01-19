using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBullet : Bullet
{
    [SerializeField] private float explosionSize = 3f;
    [SerializeField] private float explosionTime = 1f;

    private FallObject fallObject;

    private void Start()
    {
        fallObject = GetComponent<FallObject>();
    }
    protected override void Update()
    {
        base.Update();
    }

    public override void Shoot(Vector3 dir)
    {
        base.Shoot(dir);
        if (fallObject) fallObject.StartFall();
    }

    protected override void HitOther(GameObject obj)
    {
        Stop();
        if (fallObject) fallObject.StopFall();
        StartCoroutine(ExplosionCoroutine());
    }

    private IEnumerator ExplosionCoroutine()
    {
        float time = 0f;
        float baseSize = this.transform.localScale.x;
        while(time < explosionTime)
        {
            this.transform.localScale = Vector3.one * Mathf.Lerp(baseSize, baseSize * explosionSize, time / explosionTime);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Instantiate(breakEffect, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
