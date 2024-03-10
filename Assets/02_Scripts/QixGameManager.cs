using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QixGameManager : MonoBehaviour
{
    [Header("UI Text")]
    public Text Percent;
    public Text Timer;

    [Header("Time Limit")]
    public int timeLimit = 120;
    
    [Header("Complete Percent to Win the Game")]
    public int CompletePercent = 80;

    [Header("Player Script")]
    public Player player;

    [Header("Popups")]
    public GameObject ClearPopup;
    public GameObject BonusClearPopup;
    public GameObject villainOverPopup;
    public GameObject LineCrossOverPopup;

    [Header("Audio Sources")]
    public AudioSource Complete;
    public AudioSource Fail;
    public AudioSource Select;

    int prevPercent = 0;

    [Header("Each Stages")] // ��� �������� Gameobject�� ���̶�Ű â���� ���� SetActive(false)���� �� 
    public GameObject[] Stages;

    [Header("GageBars (10%)")]
    public GameObject[] gagebar;

    [Header("JoyStick UI Object")]
    public GameObject Joystick;
    bool gameEnd;


    void Awake()
    {
        switch (ButtonManager.currStage)
        {
            case "Jeje":
                Stages[0].SetActive(true);
                break;
            case "Dani":
                Stages[1].SetActive(true);
                break;
            case "Sonko":
                Stages[2].SetActive(true);
                break;
            case "LGH":
                Stages[3].SetActive(true);
                break;
            case "Namu":
                Stages[4].SetActive(true);
                break;
            case "Bonus":
                Stages[5].SetActive(true);
                break;
        }
    }

    void Start()
    {
        StartCoroutine(WaitTimerCoroutine());
        gameEnd = false;
    }
    // ���� ���� �� ���� ĵ���� ������� �ð���ŭ ���� Ÿ�̸� ����
    IEnumerator WaitTimerCoroutine()
    {
        yield return new WaitForSeconds(5);
        StartCoroutine(TimerCoroutine());
    }

    // Ÿ�̸�
    IEnumerator TimerCoroutine()
    {
        if( timeLimit > 0 && !gameEnd)
        {
            yield return new WaitForSeconds(1);
            timeLimit--;
            StartCoroutine(TimerCoroutine());
        }
    }


    void Update()
    {
        // ���� ���� �ۼ�Ʈ ������ ��� �� ó�� �Լ�
        GageSetter();

        //�ۼ�Ʈ �ؽ�Ʈ, Ÿ�̸� �ؽ�Ʈ ó��
        Percent.text = player.completePercent.ToString() + "%";
        Timer.text = timeLimit.ToString();

        // CompletePercent���� ���� �� ���� �� ���� ����
        if(player.completePercent >= CompletePercent)
        {
            // ������
            if (!gameEnd)
            {
                Complete.Play();
            }
            // ���� �� ���� Ȱ��, �ȸ��� ���� ��� ���� ������ ����, ���̽�ƽ ��Ȱ��ȭ
            gameEnd = true;
            player.ClearImageViewer();
            Joystick.SetActive(false);

            // ���ʽ� ���������� �ƴ϶�� Ŭ���� �˾� ��� �� ���� ȭ������
            if(ButtonManager.currStage != "Bonus")
            {
                ClearPopup.SetActive(true);
            }
            else
            {
                PlayerPrefs.SetInt("Bonus", 1);
                BonusClearPopup.SetActive(true);
            }

            // �÷��̾� �����鿡 ���
            PlayerPrefs.SetInt(ButtonManager.currStage, 1);
        }

        //game time over
        if(timeLimit == 0)
        {
            if (!gameEnd)
            {
                Fail.Play();
            }
            gameEnd = true;
            Joystick.SetActive(false);
            villainOverPopup.SetActive(true);
            //SceneManager.LoadScene(0);
        }

        // �ڱ� ������ �� ���� ���� --> ���
        /*if (player.LineGameOver)
        {
            if (!gameEnd)
            {
                Fail.Play();
            }
            gameEnd = true;
            Joystick.SetActive(false);
            LineCrossOverPopup.SetActive(true);
        }*/

        // ���� �浹 ��
        if (player.VillainGameOver)
        {
            if (!gameEnd)
            {
                Fail.Play();
            }
            gameEnd = true;
            Joystick.SetActive(false);
            villainOverPopup.SetActive(true);
        }
    }


    // ���� �� �ۼ�Ʈ �� �� ��ġ ���
    void GageSetter()
    {
        if(prevPercent != player.completePercent)
        {
            prevPercent = player.completePercent;
            int gageIdx = prevPercent / 10;
            
            if(gageIdx >= 1)
            {
                for (int i = 0; i < gageIdx; i++)
                {
                    gagebar[i].SetActive(true);
                }
            }
        }
    }

    public void OnClickWinPopup()
    {
        Select.Play();
        SceneManager.LoadScene(0);
    }

    public void OnClickBonusWinPopup()
    {
        Select.Play();
        LoadManager.isClearBonus = true;
        PlayerPrefs.SetInt("Bonus", 1);
        SceneManager.LoadScene("Ending");
    }
}
