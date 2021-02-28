using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackBullet : Bullet
{
    [SerializeField] private float turnSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Vector3 newDir = Vector3.RotateTowards(this.transform.forward, (Camera.main.transform.position - this.transform.position).normalized, turnSpeed * Time.deltaTime, 0f);
        this.transform.forward = newDir;
    }
}
