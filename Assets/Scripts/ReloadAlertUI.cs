using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadAlertUI : MonoBehaviour
{
    GameObject Player;
    Player scripts;
    public Text reloadAlertUIText;


    // Use this for initialization
    void Start()
    {
        Player = GameObject.Find("Player");
        scripts = Player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isReloading = scripts.isReloading;
        if(isReloading)
        {
            reloadAlertUIText.text = "RELOADING";
        }
        if (!isReloading)
        {
            reloadAlertUIText.text = "";
        }
    }
}
