using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;


public class Draggable : MonoBehaviour
{
    bool drag;

    private void OnMouseDown()
    {
        Debug.Log("Mouse down");
        drag = true;
    }

    private void OnMouseUp()
    {
        Debug.Log("Mouse up");
        drag = false;
    }

    private void Update()
    {
        if (drag)
        {
            Debug.Log("Dragging");
            Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(MousePos);

        }
    }

}
