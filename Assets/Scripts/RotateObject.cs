using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private Vector3 rotateValue;

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation *= Quaternion.Euler(rotateValue * Time.deltaTime);        
    }
}
