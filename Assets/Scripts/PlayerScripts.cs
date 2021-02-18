using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScripts : MonoBehaviour
{
    //射撃に使うもの//
    bool isFirePer = true;
    public bool isRemainBullets = true;
    public int remainBullets;       //残弾数
    public GameObject Bullet;       // bullet prefab
    public Transform Muzzle;        // 弾丸発射点
    public float rechargeTime = 0.1f;     //連射間隔
    public float reloadingTime = 3f;        //リロード所要時間
    public int fullBullets = 100;       //弾数

   
    //アングル操作に使用するもの//
    Vector3 cameraDir = Vector3.zero;        //カメラ向き
    Vector3 playerDir = Vector3.zero;        //プレイヤー向き
    Vector2 angle = Vector2.zero;        //コントローラー情報格納変数
    public float xAngUpLimit = -75f;        //上振り向き限界角
    public float xAngDownLimit = 65f;       //下振り向き限界角
    public float xAngleSpeed = 1.0f;        //縦振り向き感度
    public float yAngleSpeed = 1.0f;        //横振り向き感度
    int cameraReverse = -1;                 //上下カメラ操作反転


    //移動に使用するもの//
    private CharacterController controller;
    private Vector3 moveDirection;      //移動方向変数
    public float jumpPower = 15f;       //ジャンプ力（上昇速度）
    public float fallSpeed = 0.5f;      //落下速度
    public float walkSpeed = 4f;        //歩き速度
    public float runSpeed = 10f;         //走り速度
    public float slideSpeed = 20f;      //スライディング速度
    public float slideTime = 0.3f;      //スライディング時間
    float timerSlide;
    bool isSlide;




    // Start is called before the first frame update
    void Start()
    {
        remainBullets = fullBullets;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //移動//

        Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
        Vector3 right = Camera.main.transform.TransformDirection(Vector3.right);

        moveDirection = Input.GetAxis("Horizontal") * right + Input.GetAxis("Vertical") * forward;
        moveDirection.y = 0.0f;
        


        if (Input.GetKey(KeyCode.LeftShift))      //走る
        {
            moveDirection *= runSpeed;
        }
        else
        {
            moveDirection *= walkSpeed;
        }
            
        

        if (Input.GetKey(KeyCode.Space))        //ジャンプ
        {
            moveDirection.y += jumpPower;
        }



        if (Input.GetKeyDown(KeyCode.LeftControl) && !isSlide)      //スライディング
        { //タイマー開始
            
            isSlide = true;
        }
        if (isSlide) timerSlide += Time.deltaTime;       //タイマー加算
        if (isSlide && timerSlide > slideTime)
        { //タイマー終了
            isSlide = false;
            timerSlide = 0.0f;
        }
        //if (controller.isGrounded)
        //{
        if (isSlide)
        { //タイマーチェック
            moveDirection = Input.GetAxis("Horizontal") * right + Input.GetAxis("Vertical") * forward;
            moveDirection.x *= slideSpeed;
            moveDirection.z *= slideSpeed;
        }
        //}
        
        moveDirection.y += Physics.gravity.y*fallSpeed; //重力計算
        controller.Move(moveDirection * Time.deltaTime); //Playerを動かす処理
        







        //向き//

        if (Input.GetKeyDown(KeyCode.P)) cameraReverse *= -1;       //カメラ上下反転

        Vector2 angle = new Vector2(Input.GetAxis("Mouse X")*yAngleSpeed, Input.GetAxis("Mouse Y")*xAngleSpeed);

        playerDir += new Vector3(cameraReverse*angle.y, angle.x, 0);

        //アングル制限//
        if (xAngUpLimit >= playerDir.x) playerDir.x = xAngUpLimit;
        if (playerDir.x >= xAngDownLimit) playerDir.x = xAngDownLimit;

        this.transform.rotation = Quaternion.Euler(playerDir);



        //射撃//

        if (Input.GetMouseButton(0)&& isFirePer && isRemainBullets)     //射撃
        {
            GameObject bullets = Instantiate(Bullet) as GameObject;
            // 弾丸の位置を調整
            bullets.transform.position = Muzzle.position;
            
            remainBullets--;
            
            isFirePer = false;
            StartCoroutine(ReCharge());

            if (remainBullets == 0)
            {
                isRemainBullets = false;
                StartCoroutine(Reload());
            }
        }


        if (Input.GetKeyDown(KeyCode.R))        //手動リロード
        {
            isRemainBullets = false;
            StartCoroutine(Reload());
        }

    }


    private void LateUpdate()       //カメラ操作
    {
        Vector2 angle = new Vector2(Input.GetAxis("Mouse X") * yAngleSpeed, Input.GetAxis("Mouse Y") * xAngleSpeed);
        cameraDir += new Vector3(cameraReverse * angle.y, angle.x, 0);
        
        //アングル制限//
        if (xAngUpLimit >= cameraDir.x) cameraDir.x = xAngUpLimit;
        if (cameraDir.x >= xAngDownLimit) cameraDir.x = xAngDownLimit;
        
        Camera.main.transform.rotation = Quaternion.Euler(cameraDir);
        
    }







    //連射間隔調整関数//
    float timerRecharge=0f;
    IEnumerator ReCharge()
    {
        while (true)
        {
            timerRecharge += Time.deltaTime;
            if (timerRecharge > rechargeTime)
            {
                timerRecharge = 0f;
                isFirePer = true;
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }


    //リロードを行う関数//
    float timerReload=0f;
    IEnumerator Reload()
    {
        while (true)
        {
            timerReload += Time.deltaTime;
            if (timerReload > reloadingTime)
            {
                timerReload = 0f;
                isRemainBullets = true;
                remainBullets = fullBullets;

                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }









    //moveX//
    void moveX(float dirX, bool isRun)
    {
        Vector3 right = Camera.main.transform.TransformDirection(Vector3.right);
        moveDirection = dirX * right;
        if (isRun)
        {
            moveDirection *= runSpeed;      //走る
        }
        else
        {
            moveDirection *= walkSpeed;     //歩く
        }
        controller.Move(moveDirection * Time.deltaTime); //Playerを動かす処理
    }


    //moveZ//
    void moveZ(float dirY, bool isRun)
    {
        Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
        moveDirection = dirY * forward;
        if (isRun)
        {
            moveDirection *= runSpeed;      //走る
        }
        else
        {
            moveDirection *= walkSpeed;     //歩く
        }

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


    //スライディング//
    void sliding()      //GetKeyDownで動作
    {
        Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
        Vector3 right = Camera.main.transform.TransformDirection(Vector3.right);
        if (!isSlide)
        { //タイマー開始
            isSlide = true;
        }
        if (isSlide) timerSlide += Time.deltaTime;       //タイマー加算
        if (isSlide && timerSlide > slideTime)
        { //タイマー終了
            isSlide = false;
            timerSlide = 0.0f;
        }

        if (isSlide)
        { //タイマーチェック
            moveDirection = Input.GetAxis("Horizontal") * right + Input.GetAxis("Vertical") * forward;
            moveDirection.x *= slideSpeed;
            moveDirection.z *= slideSpeed;
        }


        controller.Move(moveDirection * Time.deltaTime); //Playerを動かす処理
    }


    //カメラ上下反転関数//
    int cameraRev(int cameraReverse)        //GetKeyDownで動作
    {
        return cameraReverse * -1;      //上下カメラ反転　cameraReverse が　-1のとき順，1のとき逆
    }




    //angle上下//
    void angleX(float angX,int cameraReverse)
    {
        angle.x = angX*xAngleSpeed;

        cameraDir += new Vector3(cameraReverse*angle.x, 0, 0);
        playerDir += new Vector3(cameraReverse*angle.x, 0, 0);

        if (xAngUpLimit >= cameraDir.x) cameraDir.x = xAngUpLimit;
        if (cameraDir.x >= xAngDownLimit) cameraDir.x = xAngDownLimit;
        if (xAngUpLimit >= playerDir.x) playerDir.x = xAngUpLimit;
        if (playerDir.x >= xAngDownLimit) playerDir.x = xAngDownLimit;

        Camera.main.transform.rotation = Quaternion.Euler(cameraDir);
        this.transform.rotation = Quaternion.Euler(playerDir);
    }


    //angle左右//
    void angleY(float angY)
    {
        angle.y = angY*yAngleSpeed;

        cameraDir += new Vector3(0, angle.y, 0);
        Camera.main.transform.rotation = Quaternion.Euler(cameraDir);       //カメラ向き

        playerDir += new Vector3(0, angle.y, 0);
        this.transform.rotation = Quaternion.Euler(playerDir);      //プレイヤー向き
    }


    //射撃//
    void pressRT()
    {
        if (isFirePer)
        {
            GameObject bullets = Instantiate(Bullet) as GameObject;
            bullets.transform.position = Muzzle.position;       // 弾丸の出現位置を調整

            remainBullets--;

            isFirePer = false;
            StartCoroutine(ReCharge());

            if (remainBullets == 0)
            {
                isRemainBullets = false;
                StartCoroutine(Reload());
            }
        }
        
    }

    
    //手動リロード//
    void selfReload()
    {
        isRemainBullets = false;
        StartCoroutine(Reload());
    }


    
}
