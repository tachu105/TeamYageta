using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    private int maxHp;
    private int currentHp;
    int damage=1;
    public Slider HPBar;


    // Start is called before the first frame update
    void Start()
    {
        maxHp = GameObject.Find("Player").GetComponent<Player>().Hp * 100;
        HPBar.maxValue = maxHp;
        currentHp = maxHp;
        HPBar.value = currentHp;
    }

    // Update is called once per frame
    void Update()
    {
        currentHp -= damage;
        HPBar.value = currentHp;
    }
}
