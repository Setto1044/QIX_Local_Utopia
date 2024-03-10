using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoadManager : MonoBehaviour
{
    public GameObject GameCanvas;
    public int existTime = 3;
    void Start()
    {
        StartCoroutine(WaitForLoadGameMap());
    }

    IEnumerator WaitForLoadGameMap()
    {
        yield return new WaitForSeconds(existTime);
        GameCanvas.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
