using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallObject : MonoBehaviour
{
    [SerializeField] private bool isRandomDelay = false;
    [SerializeField] private float delayTime = 0f;
    [SerializeField] private float fallSpeed = 1f;
    [SerializeField] private float maxFallLength = 5f;
    [SerializeField] private GameObject effect;
    [SerializeField] private Transform effectPosition;

    private float fallLength = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if (isRandomDelay) delayTime = Random.Range(0f, delayTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartFall()
    {
        StartCoroutine(FallCoroutine());
    }

    private IEnumerator FallCoroutine()
    {
        Instantiate(effect, effectPosition.position, Quaternion.identity, this.transform);

        yield return new WaitForSeconds(delayTime);

        while(fallLength < maxFallLength)
        {
            float moveVal = fallSpeed * Time.deltaTime;
            fallLength += moveVal;
            this.transform.position += Vector3.down * moveVal;
            yield return new WaitForEndOfFrame();
        }

        Destroy(this.gameObject);
    }
}
