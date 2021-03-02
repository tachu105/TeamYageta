using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrage : MonoBehaviour
{
    protected List<Bullet> bullets = new List<Bullet>();
    protected float[] times;
    protected Vector3[] defaultScale;
    [SerializeField] private float chargeTime = 3f;

    public GameObject parent;

    void Awake()
    {
        SetUp();
        GetComponentsInChildren<Bullet>(bullets);
        defaultScale = new Vector3[bullets.Count];
        for (int i = 0; i < bullets.Count; i++)
        {
            defaultScale[i] = bullets[i].transform.localScale;
            bullets[i].transform.localScale = Vector3.zero;
            bullets[i].parent = parent;
        }
        times = new float[bullets.Count];
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.childCount == 0) Destroy(this.gameObject);
    }

    protected virtual void SetUp()
    {

    }

    public void Shoot()
    {
        for (int i = 0; i < times.Length; i++) times[i] = 0f;
        Debug.Log("Start" + bullets.Count);
        StartCoroutine(ShootCoroutine());
    }

    public void ShootRandom()
    {
        for (int i = 0; i < times.Length; i++) times[i] = Random.Range(-chargeTime, 0f);
        StartCoroutine(ShootCoroutine());
    }

    private IEnumerator ShootCoroutine()
    {
        int count = 0;
        while(count < bullets.Count)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                if (!bullets[i] || times[i] > chargeTime) continue;
                if (times[i] > 0f) bullets[i].transform.localScale = defaultScale[i] * (times[i] / chargeTime);
                times[i] += Time.deltaTime;
                if(times[i] > chargeTime)
                {
                    bullets[i].dir = Vector3.forward;
                    count++;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
}