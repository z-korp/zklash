using System.Collections.Generic;
using UnityEngine;
using System;


public class DroppableManager : MonoBehaviour
{
    private GameObject[] zones;
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
        return a.transform.position.x.CompareTo(b.transform.position.x);
    }
}