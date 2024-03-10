using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditMove : MonoBehaviour
{
    public int scaler = 800;
    public float speed = 1f;


    // Update is called once per frame
    void Update()
    {
        if( this.transform.localPosition.y >= scaler)
        {
            this.transform.localPosition -= new Vector3(0, 2 * scaler, 0);
        }
        this.transform.Translate(Vector2.up * speed);
    }
}
