using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enermy_Cash : MonoBehaviour
{
    public GameObject motion1;
    public GameObject motion2;
    public float animationTime = 1.0f;

    public float Speed = 50.0f;
    private Transform myTransform = null;

    bool isFell = false;

    Vector2 RandSpawnPos;
    Vector2 RandFallPos;
    //RandDestination = new Vector2(Random.Range(-162, 156), Random.Range(-355, 286));

    public int spawnTopHeight = 400;
    public int spawnLowHeight = -400;

    void Start()
    {
        SetPosNSpeed();
        myTransform = GetComponent<Transform>();
        this.transform.position += new Vector3(Random.Range(0, 50), 0, 0);
        StartCoroutine(AnimationCat());
    }

    IEnumerator AnimationCat()
    {
        if (motion1.activeSelf)
        {
            motion1.SetActive(false);
            motion2.SetActive(true);
        }
        else
        {
            motion1.SetActive(true);
            motion2.SetActive(false);
        }
        yield return new WaitForSeconds(animationTime);
        StartCoroutine(AnimationCat());
    }

    void SetPosNSpeed()
    {
        int randX = Random.Range(-162, 156);
        RandSpawnPos = new Vector2(randX, spawnTopHeight + Random.Range(0, 50));
        RandFallPos = new Vector2(randX, spawnLowHeight);
        this.transform.position = RandSpawnPos;
        isFell = false;
        Speed = Random.Range(5, 15);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFell)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, RandFallPos, Speed);
        }
        if(this.transform.position.y <= spawnLowHeight)
        {
            isFell = true;
            SetPosNSpeed();
        }
    }
}
