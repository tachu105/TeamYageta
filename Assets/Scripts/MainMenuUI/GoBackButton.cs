using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBackButton : MonoBehaviour
{
    GameObject main;
    GameObject setUp;
    GameObject subBack;
    GameObject instractionContents;
    GameObject instractionKey;
    GameObject instractionXBOX;
    GameObject instractionPS;
    // Start is called before the first frame update
    private void Start()
    {
        main = GameObject.Find("Main");
        setUp = GameObject.Find("SetUp");
        subBack = GameObject.Find("subBackGround");
        instractionContents = GameObject.Find("InstContents");
        instractionKey = GameObject.Find("InstKey");
        instractionXBOX = GameObject.Find("InstXbox");
        instractionPS = GameObject.Find("InstPs");
    }
    public void OnClick()
    {
        main.transform.position = Vector3.forward * Mathf.Lerp(0f, -365f, Time.deltaTime);
        setUp.transform.position = Vector3.forward * Mathf.Lerp(-365f, 0f, Time.deltaTime);
        subBack.transform.position = Vector3.forward * Mathf.Lerp(-365f, 0f, Time.deltaTime);
        instractionContents.transform.position = Vector3.forward * Mathf.Lerp(-365f, 0f, Time.deltaTime);
        instractionKey.transform.position = Vector3.forward * Mathf.Lerp(-365f, 0f, Time.deltaTime);
        instractionXBOX.transform.position = Vector3.forward * Mathf.Lerp(-365f, 0f, Time.deltaTime);
        instractionPS.transform.position = Vector3.forward * Mathf.Lerp(-365f, 0f, Time.deltaTime);
    }
}
