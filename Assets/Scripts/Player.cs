using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    Vector3 cameraDir = Vector3.zero;
    Vector3 playerDir = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //移動//

        //左に移動
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(-0.1f, 0.0f, 0.0f);
        }
        // 右に移動
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(0.1f, 0.0f, 0.0f);
        }
        // 前に移動
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(0.0f, 0.0f, -0.1f);
        }
        // 後ろに移動
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(0.0f, 0.0f, 0.1f);
        }
        //上に移動
        if (Input.GetKey(KeyCode.Space))
        {
            this.transform.Translate(0.0f, 0.1f, 0.0f);
        }
        //下に移動
        if (Input.GetKey(KeyCode.LeftShift))
        {
            this.transform.Translate(0.0f, -0.1f, 0.0f);
        }





        //向き//
        
        Vector2 angle = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Debug.Log(angle);

        cameraDir += new Vector3(-angle.y, angle.x, 0);
        Camera.main.transform.rotation=Quaternion.Euler(cameraDir);

        playerDir += new Vector3(-angle.y, angle.x, 0);
        this.transform.rotation = Quaternion.Euler(playerDir);
        
        //上を向く
        /*if (Input.mousePosition.y)
        {
            this.transform.Rotate(angle.x);
            this.transform.rotation = Quaternion.Euler(angle);
        }*/
        
    }
}
