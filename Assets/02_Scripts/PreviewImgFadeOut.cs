using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewImgFadeOut : MonoBehaviour
{
    public GameObject joystick;
    void Start()
    {
        joystick.SetActive(false);
        StartCoroutine(FadeOutCoroutine());
    }

    
    IEnumerator FadeOutCoroutine(){
        yield return new WaitForSeconds(3f);
        float alp = 1.0f;
        while(alp > 0f)
        {
            alp -= 0.01f;
            yield return new WaitForSeconds(0.015f);
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alp);
        }

        this.gameObject.SetActive(false);
        joystick.SetActive(true);
    }
}
