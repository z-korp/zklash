using System.Collections.Generic;
using UnityEngine;
using System;


public class DroppableManager : MonoBehaviour
{
    public GameObject[] zones;
    public static DroppableManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of DroppableManager found!");
            return;
        }
        instance = this;

        // Sort Droppable zones by X position
        zones = GameObject.FindGameObjectsWithTag("DroppableZone");
        Array.Sort(zones, new Comparer());
    }

    public int IndexOf(DroppableZone _zone)
    {
        for (int i = 0; i < zones.Length; i++)
        {
            DroppableZone zone = zones[i].GetComponent<DroppableZone>();
            if (zone == _zone)
            {
                return i;
            }
        }
        return -1;
    }
}

public class Comparer : IComparer<GameObject>
{
    public int Compare(GameObject a, GameObject b)
    {

        if (a.transform.position.x < b.transform.position.x)
        {
            return 1;
        }
        else if (a.transform.position.x > b.transform.position.x)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
}