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
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        maxHp = player.Hp;
        HPBar.maxValue = maxHp;
        HPBar.value = player.Hp;
    }

    // Update is called once per frame
    void Update()
    {
        HPBar.value = Player.instance.Hp;
    }
}
