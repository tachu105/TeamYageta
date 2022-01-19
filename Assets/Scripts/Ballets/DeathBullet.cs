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
        Barrage barrage = Instantiate(childBarrage, new Vector3(transform.position.x,Camera.main.transform.position.y, transform.position.z), Quaternion.identity).GetComponent<Barrage>();
        barrage.Shoot();
        //障害物があるか
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (Camera.main.transform.position - transform.position).normalized, out hit, Mathf.Infinity))
        {
            if (hit.collider.tag == "Player")
            {

                Player.instance.Hp -= (int)damage;
            }
        }
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