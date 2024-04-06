using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System;

public class EntitySync : MonoBehaviour
{
  public string entity;

  void Update()
  {
    if (string.IsNullOrEmpty(entity))
    {
      entity = Guid.NewGuid().ToString();
    }
  }


}