using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnState : MonoBehaviour
{
    public bool pressed = false;

    public void Press()
    {
        pressed = true;
    }
    public void NotPress()
    {
        pressed = false;
    }
}
