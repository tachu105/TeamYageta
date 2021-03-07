using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float BgmVolume = 0.5f;
    public float SEVolume = 0.5f;

    public static GameManager instance;

    void Awake()
    {
        if (instance) Destroy(this.gameObject);
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
