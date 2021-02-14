using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InputInterface
{
    /// <summary>
    /// Lスティック上下入力
    /// </summary>
    /// <param name="moveVal">入力値 -1 ~ +1 </param>
    void moveZ(float moveVal);

    /// <summary>
    /// Lスティック左右入力
    /// </summary>
    /// <param name="moveVal">入力値 -1 ~ +1 </param>
    void moveX(float moveVal);

    /// <summary>
    /// Rスティック左右入力
    /// </summary>
    /// <param name="moveVal">入力値　-1 ~ +1</param>
    void lookX(float moveVal);

    /// <summary>
    /// Rスティック上下入力　
    /// </summary>
    /// <param name="moveVal">入力値　-1 ~ +1</param>
    void lookY(float moveVal);

    /// <summary>
    /// 射撃(RT)
    /// </summary>
    void pressRT();
}

public class InputController : MonoBehaviour
{
    public bool isUsePad = false;
    private Player player;

    protected float L_Z;
    protected float L_X;
    protected float R_Z;
    protected float R_X;
    protected bool RT = false;

    [SerializeField] private string shootKey = "k";


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isUsePad) checkPadInput();
        else checkKeybordInput();
    }


    void checkPadInput()
    {
        L_Z = Input.GetAxis("Vertical");
        if (L_Z != 0) player.moveZ(L_Z);
        L_X = Input.GetAxis("Horizontal");
        if (L_X != 0) player.moveX(L_X);
        L_Z = Input.GetAxis("Vertical2");
        if (L_Z != 0) player.moveZ(L_Z);
        L_X = Input.GetAxis("Horizontal2");
        if (L_X != 0) player.moveX(L_X);

        RT = Input.GetButtonDown("RT");
        if (RT) player.pressRT();
    }

    void checkKeybordInput()
    {
        if (Input.GetKey("w") && Input.GetKey("s")) L_Z = 0f;
        else if (Input.GetKey("w")) L_Z = 1f;
        else if (Input.GetKey("s")) L_Z = -1f;
        else L_Z = 0f;
        if (L_Z != 0) player.moveZ(L_Z);
        if (Input.GetKey("a") && Input.GetKey("d")) L_X = 0f;
        else if (Input.GetKey("a")) L_X = -1f;
        else if (Input.GetKey("d")) L_X = 1f;
        else L_Z = 0f;
        if (L_X != 0) player.moveX(L_X);

        L_Z = Input.GetAxis("Mouse Y");
        if (L_Z != 0) player.moveZ(R_Z);
        L_X = Input.GetAxis("Mouse X");
        if (L_X != 0) player.moveX(R_X);

        RT = Input.GetKey(KeyCode.KeypadEnter);
        if (RT) player.pressRT();
    }
}
