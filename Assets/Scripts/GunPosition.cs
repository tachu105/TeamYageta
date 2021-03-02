using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPosition : MonoBehaviour
{
    GameObject Player;
    private InputController inputController;
    private Vector3 gunPos;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        inputController = Player.GetComponent<InputController>();
    }

    // Update is called once per frame
    void Update()
    {
        gunPos = inputController.LT ? new Vector3(0.313633f,0f,-0.6f) : new Vector3(0.313633f, 0f, 0f);
        this.transform.localPosition = gunPos;
    }
}
