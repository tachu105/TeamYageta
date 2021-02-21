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