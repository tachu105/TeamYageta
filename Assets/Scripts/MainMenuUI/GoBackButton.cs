using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBackButton : MonoBehaviour
{
    GameObject main;
    GameObject setUp;
    // Start is called before the first frame update
    private void Start()
    {
        main = GameObject.Find("GameObject");
        setUp = GameObject.Find("GameObject");
    }
    public void OnClick()
    {
        main.transform.position = Vector3.forward * Mathf.Lerp(0f, -365f, Time.deltaTime);
        setUp.transform.position = Vector3.forward * Mathf.Lerp(-365f, 0f, Time.deltaTime);
    }
}
