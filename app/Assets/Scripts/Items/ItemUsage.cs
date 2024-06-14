using System.Collections;
using UnityEngine;
using TMPro;

public class ItemUsage : MonoBehaviour
{
    public ItemData item;
    
    void Start()
    {
        Debug.Log("Start item");
    }

    public void Consume()
    {
        Debug.Log("Consuming item");
    }
}
