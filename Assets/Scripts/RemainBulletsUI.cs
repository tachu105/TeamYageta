using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemainBulletsUI : MonoBehaviour
{

    GameObject Player;
    Player scripts;
    public Text remainBulletsText;


    // Use this for initialization
    void Start()
    {
        Player = GameObject.Find("Player");
        scripts = Player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        int remainBullets = scripts.remainBullets;
        int fullBullets = scripts.fullBullets;
        remainBulletsText.text = remainBullets + "/" + fullBullets;
    }
}
