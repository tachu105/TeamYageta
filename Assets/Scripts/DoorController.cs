using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] Enemy[] targetEnemies;

    void Update()
    {
        if (isAllClear())
        {
            GetComponent<Animator>().SetBool("Open", true);
        }
    }

    bool isAllClear()
    {
        for(int i = 0; i < targetEnemies.Length; i++)
        {
            if (targetEnemies[i].GetHP() > 0f) return false;
        }
        return true;
    }
}
