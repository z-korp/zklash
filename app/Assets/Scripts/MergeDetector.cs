using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeDetector : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Mob"))
        {
            Debug.Log(gameObject.name + " a collisionné avec " + collision.gameObject.name);
        }
    }

}