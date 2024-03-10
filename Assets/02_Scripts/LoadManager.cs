using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public GameObject[] clearStamps;
    public string[] stageNames = { "Jeje", "Dani", "Sonko", "LGH", "Namu", "Bonus" };
    int cnt = 0;
    public static bool isClearBonus = false;

    void Start()
    {
        cnt = 0;
        for (int i = 0; i < clearStamps.Length; i++)
        {
            if (PlayerPrefs.GetInt(stageNames[i]) == 1)
            {
                clearStamps[i].SetActive(true);
                cnt++;
            }
        }
        if (cnt == 5 && PlayerPrefs.GetInt("Bonus") == 0)
        {
            ButtonManager.currStage = "Bonus";
            SceneManager.LoadScene("QixGameScene");
        }
    }

}