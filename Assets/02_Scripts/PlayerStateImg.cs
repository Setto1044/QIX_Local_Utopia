using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateImg : MonoBehaviour
{
    public Sprite safe; 
    public Sprite adventure; 
    public Sprite web; 
    public void PlayerSafe()
    {
        this.GetComponent<SpriteRenderer>().sprite = safe;
    }
    public void PlayerAdventure()
    {
        this.GetComponent<SpriteRenderer>().sprite = adventure;
    }
    public void Playerweb()
    {
        this.GetComponent<SpriteRenderer>().sprite = web;
    }
}
