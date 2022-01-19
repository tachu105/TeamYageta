using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallObject : MonoBehaviour
{
    [SerializeField] private bool isRandomDelay = false;
    [SerializeField] private float delayTime = 0f;
    [SerializeField] private float fallSpeed = 1f;          //現在の落下速度
    [SerializeField] private float gravity = 0f;            //落下の加速度 (0の場合は定速)
    [SerializeField] private float maxFallSpeed = 50f;      //最大落下速度
    [SerializeField] private float maxFallLength = 5f;      //最大落下距離 (0の場合は無限)
    [SerializeField] private GameObject effect;
    [SerializeField] private Transform effectPosition;

    private float fallLength = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if (isRandomDelay) delayTime = Random.Range(0f, delayTime);
        if (gravity != 0f) fallSpeed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool isFalling = false;
    public void StartFall()
    {
        if (isFalling) return;
        isFalling = true;
        StartCoroutine(FallCoroutine());
    }

    public void StopFall()
    {
        if (!isFalling) return;
        isFalling = false;
        StopAllCoroutines();
    }

    private IEnumerator FallCoroutine()
    {
        if(effect) Instantiate(effect, effectPosition.position, Quaternion.identity, this.transform);

        yield return new WaitForSeconds(delayTime);

        if (maxFallLength == 0f)
        {
            while (true)
            {
                float moveVal = fallSpeed * Time.deltaTime;
                if(fallSpeed < maxFallSpeed) fallSpeed += gravity * Time.deltaTime;
                this.transform.position += Vector3.down * moveVal;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (fallLength < maxFallLength)
            {
                float moveVal = fallSpeed * Time.deltaTime;
                if (fallSpeed < maxFallSpeed) fallSpeed += gravity * Time.deltaTime;
                fallLength += moveVal;
                this.transform.position += Vector3.down * moveVal;
                yield return new WaitForEndOfFrame();
            }
        }

        Destroy(this.gameObject);
    }
}
