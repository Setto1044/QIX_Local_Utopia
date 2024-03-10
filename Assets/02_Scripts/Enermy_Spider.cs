using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enermy_Spider : MonoBehaviour
{
    public GameObject Web;

    public float Speed = 20.0f;
    private Transform myTransform = null;

    Vector2 RandDestination;

    public int minx;
    public int maxx;
    public int miny;
    public int maxy;

    void Start()
    {
        myTransform = GetComponent<Transform>();
        StartCoroutine(MoveEnermy());
        StartCoroutine(WebSpawn());
    }

    IEnumerator MoveEnermy()
    {
        RandDestination = new Vector2(Random.Range(minx, maxx), Random.Range(miny, maxy));
        yield return new WaitForSeconds(Random.Range(1, 4));
        StartCoroutine(MoveEnermy());
    }

    IEnumerator WebSpawn()
    {
        Instantiate(Web, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z-1) , Quaternion.identity);
        yield return new WaitForSeconds(Random.Range(5, 10));
        StartCoroutine(WebSpawn());
    }


    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector2.MoveTowards(this.transform.position, RandDestination, Speed);
    }
}
