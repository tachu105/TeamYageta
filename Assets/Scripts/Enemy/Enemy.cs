using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    /// <summary>体力</summary>
    [SerializeField] protected int hp;    
    /// <summary>状態</summary>
    protected EnemyState state;
    [SerializeField] protected SearchArea searchArea;
    protected bool isSleeping = false;
    public int GetHP() { return hp; }
    public EnemyState GetState() { return state; }
    
    /// <summary>
    /// 行動
    /// </summary>
    public abstract void Action();

    /// <summary>
    /// 被ダメージ処理
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public abstract void Damage(Bullet bullet, HitArea area);

    /// <summary>
    /// 死亡時の動作
    /// </summary>
    public abstract void Dead();

    protected void Sleep(float time)
    {
        StartCoroutine(SleepCoroutine(time));
    }
    private IEnumerator SleepCoroutine(float sleepTime)
    {
        float time = 0f;
        isSleeping = true;
        while (time < sleepTime)
        {
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        isSleeping = false;
    }
}

public enum EnemyState
{
    Wait,
    Walk,
    Chase,
    Attack,
    Down,
    Dead
}