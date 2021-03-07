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
    public bool isUseXboxPad = false;
    public bool isUsePsPad = false;
    private InputInterface player;
    private Player mainScript;

    public float L_V;
    public float L_H;
    public float R_V;
    public float R_H;
    public bool RT = false;
    public bool LT = false;
    public bool RB = false;
    public bool LB = false;
    public bool A = false;
    public bool B = false;
    public bool X = false;
    public bool Y = false;
    public bool L = false;
    public bool R = false;

    [SerializeField] private string keyUp = "w";
    [SerializeField] private string keyDown = "s";
    [SerializeField] private string keyLeft = "a";
    [SerializeField] private string keyRight = "d";
    [SerializeField] private string keyA = "z";
    [SerializeField] private string keyB = "space";
    [SerializeField] private string keyX = "r";
    [SerializeField] private string keyY = "p";
    [SerializeField] private string keyRT = "0";
    [SerializeField] private string keyLT = "1";
    [SerializeField] private string keyRB = "o";
    [SerializeField] private string keyLB = "left ctrl";
    [SerializeField] private string keyL = "left shift";
    [SerializeField] private string keyR = "right shift";


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<InputInterface>();
        mainScript = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isUseXboxPad && !isUsePsPad) checkXboxPadInput();
        else if (!isUseXboxPad && isUsePsPad) checkPSPadInput();
        else checkKeybordInput();
    }

    
    void checkXboxPadInput()
    {
        R_V = -Input.GetAxis("R Stick Vertical");
        if (R_V != 0) player.RstickVertical(R_V);
        R_H = Input.GetAxis("R Stick Horizontal");
        if (R_H != 0) player.RstickHorizontal(R_H);
        L_V = Input.GetAxis("L Stick Vertical");
        if (L_V != 0) player.LstickVertical(L_V);
        L_H = Input.GetAxis("L Stick Horizontal");
        if (L_H != 0) player.LstickHorizontal(L_H);
        

        A = Input.GetButton("A button");
        if (A) player.PressA();
        B = Input.GetButton("B button");
        if (B) player.PressB();
        X = Input.GetButton("X button");
        if (X) player.PressX();
        Y = Input.GetButton("Y button");
        if (Y) player.PressY();

        float triggerLT = Input.GetAxis("LT");
        if (triggerLT > 0f)
        {
            LT = true;
            player.PressLT();
        }
        else LT = false;
        float triggerRT = Input.GetAxis("RT");
        if (triggerRT > 0f)
        {
            RT = true;
            player.PressRT();
        }
        else RT = false;

        RB = Input.GetButton("RB");
        if (RB) player.PressRB();
        LB = Input.GetButton("LB");
        if (LB) player.PressLB();

        R = Input.GetButton("R button");
        if (R) player.PressR();
        L = Input.GetButton("L button");
        if (L) player.PressL();
    }

    void checkKeybordInput()
    {
        R_V = Input.GetAxis("Mouse Y");
        if (R_V != 0) player.RstickVertical(R_V);
        R_H = Input.GetAxis("Mouse X");
        if (R_H != 0) player.RstickHorizontal(R_H);
        if (GetInput(keyUp) && GetInput(keyDown)) L_V = 0f;
        else if (GetInput(keyUp)) L_V = 1f;
        else if (GetInput(keyDown)) L_V = -1f;
        else L_V = 0f;
        if (L_V != 0) player.LstickVertical(L_V);
        if (GetInput(keyLeft) && GetInput(keyRight)) L_H = 0f;
        else if (GetInput(keyLeft)) L_H = -1f;
        else if (GetInput(keyRight)) L_H = 1f;
        else L_H = 0f;
        if (L_H != 0) player.LstickHorizontal(L_H);

        

        A = GetInput(keyA);
        if (A) player.PressA();
        B = GetInput(keyB);
        if (B) player.PressB();
        X = GetInput(keyX);
        if (X) player.PressX();
        Y = GetInput(keyY);
        if (Y) player.PressY();

        RT = GetInput(keyRT);
        if (RT) player.PressRT();
        LT = GetInput(keyLT);
        if (LT) player.PressLT();
        RB = GetInput(keyRB);
        if (RB) player.PressRB();
        LB = GetInput(keyLB);
        if (LB) player.PressLB();
        R = GetInput(keyR);
        if (R) player.PressR();
        L = GetInput(keyL);
        if (L) player.PressL();
    }

    bool GetInput(string str)
    {
        int n;
        bool isNumber = int.TryParse(str, out n);
        if (isNumber) return Input.GetMouseButton(n);
        else return Input.GetKey(str);
    }


    void checkPSPadInput()
    {
        R_V = -Input.GetAxis("DS4 R Stick Vertical");
        if (R_V != 0) player.RstickVertical(R_V);
        R_H = Input.GetAxis("DS4 R Stick Horizontal");
        if (R_H != 0) player.RstickHorizontal(R_H);
        L_V = Input.GetAxis("DS4 L Stick Vertical");
        if (L_V != 0) player.LstickVertical(L_V);
        L_H = Input.GetAxis("DS4 L Stick Horizontal");
        if (L_H != 0) player.LstickHorizontal(L_H);


        A = Input.GetButton("DS4 X button");
        if (A) player.PressA();
        B = Input.GetButton("DS4 circle button");
        if (B) player.PressB();
        X = Input.GetButton("DS4 square button");
        if (X) player.PressX();
        Y = Input.GetButton("DS4 triangle button");
        if (Y) player.PressY();

        LT = Input.GetButton("DS4 L2");
        if (LT) player.PressLT();
        RT = Input.GetButton("DS4 R2");
        if (RT) player.PressRT();
        RB = Input.GetButton("DS4 R1");
        if (RB) player.PressRB();
        LB = Input.GetButton("DS4 L1");
        if (LB) player.PressLB();

        R = Input.GetButton("DS4 R button");
        if (R) player.PressR();
        L = Input.GetButton("DS4 L button");
        if (L) player.PressL();
    }

    
}


