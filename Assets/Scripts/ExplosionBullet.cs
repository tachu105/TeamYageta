using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBullet : Bullet
{
    [SerializeField] private float explosionSize = 3f;
    [SerializeField] private float explosionTime = 1f;
    protected override void Update()
    {
        if (dir != Vector3.zero) this.transform.forward = Vector3.RotateTowards(this.transform.forward, Vector3.down, 0.1f * Time.deltaTime, 0f);
        base.Update();
    }

    protected override void HitOther(GameObject obj)
    {
        this.dir = Vector3.zero;
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
        Destroy(this.gameObject);
    }
}
