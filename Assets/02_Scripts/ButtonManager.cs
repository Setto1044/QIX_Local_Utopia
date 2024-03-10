using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [Header("Load Stage by Comoaring this String and Stage Name")]
    public static string currStage = string.Empty;

    public LoadManager loadManager;
    public GameObject StartMenu;
    public GameObject PopUpMenu;
    public GameObject StageMenu;
    public GameObject ResetPopUp;
    public GameObject SettingPopUp;

    public AudioSource BtnAudio;

    public void OnClickStartBtn()
    {
        BtnAudio.Play();
        StartMenu.SetActive(false);
        PopUpMenu.SetActive(true);
    }
    public void OnClickPopup()
    {
        BtnAudio.Play();
        PopUpMenu.SetActive(false);
        StageMenu.SetActive(true);
    }

    public void OnClickResetBtn()
    {
        BtnAudio.Play();
        ResetPopUp.SetActive(true);
    }

    public void OnClickResetYes()
    {
        BtnAudio.Play();
        for (int i = 0; i < 6; i++)
        {
            PlayerPrefs.SetInt(loadManager.stageNames[i], 0);
        }
        //PlayerPrefs.DeleteAll();
        for (int i = 0; i < loadManager.clearStamps.Length; i++)
        {
            loadManager.clearStamps[i].SetActive(false);
        }
        ResetPopUp.SetActive(false);
    }

    public void OnClickResetNo()
    {
        BtnAudio.Play();
        ResetPopUp.SetActive(false);
    }


    public void OnClickJeje()
    {
        BtnAudio.Play();
        Debug.Log("Jeje");
        currStage = "Jeje";
        SceneManager.LoadScene("QixGameScene");
    }

    public void OnClickDani()
    {
        BtnAudio.Play();
        Debug.Log("Dani");
        currStage = "Dani";
        SceneManager.LoadScene("QixGameScene");
    }

    public void OnClickSonko()
    {
        BtnAudio.Play();
        Debug.Log("Sonko");
        currStage = "Sonko";
        SceneManager.LoadScene("QixGameScene");
    }

    public void OnClickLGH()
    {
        BtnAudio.Play();
        Debug.Log("LGH");
        currStage = "LGH";
        SceneManager.LoadScene("QixGameScene");
    }

    public void OnClickNamu()
    {
        BtnAudio.Play();
        Debug.Log("Namu");
        currStage = "Namu";
        SceneManager.LoadScene("QixGameScene");
    }

    public void OnClickCreditBtn()
    {
        BtnAudio.Play();
        SceneManager.LoadScene("Credit");
    }

    public void OnClickBack2MenuBtn()
    {
        BtnAudio.Play();
        SettingPopUp.SetActive(false);
    }

    public void OnClickSettingBtn()
    {
        BtnAudio.Play();
        SettingPopUp.SetActive(true);
    }

    public void OnClickPopupMenuBack()
    {
        BtnAudio.Play();
        PopUpMenu.SetActive(false);
        StartMenu.SetActive(true);
    }

    public void OnClickStageMenuBack()
    {
        BtnAudio.Play();
        StageMenu.SetActive(false);
        StartMenu.SetActive(true);
    }

}