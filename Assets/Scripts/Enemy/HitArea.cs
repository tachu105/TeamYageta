using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HitArea : MonoBehaviour
{
    public float damageRate = 1.0f;
    public Enemy enemy;
    [SerializeField] private string[] hitObjectTag;
}
