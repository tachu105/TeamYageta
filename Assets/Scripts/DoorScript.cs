using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    //ドアのエリアに入っているか
    private bool isNear;
    //ドアのアニメーター
    private Animator animator;

   
    
    // Start is called before the first frame update
    void Start()
    {
        isNear = false;
        animator = transform.parent.GetComponent<Animator>();

    
    }

    // Update is called once per frame
    void Update()
    {
        if(isNear){
            animator.SetBool("Open",!animator.GetBool("Open"));
        

        }
    }


    void OnTriggerEnter(Collider col){
        if(col.tag == "Player"){
            isNear = true;
        }
    }


   void OnTriggerExit(Collider col){
      if(col.tag == "Player"){
           isNear = false;
       }
    } 

}
