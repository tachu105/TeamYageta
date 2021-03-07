using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUpButton : MonoBehaviour
{
    GameObject main;
    // Start is called before the first frame update
    private void Start()
    {
        main = GameObject.Find("GameObject");
    }
    public void OnClick()
    {
        main.transform.position = Vector3.forward * Mathf.Lerp(-365f, 0f, Time.deltaTime);
    }
}
