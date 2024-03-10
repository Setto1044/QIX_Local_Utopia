using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GIFSetter : MonoBehaviour
{
    GameObject square;
    public GameObject clearPopup;
    public Sprite[] gifFrames;
    public float gifsec = 0.05f;
    bool doOnce = false;

    void Start()
    {
        square = this.transform.GetChild(0).gameObject;
    }
    void Update()
    {
        if (clearPopup.activeSelf && !doOnce)
        {
            square.SetActive(true);
            StartCoroutine(GIFStart());
            doOnce = true;
        }

    }

    IEnumerator GIFStart()
    {
        for (int i = 0; i < gifFrames.Length; i++)
        {
            square.GetComponent<SpriteRenderer>().sprite = gifFrames[i];
            yield return new WaitForSeconds(gifsec);
        }
        yield return new WaitForSeconds(2);
        StartCoroutine(GIFStart());
    }
}
