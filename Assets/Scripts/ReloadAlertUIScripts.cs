using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadAlertUIScripts : MonoBehaviour
{
    GameObject Player;
    PlayerScripts scripts;
    public Text reloadAlertUIText;


    // Use this for initialization
    void Start()
    {
        Player = GameObject.Find("Player");
        scripts = Player.GetComponent<PlayerScripts>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isRemainBullets = scripts.isRemainBullets;
        if(!isRemainBullets)
        {
            reloadAlertUIText.text = "RELOADING";
        }
        if (isRemainBullets)
        {
            reloadAlertUIText.text = "";
        }


    }
}
