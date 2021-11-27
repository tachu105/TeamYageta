using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaFloor : MonoBehaviour
{
    [SerializeField] private float damageSecond = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {

            Player.instance.Hp -= (int)(damageSecond * Time.deltaTime);
        }
    }
}
