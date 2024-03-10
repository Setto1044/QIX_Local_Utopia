using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingScriptSeq : MonoBehaviour
{
    public GameObject NextBtns;
    public AudioSource SelectSound;
    public void Next()
    {
        SelectSound.Play();
        NextBtns.SetActive(true);
        this.gameObject.SetActive(false);
    }
    public void GoMenu()
    {
        SelectSound.Play();
        SceneManager.LoadScene(0);
    }

    public void LastBtn()
    {
        SelectSound.Play();
        SceneManager.LoadScene("Credit");
    }
}
