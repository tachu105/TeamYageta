using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierObject : MonoBehaviour
{
    [SerializeField] private int hp = 500;
    [SerializeField] private GameObject DestroyEffect;

    public void Damage(float damage)
    {
        hp -= (int)damage;
        if (hp < 0) Break();
    }

    private void Break()
    {
        Instantiate(DestroyEffect, this.transform.position, Quaternion.identity);
        Destroy(this.gameObject, 3f);
    }
}
