using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEgg : MonoBehaviour
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
        StartCoroutine(AnimationCoroutine());
        StartCoroutine(MoveX());
    }
    IEnumerator AnimationCoroutine()
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
        StartCoroutine(AnimationCoroutine());
    }
    IEnumerator MoveX()
    {
        RandDestination = new Vector2(Random.Range(minx, maxx), this.gameObject.transform.position.y);
        if(RandDestination.x < this.transform.position.x)
        {
            motion1.transform.localScale = new Vector3(-1, 1, 1);
            motion2.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            motion1.transform.localScale = new Vector3(1, 1, 1);
            motion2.transform.localScale = new Vector3(1, 1, 1);
        }

        yield return new WaitForSeconds(Random.Range(3, 5));
        StartCoroutine(MoveX());
    }


    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, RandDestination, Speed);
    }
}
