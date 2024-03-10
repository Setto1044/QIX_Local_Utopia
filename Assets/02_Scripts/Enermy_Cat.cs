using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enermy_Cat : MonoBehaviour
{
    public GameObject motion1;
    public GameObject motion2;
    public float animationTime = 1.0f;

    public float Speed = 50.0f;
    private Transform myTransform = null;

    Vector2 RandDestination;

    public int minx;
    public int maxx;
    public int miny;
    public int maxy;

    void Start()
    {
        myTransform = GetComponent<Transform>();
        StartCoroutine(AnimationCat());
        StartCoroutine(MoveEnermy());
    }

    IEnumerator AnimationCat()
    {
        if(motion1.activeSelf)
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

    IEnumerator MoveEnermy()
    {
        RandDestination = new Vector2(Random.Range(minx, maxx), Random.Range(miny, maxy));
        yield return new WaitForSeconds(Random.Range(1, 4));
        StartCoroutine(MoveEnermy());
    }


    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, RandDestination, Speed);
    }
}
