using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, InputInterface
{
    //射撃に使うもの//
    public GameObject Bullet;
    [HideInInspector] public bool isRemainBullets = true;
    [HideInInspector] public bool isReloading = false;
    [HideInInspector] public int remainBullets = 100;       //残弾数
    [HideInInspector] public float rechargeTime = 0.1f;         //連射間隔
    [HideInInspector] public float reloadingTime = 2.2f;        //リロード所要時間
    private bool isFirePer = true;
    private Vector3 Muzzle;
    private bool isReloadEnd = true;
    public int fullBullets;       //弾数


    //アングル操作に使用するもの//
    private float cameraDirX;         //X軸カメラ向き
    private float cameraDirY;         //Y軸カメラ向き
    private Vector3 playerDir = Vector3.zero;        //プレイヤー向き
    private Vector2 angle = Vector2.zero;            //コントローラー情報格納変数
    private const float xAngUpLimit = -75f;        //上振り向き限界角
    private const float xAngDownLimit = 47f;       //下振り向き限界角
    [SerializeField] public float xAngleSpeed = 1.0f;        //縦振り向き感度
    [SerializeField] public float yAngleSpeed = 1.0f;        //横振り向き感度
    public int cameraReverse = -1;                 //上下カメラ操作反転
    private bool isPressYBefore;
    private bool isPressYNow = false;


    //移動に使用するもの//
    private CharacterController controller;
    private Vector3 gravityDirection;      //プレイヤーの重力
    private float moveValX;
    private float moveValZ;
    private float slideValZ;
    private float slideValX;
    private int jumpCounter = 0;
    [SerializeField] private float jumpPower = 10f;       //ジャンプ力
    [SerializeField] private int jumpCount = 2;         //ジャンプ回数
    [SerializeField] private float walkSpeed = 4f;        //歩き速度
    [SerializeField] private float runSpeed = 7f;         //走り速度
    [SerializeField] private float slideSpeed = 19f;      //スライディング速度
    [SerializeField] private float slideTime = 0.4f;      //スライディング時間
    private bool isSlide;
    private bool isSlidePer;
    private bool isSlideCountPer=true;
    private bool isPressABefore;
    private bool isPressANow = false;
    private bool isPressRBBefore;
    private bool isPressRBNow = false;
    private bool isPressLBBefore;
    private bool isPressLBNow = false;


    //HP//
    [SerializeField] private int easy = 1500;
    [SerializeField] private int normal = 1000;
    [SerializeField] private int hard = 500;
    [HideInInspector] public int Hp;
    [HideInInspector] public int difficulty;


    //Dead//
    [HideInInspector] public bool isDead = false;
    [SerializeField] private GameObject deadObject;
    [SerializeField] private GameObject gameOverWindow;

    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip slideSound;

    //物理演算//
    private const float GRAVITY = 13f;
    private const float RUBBING = 0.002f;     //摩擦


    public InputController inputController;
    private Animator animator;
    private AudioSource audioSource;

    public static Player instance;

    void Awake()
    {
        if (instance) Destroy(this.gameObject);
        instance = this;
        Player.instance.xAngleSpeed = GameManager.instance.xSpeedSlider.value;
        Player.instance.yAngleSpeed = GameManager.instance.ySpeedSlider.value;
        Player.instance.cameraReverse = GameManager.instance.flipToggle.isOn ? 1 : -1;
        
        difficulty = GameManager.instance.difficultyValue;
        if (difficulty == 1) Hp = easy;
        else if (difficulty == 2) Hp = normal;
        else if (difficulty == 3) Hp = hard;
        else Hp = normal;
    }

    void Start()
    {
        inputController = GetComponent<InputController>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        fullBullets = remainBullets;
        Player.instance.inputController.isUseKeyBoard = GameManager.instance.KeyboardToggle.isOn;
        Player.instance.inputController.isUseXboxPad = GameManager.instance.XboxToggle.isOn;
        Player.instance.inputController.isUsePsPad = GameManager.instance.PsToggle.isOn;
    }


    void Update()
    {
        audioSource.volume = GameManager.instance.SEVolume;

        if (isDead) return;
        if (Hp < 0f) Dead();
        gravityDirection.y -= GRAVITY * Time.deltaTime;
        controller.Move(gravityDirection * Time.deltaTime);        //Playerの重力

        //カメラ上下反転判定//
        isPressYBefore = isPressYNow;
        isPressYNow = inputController.Y;

        //自動リロード//
        if (!isRemainBullets && jumpCounter == 0) SelfReload();

        //ジャンプ回数制限//
        if (controller.isGrounded) jumpCounter = 0;
        isPressABefore = isPressANow;
        isPressANow = inputController.A;
        if (!isPressABefore && isPressANow) jumpCounter++;
        isPressRBBefore = isPressRBNow;
        isPressRBNow = inputController.RB;
        if (!isPressRBBefore && isPressRBNow) jumpCounter++;

        //スラーディング制限//
        isPressLBBefore = isPressLBNow;
        isPressLBNow = inputController.LB;

        //弾丸発射位置変更//
        Muzzle = inputController.LT ? GameObject.Find("Muzzle2").transform.position : GameObject.Find("Muzzle1").transform.position;

        //スライディング許可//
        if (inputController.LB && (inputController.L_H != 0f || inputController.L_V != 0f)) isSlidePer = true;
        else isSlidePer = false;

        //アニメーション//
        animator.SetBool("reload", isReloading);
        if (inputController.LT)
        {
            animator.SetFloat("moveZ", 0);
            animator.SetBool("Run", false);
            animator.SetBool("scope", true);
        }
        else animator.SetBool("scope", false);

        if (isSlide)
        {
            animator.SetFloat("moveZ", 0);
            animator.SetBool("Run", false);
            animator.SetBool("scope", true);
            animator.SetBool("slide", true);
        }
        else animator.SetBool("slide", false);

        if (controller.isGrounded)
        {
            //歩く//
            if (inputController.L_H > 0) animator.SetFloat("moveZ", inputController.L_H);
            else if (inputController.L_V > 0) animator.SetFloat("moveZ", inputController.L_V);
            else if (inputController.L_H < 0) animator.SetFloat("moveZ", -inputController.L_H);
            else if (inputController.L_V < 0) animator.SetFloat("moveZ", -inputController.L_V);
            else if (inputController.L_V == 0 && inputController.L_H == 0) animator.SetFloat("moveZ", 0);
            //走る//
            if (inputController.L && !inputController.RT)
            {
                if (inputController.L_H != 0 || inputController.L_V != 0) animator.SetBool("Run", true);
                else animator.SetBool("Run", false);
            }
            else animator.SetBool("Run", false);
        }
        else
        {
            animator.SetFloat("moveZ", 0);
            animator.SetBool("Run", false);
        }


        
    }

   
    void LateUpdate()
    {
        Camera.main.transform.rotation = Quaternion.Euler(cameraDirX,cameraDirY,0f);
    }


    void Dead()
    {
        isDead = true;
        gameOverWindow.SetActive(true);
        GameManager.instance.EndGame();
        GameObject obj = Instantiate(deadObject, Camera.main.transform.position, Quaternion.identity);
        Camera.main.transform.parent = obj.transform;
        obj.GetComponent<Rigidbody>().AddForce(-this.transform.forward, ForceMode.Impulse);
        Destroy(this.gameObject);
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
                isFirePer = true;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }


    //リロードを行う関数//
    IEnumerator Reload()
    {
        if (isReloading) yield break;
        audioSource.PlayOneShot(reloadSound);
        isReloading = true;
        float timerReload = 0f;
        float time = 0f;
        while (true)
        {
            timerReload += Time.deltaTime;
            if (timerReload > reloadingTime)
            {
                isRemainBullets = true;
                remainBullets = fullBullets;
                isReloading = false;

                isReloadEnd = false;
                while (time < 0.5f)
                {
                    time += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
                isReloadEnd = true;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }


    //moveX//
    void MoveX(float dirX, bool isRun)
    {
        if (isSlide) return;
        moveValX = dirX;
        if (inputController.RT || !isRun || inputController.LT || isReloading) moveValX *= walkSpeed;
        else if (isRun) moveValX *= runSpeed;
        if (inputController.L_V != 0) moveValX /= Mathf.Sqrt(2);
        controller.Move(moveValX * Camera.main.transform.right * Time.deltaTime); //Playerを動かす処理
    }


    //moveZ//
    void MoveZ(float dirY, bool isRun)
    {
        if (isSlide) return;
        moveValZ = dirY;
        if (inputController.RT || !isRun || inputController.LT || isReloading) moveValZ *= walkSpeed;
        else if (isRun) moveValZ *= runSpeed;
        if (inputController.L_H != 0) moveValZ /= Mathf.Sqrt(2);
        controller.Move(moveValZ * Camera.main.transform.forward * Time.deltaTime); //Playerを動かす処理
    }


    //jump//
    IEnumerator Jump()
    {
        if (jumpCounter > jumpCount) yield break;
        gravityDirection.y = jumpPower;
        yield return new WaitForEndOfFrame();
    }


    //スライディング//
    IEnumerator SlideCoroutine()      
    {
        if (isSlide) yield break;

        isSlide = true;
        isSlideCountPer = false;
        float time = 0f;

        audioSource.PlayOneShot(slideSound);

        slideValZ = inputController.L_V;
        slideValX = inputController.L_H;
        Vector3 slideVal = (slideValZ * Camera.main.transform.forward + slideValX * Camera.main.transform.right) * slideSpeed * Time.deltaTime;

        while (time < 0.1f)
        {
            Camera.main.transform.localPosition = Vector3.up * Mathf.Lerp(1.95f, 1.6f, time * 10f);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        time = 0f;
        while (time < slideTime&&((slideVal.x>0.01f||slideVal.x<-0.01f)||(slideVal.z>0.01f||slideVal.z<-0.01f)))
        {
            controller.Move(slideVal); 
            if (slideVal.x > 0.01f) slideVal.x -= RUBBING;
            else if (slideVal.x < -0.01f) slideVal.x += RUBBING;
            if (slideVal.z > 0.01f) slideVal.z -= RUBBING;
            else if (slideVal.z < -0.01f) slideVal.z += RUBBING;

            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        time = 0f;
        while (time < 0.13f)
        {
            Camera.main.transform.localPosition = Vector3.up * Mathf.Lerp(1.6f, 1.95f,  time * 10f);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        isSlide = false;

        time = 0f;
        while (time < 0.2f)
        {
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        isSlideCountPer = true;
    }


    //angle上下//
    void AngleVartical(float angX, int cameraReverse)
    {
        angle.x = angX * xAngleSpeed;
        
        cameraDirX += cameraReverse * angle.x;
        playerDir += new Vector3(cameraReverse * angle.x, 0, 0);

        if (xAngUpLimit >= cameraDirX) cameraDirX = xAngUpLimit;
        if (cameraDirX >= xAngDownLimit) cameraDirX = xAngDownLimit;
        if (xAngUpLimit >= playerDir.x) playerDir.x = xAngUpLimit;
        if (playerDir.x >= xAngDownLimit) playerDir.x = xAngDownLimit;

        this.transform.rotation = Quaternion.Euler(playerDir);      //プレイヤー向き
    }


    //angle左右//
    void AngleHorizontal(float angY)
    {
        angle.y = angY * yAngleSpeed;

        cameraDirY += angle.y;
        playerDir += new Vector3(0, angle.y, 0);
        this.transform.rotation = Quaternion.Euler(playerDir);      //プレイヤー向き
    }


    //射撃//
    void Shoot()
    {
        
        if(isRemainBullets)
        {
            if (!isFirePer) return;

            Bullet bullet = Instantiate(Bullet).GetComponent<Bullet>();
            bullet.parent = this.gameObject;
            // 弾丸の位置を調整
            bullet.transform.position = Muzzle;
            bullet.dir = Camera.main.transform.forward;

            remainBullets--;

            isFirePer = false;
            StartCoroutine(ReCharge());

            if (remainBullets <= 0f) isRemainBullets = false;
        }
    }


    //手動リロード//
    void SelfReload()
    {
        isRemainBullets = false;
        StartCoroutine(Reload());
    }


    



    /// <summary>
    /// Lスティック上下入力
    /// </summary>
    /// <param name="val">入力値 -1 ~ +1 </param>
    public void LstickVertical(float val)
    {
        MoveZ(val, inputController.L);
    }

    /// <summary>
    /// Lスティック左右入力
    /// </summary>
    /// <param name="val">入力値 -1 ~ +1 </param>
    public void LstickHorizontal(float val)
    {
        MoveX(val, inputController.L);
    }

    /// <summary>
    /// Rスティック左右入力
    /// </summary>
    /// <param name="val">入力値　-1 ~ +1</param>
    public void RstickHorizontal(float val)
    {
        AngleHorizontal(val);
    }

    /// <summary>
    /// Rスティック上下入力　
    /// </summary>
    /// <param name="val">入力値　-1 ~ +1</param>
    public void RstickVertical(float val)
    {
        AngleVartical(val, cameraReverse);
    }

    /// <summary>
    /// Aボタン
    /// </summary>
    public void PressA()
    {
        if (!isPressABefore && isPressANow) StartCoroutine(Jump());     //ジャンプ
    }

    /// <summary>
    /// Bボタン
    /// </summary>
    public void PressB()
    {
    
    }

    /// <summary>
    /// Xボタン
    /// </summary>
    public void PressX()
    {
        if(jumpCounter==0&&isSlideCountPer) SelfReload();       //手動リロード
    }

    /// <summary>
    /// Yボタン
    /// </summary>
    public void PressY()
    {
        if(!isPressYBefore&&isPressYNow) cameraReverse *= -1;        //上下カメラ反転　cameraReverse が　-1のとき順，1のとき逆
    }

    /// <summary>
    /// RT入力
    /// </summary>
    public void PressRT()
    {
        if(isSlideCountPer&&isReloadEnd&&!isSlide) Shoot();       //射撃
    }

    /// <summary>
    /// LT入力
    /// </summary>
    public void PressLT()
    {
        //スコープ
    }

    /// <summary>
    /// RB入力
    /// </summary>
    public void PressRB()
    {
        if (!isPressRBBefore && isPressRBNow) StartCoroutine(Jump());     //ジャンプ
    }

    /// <summary>
    /// LB入力
    /// </summary>
    public void PressLB()
    {
        if (!isPressLBBefore && isPressLBNow)
            if(isSlidePer&&!isSlide&&!isReloading)
                StartCoroutine(SlideCoroutine());       //スライディング
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

