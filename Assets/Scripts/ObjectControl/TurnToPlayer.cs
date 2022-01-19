using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnToPlayer : MonoBehaviour
{
    [SerializeField] private float turnSpeed = 60f;

    // Update is called once per frame
    void Update()
    {
        Vector3 newDir = Vector3.RotateTowards(this.transform.forward, (Camera.main.transform.position - this.transform.position).normalized, turnSpeed * Mathf.Deg2Rad * Time.deltaTime, 0f);
        this.transform.forward = newDir;
    }
}
