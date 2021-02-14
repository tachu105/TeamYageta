using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float rechargeTime = 1f;     //連射間隔

    bool isFirePer = true;

   
   


    public Vector3 cameraDir = Vector3.zero;
    public Vector3 playerDir = Vector3.zero;
    public Vector2 angle = Vector2.zero;

    
    public GameObject Bullet;       // bullet prefab


    public Transform Muzzle;        // 弾丸発射点


    //ジャンプに使用するもの
    private CharacterController controller;
    private Vector3 moveDirection;
    public float jumpPower = 15f;
    public float walkSpeed = 6f;
    public float fallSpeed = 0.5f;
    public float runSpeed = 10f;




    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //移動//

       
        moveDirection.x = Input.GetAxis("Horizontal");
        moveDirection.y = 0.0f;
        moveDirection.z = Input.GetAxis("Vertical");


        if (Input.GetKey(KeyCode.LeftControl))
        {
            moveDirection *= runSpeed;
        }
            moveDirection *= walkSpeed;
        

        if (Input.GetKey(KeyCode.Space))
        {
            moveDirection.y += jumpPower;
        }

        moveDirection.y += Physics.gravity.y*fallSpeed; //重力計算
        controller.Move(moveDirection * Time.deltaTime); //Playerを動かす処理

        Debug.Log(walkSpeed);





        //向き//

        Vector2 angle = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        cameraDir += new Vector3(-angle.y, angle.x, 0);
        Camera.main.transform.rotation=Quaternion.Euler(cameraDir);

        playerDir += new Vector3(-angle.y, angle.x, 0);
        this.transform.rotation = Quaternion.Euler(playerDir);



        //射撃//

        if (Input.GetMouseButton(0)&& isFirePer)
        {
            GameObject bullets = Instantiate(Bullet) as GameObject;
            // 弾丸の位置を調整
            bullets.transform.position = Muzzle.position;
            
            isFirePer = false;
            StartCoroutine(ReCharge());
        }
        

    }



    //連射間隔調整関数//
    float time=0f;
    IEnumerator ReCharge()
    {
        while (true)
        {
            time += Time.deltaTime;
            if (time > rechargeTime)
            {
                time = 0f;
                isFirePer = true;
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }




    //moveX//
    void moveX(float dirX, bool isRun)
    {
        moveDirection.x = dirX;
        if (isRun)
        {
            moveDirection *= runSpeed;      //早く動く
        }
        moveDirection *= walkSpeed;     //通常速度
        controller.Move(moveDirection * Time.deltaTime); //Playerを動かす処理
    }


    //moveZ//
    void moveZ(float dirY, bool isRun)
    {
        moveDirection.z = dirY;
        if (isRun)
        {
            moveDirection *= runSpeed;      //速く動く
        }
        moveDirection *= walkSpeed;     //通常速度

        controller.Move(moveDirection * Time.deltaTime); //Playerを動かす処理
    }


    //jump//
    void jump()
    {
        moveDirection.y = 0.0f;
        moveDirection.y += jumpPower;
        moveDirection.y += Physics.gravity.y * fallSpeed; //重力計算
        controller.Move(moveDirection * Time.deltaTime); //Playerを動かす処理
    }


    //angleX//
    void angleX(float angX)
    {
        angle.x = angX;

        cameraDir += new Vector3(-angle.y, angle.x, 0);
        Camera.main.transform.rotation = Quaternion.Euler(cameraDir);       //カメラ向き

        playerDir += new Vector3(-angle.y, angle.x, 0);
        this.transform.rotation = Quaternion.Euler(playerDir);      //プレイヤー向き
    }


    //angleY//
    void angleY(float angY)
    {
        angle.y = angY;

        cameraDir += new Vector3(-angle.y, angle.x, 0);
        Camera.main.transform.rotation = Quaternion.Euler(cameraDir);       //カメラ向き

        playerDir += new Vector3(-angle.y, angle.x, 0);
        this.transform.rotation = Quaternion.Euler(playerDir);      //プレイヤー向き
    }


    //射撃//
    void pressRT(bool isPreRT)
    {
        if (isPreRT && isFirePer)
        {
            GameObject bullets = Instantiate(Bullet) as GameObject;
            bullets.transform.position = Muzzle.position;       // 弾丸の出現位置を調整

            isFirePer = false;
            StartCoroutine(ReCharge());
        }
        
    }
}
