using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWeb : MonoBehaviour
{
    bool doOnce = true;
    void Update()
    {
        if (this.gameObject.activeSelf && doOnce)
        {
            doOnce = false;
            StartCoroutine(Time_Destroy());
        }
    }

    IEnumerator Time_Destroy()
    {
        yield return new WaitForSeconds(5);
        if(this.gameObject.name != "RawImage_Web")
        {
            Destroy(this.gameObject);
        }
        
    }

}
