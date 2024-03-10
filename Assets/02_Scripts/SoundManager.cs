using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    public AudioSource bgmAudio;
    public AudioSource effAudio;

    public GameObject bgm;
    public GameObject eff;

    void Start()
    {
        if (PlayerPrefs.GetInt("bgm") == 1)
        {
            bgm.GetComponent<Text>().text = "ON";
            bgmAudio.mute = false;
        }
        else
        {
            bgm.GetComponent<Text>().text = "OFF";
            bgmAudio.mute = true;
        }
        
        if (PlayerPrefs.GetInt("eff") == 1)
        {
            eff.GetComponent<Text>().text = "ON";
            effAudio.mute = false;
        }
        else
        {
            eff.GetComponent<Text>().text = "OFF";
            effAudio.mute = true;
        }

        
    }

    public void BGM()
    {
        if (PlayerPrefs.GetInt("bgm") == 0)
        {
            PlayerPrefs.SetInt("bgm", 1);
            bgm.GetComponent<Text>().text = "ON";
            bgmAudio.mute = false;
        }
        else
        {
            PlayerPrefs.SetInt("bgm", 0);
            bgm.GetComponent<Text>().text = "OFF";
            bgmAudio.mute = true;
        }
    }

    public void EFF()
    {
        if (PlayerPrefs.GetInt("eff") == 0)
        {
            PlayerPrefs.SetInt("eff", 1);
            eff.GetComponent<Text>().text = "ON";
            effAudio.mute = false;
            effAudio.Play();
        }
        else
        {
            PlayerPrefs.SetInt("eff", 0);
            eff.GetComponent<Text>().text = "OFF";
            effAudio.mute = true;
        }
    }

    
}
