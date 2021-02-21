using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SearchArea : MonoBehaviour
{
    /// <summary>
    /// ターゲットのタグ
    /// </summary>
    [SerializeField] private string[] targetsTag;
    /// <summary>
    /// 発見済みか否か
    /// </summary>
    [SerializeField] private bool isDetected = false;
    /// <summary>
    /// 視野角
    /// </summary>
    [SerializeField] private float searchAngle = 100f; //0~180
    /// <summary>
    /// 追跡中のターゲット
    /// </summary>
    public GameObject currentTartget;
    /// <summary>
    /// 見失うまでの時間
    /// </summary>
    [SerializeField] private float timeToMiss = 3f;

    /// <summary>
    /// 発見済みかどうかを取得
    /// </summary>
    /// <returns></returns>
    public bool IsDetected() { return isDetected; }

    //エリアに入った時の処理
    private void OnTriggerStay(Collider other)
    {
        //追跡中のターゲットがいたら無視
        if (isDetected && currentTartget != null) return;

        //入ったのがターゲットか
        if (targetsTag.Contains<string>(other.tag))
        {
            //視野角に収まっているか
            float angle = Vector3.Angle(transform.forward, other.transform.position - transform.position);
            if (angle > searchAngle) return;

            //障害物があるか
            RaycastHit hit;
            if (Physics.Raycast(transform.position, (other.transform.position - transform.position).normalized, out hit, Mathf.Infinity))
            {
                if (hit.collider == other)
                {
                    currentTartget = other.gameObject;
                    isDetected = true;
                }
            }
        }
    }

    //エリアから出たら
    private void OnTriggerExit(Collider other)
    {
        // 追跡中のターゲットだったら
        if(other.gameObject == currentTartget)
        {
            isDetected = false;
            StartCoroutine(MissCoroutine());
        }
    }

    //見逃すまでカウントダウン
    private IEnumerator MissCoroutine()
    {
        float time = 0f;
        while(time < timeToMiss)
        {
            if (isDetected) yield break;
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        currentTartget = null;
    }
}
