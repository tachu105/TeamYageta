using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetScopeUI : MonoBehaviour
{

    public Image TargetScope;
    public Sprite spriteNomal;
    public Sprite spriteZoom;

    GameObject Player;
    private InputController inputController;


    // Use this for initialization
    void Start()
    {
        Player = GameObject.Find("Player");
        inputController = Player.GetComponent<InputController>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if (inputController.LT)
        {
            TargetScope.sprite = spriteZoom;
            TargetScope.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.width*0.7f);
            TargetScope.rectTransform.anchoredPosition = new Vector3(0f, 0f, 0f);
        }
        else
        {
            TargetScope.sprite = spriteNomal;
            TargetScope.rectTransform.sizeDelta = new Vector2(50f, 50f);
            TargetScope.rectTransform.anchoredPosition = new Vector3(10f, -8.7f, 0f);
        }
    }
}