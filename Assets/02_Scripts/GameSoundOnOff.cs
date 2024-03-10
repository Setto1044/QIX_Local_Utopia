using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSoundOnOff : MonoBehaviour
{
    public AudioSource[] EFF;
    public AudioSource[] BGM;
    void Start()
    {
        if( PlayerPrefs.GetInt("eff") == 1)
        {
            for (int i = 0; i < EFF.Length; i++)
            {
                EFF[i].mute = false;
            }
            
        }
        else
        {
            for (int i = 0; i < EFF.Length; i++)
            {
                EFF[i].mute = true;
            }
            
        }
        if (PlayerPrefs.GetInt("bgm") == 1)
        {
            for (int i = 0; i < BGM.Length; i++)
            {
                BGM[i].mute = false;
            }
        }
        else
        {
            for (int i = 0; i < BGM.Length; i++)
            {
                BGM[i].mute = true;

            }
        }
    }
}
