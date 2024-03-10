using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyButtonSetter : MonoBehaviour
{
    Player player;
    public Button[] btns;
    int[] di = { -1, 1, 0, 0 };
    int[] dj = { 0, 0, -1, 1 };

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void FixedUpdate()
    {
        if (btns[0].GetComponent<BtnState>().pressed)
        {
            player.Move(di[2], dj[2]);
        }

        else if (btns[1].GetComponent<BtnState>().pressed)
        {
            player.Move(di[0], dj[0]);
        }
        else if (btns[2].GetComponent<BtnState>().pressed)
        {
            player.Move(di[1], dj[1]);
        }
        else if (btns[3].GetComponent<BtnState>().pressed)
        {
            player.Move(di[3], dj[3]);
        }
    }
}
