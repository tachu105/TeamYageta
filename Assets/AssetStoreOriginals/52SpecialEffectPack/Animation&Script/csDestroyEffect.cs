using UnityEngine;
using System.Collections;

public class csDestroyEffect : MonoBehaviour {

	void Update ()
    {
	    if(this.transform.childCount == 0f)
        {
            Destroy(gameObject);
        }
	}
}
