using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private float remainTime = 1f;
    [SerializeField] private Color criticalColor;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color badColor;
    private Text text;

    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        text.color = Color.Lerp(text.color, new Color(1f, 0f, 0f, 0f), remainTime * Time.deltaTime);
    }

    public void ShowDamage(float damage, HitArea hitArea)
    {
        text.text = ((int)(damage * hitArea.damageRate)).ToString();
        if (hitArea.damageRate > 1f) text.color = criticalColor;
        else if (hitArea.damageRate < 1f) text.color = badColor;
        else text.color = normalColor;
    }
}
