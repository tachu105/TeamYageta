using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour, InputInterface
{
    /// <summary>
    /// Lスティック上下入力
    /// </summary>
    /// <param name="val">入力値 -1 ~ +1 </param>
    public void LstickVertical(float val)
    {
        Debug.Log("L Stick Vertical:" + val);
    }

    /// <summary>
    /// Lスティック左右入力
    /// </summary>
    /// <param name="val">入力値 -1 ~ +1 </param>
    public void LstickHorizontal(float val)
    {
        Debug.Log("L Stick Horizontal:" + val);
    }

    /// <summary>
    /// Rスティック左右入力
    /// </summary>
    /// <param name="val">入力値　-1 ~ +1</param>
    public void RstickHorizontal(float val)
    {
        Debug.Log("R Stick Vertical:" + val);
    }

    /// <summary>
    /// Rスティック上下入力　
    /// </summary>
    /// <param name="val">入力値　-1 ~ +1</param>
    public void RstickVertical(float val)
    {
        Debug.Log("R Stick Horizontal:" + val);
    }

    /// <summary>
    /// Aボタン
    /// </summary>
    public void PressA()
    {
        Debug.Log("A button pressed");
    }

    /// <summary>
    /// Bボタン
    /// </summary>
    public void PressB()
    {
        Debug.Log("B button pressed");
    }

    /// <summary>
    /// Xボタン
    /// </summary>
    public void PressX()
    {
        Debug.Log("X button pressed");
    }

    /// <summary>
    /// Yボタン
    /// </summary>
    public void PressY()
    {
        Debug.Log("Y button pressed");
    }

    /// <summary>
    /// RT入力
    /// </summary>
    public void PressRT()
    {
        Debug.Log("RT pressed");
    }

    /// <summary>
    /// LT入力
    /// </summary>
    public void PressLT()
    {
        Debug.Log("LT pressed");
    }

    /// <summary>
    /// RB入力
    /// </summary>
    public void PressRB()
    {
        Debug.Log("RB pressed");
    }

    /// <summary>
    /// LB入力
    /// </summary>
    public void PressLB()
    {
        Debug.Log("LB pressed");
    }

    /// <summary>
    /// Lスティック押し込み
    /// </summary>
    public void PressL()
    {
        Debug.Log("L stick button pressed");
    }

    /// <summary>
    /// Rスティック押し込み
    /// </summary>
    public void PressR()
    {
        Debug.Log("R stick button pressed");
    }
}
