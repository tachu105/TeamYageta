using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, InputInterface
{
    //射撃に使うもの//
    bool isFirePer = true;
    public bool isRemainBullets = true;
    public int remainBullets = 100;       //残弾数
    public GameObject Bullet;       // bullet prefab
    public Transform Muzzle;        // 弾丸発射点
    public float rechargeTime = 0.1f;     //連射間隔
    public float reloadingTime = 3f;        //リロード所要時間
    [HideInInspector] public int fullBullets;       //弾数


    //アングル操作に使用するもの//
    private Vector3 cameraDir = Vector3.zero;        //カメラ向き
    private Vector3 playerDir = Vector3.zero;        //プレイヤー向き
    private Vector2 angle = Vector2.zero;            //コントローラー情報格納変数
    private const float xAngUpLimit = -75f;        //上振り向き限界角
    private const float xAngDownLimit = 65f;       //下振り向き限界角
    [SerializeField] private float xAngleSpeed = 1.0f;        //縦振り向き感度
    [SerializeField] private float yAngleSpeed = 1.0f;        //横振り向き感度
    private int cameraReverse = -1;                 //上下カメラ操作反転


    //移動に使用するもの//
    private CharacterController controller;
    private Vector3 moveDirection;      //移動方向変数
    [SerializeField] private float jumpPower = 5f;       //ジャンプ力（上昇速度）
    [SerializeField] private float walkSpeed = 4f;        //歩き速度
    [SerializeField] private float runSpeed = 10f;         //走り速度
    [SerializeField] private float slideSpeed = 20f;      //スライディング速度
    [SerializeField] private float slideTime = 0.3f;      //スライディング時間
    private bool isSlide;

    //物理演算
    private const float GRAVITY = 9.8f;
    private const float RUBBING = 0.1f;     //摩擦

    private InputController inputController;

    // Start is called before the first frame update
    void Start()
    {
        inputController = GetComponent<InputController>();
        fullBullets = remainBullets;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection.y -= GRAVITY * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime); //Playerを動かす処理
    }

    //連射間隔調整関数//
    IEnumerator ReCharge()
    {
        float timerRecharge = 0f;
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
    IEnumerator Reload()
    {
        float timerReload = 0f;
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
        if (isSlide) return;
        float moveVal = dirX;
        moveVal *= isRun ? runSpeed : walkSpeed;
        controller.Move(moveVal * Camera.main.transform.right * Time.deltaTime); //Playerを動かす処理
    }

    //moveZ//
    void moveZ(float dirY, bool isRun)
    {
        if (isSlide) return;
        float moveVal = dirY;
        moveVal *= isRun ? runSpeed : walkSpeed;
        controller.Move(moveVal * Camera.main.transform.forward * Time.deltaTime); //Playerを動かす処理
    }

    //jump//
    void jump()
    {
        moveDirection.y = jumpPower;
    }


    //スライディング//
    IEnumerator SlideCoroutine()      //GetKeyDownで動作
    {
        if (isSlide) yield break;

        isSlide = true;
        float time = 0f;
        float moveVal = slideSpeed;
        while (time < slideTime && moveVal > 0f)
        {
            controller.Move(moveVal * Camera.main.transform.forward * Time.deltaTime); //Playerを動かす処理
            moveVal -= RUBBING;
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        isSlide = false;
    }


    //カメラ上下反転関数//
    int cameraRev(int cameraReverse)        //GetKeyDownで動作
    {
        return cameraReverse * -1;      //上下カメラ反転　cameraReverse が　-1のとき順，1のとき逆
    }

    //angle上下//
    void angleVartical(float angX, int cameraReverse)
    {
        angle.x = angX * xAngleSpeed;

        cameraDir += new Vector3(cameraReverse * angle.x, 0, 0);
        playerDir += new Vector3(cameraReverse * angle.x, 0, 0);

        if (xAngUpLimit >= cameraDir.x) cameraDir.x = xAngUpLimit;
        if (cameraDir.x >= xAngDownLimit) cameraDir.x = xAngDownLimit;
        if (xAngUpLimit >= playerDir.x) playerDir.x = xAngUpLimit;
        if (playerDir.x >= xAngDownLimit) playerDir.x = xAngDownLimit;

        Camera.main.transform.rotation = Quaternion.Euler(cameraDir);
        this.transform.rotation = Quaternion.Euler(playerDir);
    }


    //angle左右//
    void angleHorizontal(float angY)
    {
        angle.y = angY * yAngleSpeed;

        cameraDir += new Vector3(0, angle.y, 0);
        Camera.main.transform.rotation = Quaternion.Euler(cameraDir);       //カメラ向き

        playerDir += new Vector3(0, angle.y, 0);
        this.transform.rotation = Quaternion.Euler(playerDir);      //プレイヤー向き
    }


    //射撃//
    void Shoot()
    {
        if (!isRemainBullets) selfReload();
        else
        {
            if (!isFirePer) return;

            GameObject bullets = Instantiate(Bullet) as GameObject;
            // 弾丸の位置を調整
            bullets.transform.position = Muzzle.position;

            remainBullets--;

            isFirePer = false;
            StartCoroutine(ReCharge());

            if (remainBullets <= 0f) isRemainBullets = false;
        }

    }


    //手動リロード//
    void selfReload()
    {
        StartCoroutine(Reload());
    }

    /// <summary>
    /// Lスティック上下入力
    /// </summary>
    /// <param name="val">入力値 -1 ~ +1 </param>
    public void LstickVertical(float val)
    {
        moveZ(val, inputController.L);
    }

    /// <summary>
    /// Lスティック左右入力
    /// </summary>
    /// <param name="val">入力値 -1 ~ +1 </param>
    public void LstickHorizontal(float val)
    {
        moveX(val, inputController.L);
    }

    /// <summary>
    /// Rスティック左右入力
    /// </summary>
    /// <param name="val">入力値　-1 ~ +1</param>
    public void RstickHorizontal(float val)
    {
        angleHorizontal(val);
    }

    /// <summary>
    /// Rスティック上下入力　
    /// </summary>
    /// <param name="val">入力値　-1 ~ +1</param>
    public void RstickVertical(float val)
    {

        angleVartical(val, cameraReverse);
    }

    /// <summary>
    /// Aボタン
    /// </summary>
    public void PressA()
    {

    }

    /// <summary>
    /// Bボタン
    /// </summary>
    public void PressB()
    {
        //ジャンプ
        jump();
    }

    /// <summary>
    /// Xボタン
    /// </summary>
    public void PressX()
    {
        selfReload();       //手動リロード
    }

    /// <summary>
    /// Yボタン
    /// </summary>
    public void PressY()
    {
        cameraReverse *= -1;
    }

    /// <summary>
    /// RT入力
    /// </summary>
    public void PressRT()
    {
        Shoot();
    }

    /// <summary>
    /// LT入力
    /// </summary>
    public void PressLT()
    {

    }

    /// <summary>
    /// RB入力
    /// </summary>
    public void PressRB()
    {

    }

    /// <summary>
    /// LB入力
    /// </summary>
    public void PressLB()
    {
        StartCoroutine(SlideCoroutine());
    }

    /// <summary>
    /// Lスティック押し込み
    /// </summary>
    public void PressL()
    {

    }

    /// <summary>
    /// Rスティック押し込み
    /// </summary>
    public void PressR()
    {

    }

}
