using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeDetector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Vous pouvez utiliser le tag pour vous assurer que vous réagissez seulement aux collisions avec des objets spécifiques
        if (collision.transform.CompareTag("Mob")) // Remplacez "VotreTagCible" par le tag approprié
        {
            Debug.Log(gameObject.name + " a collisionné avec " + collision.gameObject.name);
        }
    }

}