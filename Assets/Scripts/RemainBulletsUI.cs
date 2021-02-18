using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemainBulletsUI : MonoBehaviour
{
    Player player;
    public Text remainBulletsText;


    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        int remainBullets = player.remainBullets;
        int fullBullets = player.fullBullets;
        remainBulletsText.text = remainBullets + "/" + fullBullets;
    }
}
