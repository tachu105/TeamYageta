using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InputInterface
{
    /// <summary>
    /// Lスティック上下入力
    /// </summary>
    /// <param name="val">入力値 -1 ~ +1 </param>
    void LstickVertical(float val);

    /// <summary>
    /// Lスティック左右入力
    /// </summary>
    /// <param name="val">入力値 -1 ~ +1 </param>
    void LstickHorizontal(float val);

    /// <summary>
    /// Rスティック左右入力
    /// </summary>
    /// <param name="val">入力値　-1 ~ +1</param>
    void RstickHorizontal(float val);

    /// <summary>
    /// Rスティック上下入力　
    /// </summary>
    /// <param name="val">入力値　-1 ~ +1</param>
    void RstickVertical(float val);

    /// <summary>
    /// Aボタン
    /// </summary>
    void PressA();

    /// <summary>
    /// Bボタン
    /// </summary>
    void PressB();

    /// <summary>
    /// Xボタン
    /// </summary>
    void PressX();

    /// <summary>
    /// Yボタン
    /// </summary>
    void PressY();

    /// <summary>
    /// RT入力
    /// </summary>
    void PressRT();

    /// <summary>
    /// LT入力
    /// </summary>
    void PressLT();

    /// <summary>
    /// RB入力
    /// </summary>
    void PressRB();

    /// <summary>
    /// LB入力
    /// </summary>
    void PressLB();

    /// <summary>
    /// Lスティック押し込み
    /// </summary>
    void PressL();

    /// <summary>
    /// Rスティック押し込み
    /// </summary>
    void PressR();

}

public class InputController : MonoBehaviour
{
    public bool isUsePad = false;
    private InputInterface player;

    protected float L_V;
    protected float L_H;
    protected float R_V;
    protected float R_H;
    protected bool RT = false;
    protected bool LT = false;
    protected bool RB = false;
    protected bool LB = false;
    protected bool A = false;
    protected bool B = false;
    protected bool X = false;
    protected bool Y = false;
    protected bool L = false;
    protected bool R = false;

    [SerializeField] private string keyUp = "w";
    [SerializeField] private string keyDown = "s";
    [SerializeField] private string keyLeft = "a";
    [SerializeField] private string keyRight = "d";
    [SerializeField] private string keyA = "z";
    [SerializeField] private string keyB = "x";
    [SerializeField] private string keyX = "c";
    [SerializeField] private string keyY = "v";
    [SerializeField] private string keyRT = "l";
    [SerializeField] private string keyLT = "k";
    [SerializeField] private string keyRB = "o";
    [SerializeField] private string keyLB = "i";
    [SerializeField] private string keyL = "left shift";
    [SerializeField] private string keyR = "right shift";


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<InputInterface>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isUsePad) checkPadInput();
        else checkKeybordInput();
    }


    void checkPadInput()
    {
        L_V = Input.GetAxis("L Stick Vertical");
        if (L_V != 0) player.LstickVertical(L_V);
        L_H = Input.GetAxis("L Stick Horizontal");
        if (L_H != 0) player.LstickHorizontal(L_H);
        R_V = Input.GetAxis("R Stick Vertical");
        if (R_V != 0) player.RstickVertical(R_V);
        R_H = Input.GetAxis("R Stick Horizontal");
        if (R_H != 0) player.RstickHorizontal(R_H);

        A = Input.GetButtonDown("A button");
        if (A) player.PressA();
        B = Input.GetButtonDown("B button");
        if (B) player.PressB();
        X = Input.GetButtonDown("X button");
        if (X) player.PressX();
        Y = Input.GetButtonDown("Y button");
        if (Y) player.PressY();

        float trigger = Input.GetAxis("LT RT");
        if (trigger < 0f)
        {
            LT = true;
            player.PressLT();
        }
        else if (trigger > 0f)
        {
            RT = true;
            player.PressRT();
        }
        else RT = LT = false;

        RB = Input.GetButtonDown("RB");
        if (RB) player.PressRB();
        LB = Input.GetButtonDown("LB");
        if (LB) player.PressLB();

        R = Input.GetButtonDown("R button");
        if (R) player.PressR();
        L = Input.GetButtonDown("L button");
        if (L) player.PressL();
    }

    void checkKeybordInput()
    {
        if (Input.GetKey(keyUp) && Input.GetKey(keyDown)) L_V = 0f;
        else if (Input.GetKey(keyUp)) L_V = 1f;
        else if (Input.GetKey(keyDown)) L_V = -1f;
        else L_V = 0f;
        if (L_V != 0) player.LstickVertical(L_V);
        if (Input.GetKey(keyLeft) && Input.GetKey(keyRight)) L_H = 0f;
        else if (Input.GetKey(keyLeft)) L_H = -1f;
        else if (Input.GetKey(keyRight)) L_H = 1f;
        else L_H = 0f;
        if (L_H != 0) player.LstickHorizontal(L_H);

        R_V = Input.GetAxis("Mouse X");
        if (R_V != 0) player.RstickVertical(R_V);
        R_H = Input.GetAxis("Mouse Y");
        if (R_H != 0) player.RstickHorizontal(R_H);

        A = Input.GetKey(keyA);
        if (A) player.PressA();
        B = Input.GetKey(keyB);
        if (B) player.PressB();
        X = Input.GetKey(keyX);
        if (X) player.PressX();
        Y = Input.GetKey(keyY);
        if (Y) player.PressY();

        RT = Input.GetKey(keyRT);
        if (RT) player.PressRT();
        LT = Input.GetKey(keyLT);
        if (LT) player.PressLT();
        RB = Input.GetKey(keyRB);
        if (RB) player.PressRB();
        LB = Input.GetKey(keyLB);
        if (LB) player.PressLB();
        R = Input.GetKey(keyR);
        if (R) player.PressR();
        L = Input.GetKey(keyL);
        if (L) player.PressL();
    }
}
