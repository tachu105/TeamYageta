using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUpButton : MonoBehaviour
{
    GameObject main;
    GameObject setUp;
    GameObject subBack;
    // Start is called before the first frame update
    private void Start()
    {
        main = GameObject.Find("Main");
        setUp = GameObject.Find("SetUp");
        subBack = GameObject.Find("subBackGround");
    }
    public void OnClick()
    {
        main.transform.position = Vector3.forward * Mathf.Lerp(-365f, 0f, Time.deltaTime);
        setUp.transform.position = Vector3.forward * Mathf.Lerp(0f, -365f, Time.deltaTime);
        subBack.transform.position = Vector3.forward * Mathf.Lerp(0f, -365f, Time.deltaTime);
    }
}
