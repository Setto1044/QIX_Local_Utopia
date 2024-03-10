using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enermy_Foot : MonoBehaviour
{
    public GameObject L_foot;
    public GameObject R_foot;
    public float animationTime = 0.5f;

    public float Speed = 20.0f;
    private Transform myTransform = null;

    Vector2 RandDestination;
    Vector2 bigFoot;
    Vector2 middleFoot;
    Vector2 smallFoot;

    public int minx;
    public int maxx;
    public int miny;
    public int maxy;

    void Awake()
    {
        bigFoot = new Vector2(L_foot.transform.localScale.x * 1.5f, L_foot.transform.localScale.y * 1.5f);
        middleFoot = new Vector2(L_foot.transform.localScale.x , L_foot.transform.localScale.y);
        smallFoot = new Vector2(L_foot.transform.localScale.x * 0.5f, L_foot.transform.localScale.y * 0.75f);
    }

    void Start()
    {
        myTransform = GetComponent<Transform>();
        StartCoroutine(AnimationFoot(0));
        StartCoroutine(MoveEnermy());
    }

    IEnumerator AnimationFoot(int sequence)
    {
        switch (sequence)
        {
            //big left
            case 0:
                L_foot.transform.localScale = bigFoot;
                R_foot.transform.localScale = smallFoot;
                break;
            //same size
            case 1:
                L_foot.transform.localScale = middleFoot;
                R_foot.transform.localScale = middleFoot;
                break;
            case 2:
                L_foot.transform.localScale = smallFoot;
                R_foot.transform.localScale = bigFoot;
                break;
            case 3:
                L_foot.transform.localScale = middleFoot;
                R_foot.transform.localScale = middleFoot;
                break;
        }

        yield return new WaitForSeconds(animationTime);
        StartCoroutine(AnimationFoot(++sequence%4));
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
