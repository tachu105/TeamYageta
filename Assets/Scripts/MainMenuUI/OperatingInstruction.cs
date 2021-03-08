using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OperatingInstruction : MonoBehaviour
{
    GameObject main;
    GameObject instractionContents;
    GameObject instractionKey;
    GameObject instractionXBOX;
    GameObject instractionPS;
    
    GameObject subBack;
    
    Toggle toggleKey;
    Toggle toggleXbox;
    Toggle togglePs;

    bool key;
    bool Xbox;
    bool Ps;
    
    // Start is called before the first frame update
    private void Start()
    {
        main = GameObject.Find("Main");
        instractionContents = GameObject.Find("InstContents");
        subBack = GameObject.Find("subBackGround");
        instractionKey = GameObject.Find("InstKey");
        instractionXBOX = GameObject.Find("InstXbox");
        instractionPS = GameObject.Find("InstPs");
        toggleKey = GameObject.Find("KeyBoard Toggle").GetComponent<Toggle>();
        toggleXbox = GameObject.Find("XBox Toggle").GetComponent<Toggle>();
        togglePs = GameObject.Find("PS Toggle").GetComponent<Toggle>();
       
        

    }

    private void Update()
    {
        key = toggleKey.isOn;
        Xbox = toggleXbox.isOn;
        Ps = togglePs.isOn;
    }
    public void OnClick()
    {
        main.transform.position = Vector3.forward * Mathf.Lerp(-365f, 0f, Time.deltaTime);
        instractionContents.transform.position = Vector3.forward * Mathf.Lerp(0f, -365f, Time.deltaTime);
        subBack.transform.position = Vector3.forward * Mathf.Lerp(0f, -365f, Time.deltaTime);
        if (key) instractionKey.transform.position = Vector3.forward * Mathf.Lerp(0f, -365f, Time.deltaTime);
        else if (Xbox) instractionXBOX.transform.position = Vector3.forward * Mathf.Lerp(0f, -365f, Time.deltaTime);
        else if (Ps) instractionPS.transform.position = Vector3.forward * Mathf.Lerp(0f, -365f, Time.deltaTime);

    }
}