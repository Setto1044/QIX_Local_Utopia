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

    [Header("Each Stages")] // 모든 스테이지 Gameobject는 하이라키 창에서 전부 SetActive(false)여야 함 
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
    // 게임 시작 전 설명 캔버스 띄워지는 시간만큼 게임 타이머 정지
    IEnumerator WaitTimerCoroutine()
    {
        yield return new WaitForSeconds(5);
        StartCoroutine(TimerCoroutine());
    }

    // 타이머
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
        // 영역 먹은 퍼센트 게이지 계산 및 처리 함수
        GageSetter();

        //퍼센트 텍스트, 타이머 텍스트 처리
        Percent.text = player.completePercent.ToString() + "%";
        Timer.text = timeLimit.ToString();

        // CompletePercent보다 많은 땅 차지 시 게임 성공
        if(player.completePercent >= CompletePercent)
        {
            // 성공음
            if (!gameEnd)
            {
                Complete.Play();
            }
            // 게임 끝 변수 활성, 안먹은 땅을 모두 먹은 땅으로 변경, 조이스틱 비활성화
            gameEnd = true;
            player.ClearImageViewer();
            Joystick.SetActive(false);

            // 보너스 스테이지가 아니라면 클리어 팝업 띄운 후 메인 화면으로
            if(ButtonManager.currStage != "Bonus")
            {
                ClearPopup.SetActive(true);
            }
            else
            {
                PlayerPrefs.SetInt("Bonus", 1);
                BonusClearPopup.SetActive(true);
            }

            // 플레이어 프리펩에 기록
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

        // 자기 선밟을 때 게임 종료 --> 취소
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

        // 적과 충돌 시
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


    // 먹은 땅 퍼센트 바 및 수치 계산
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
