using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anzol : MonoBehaviour
{
    void Start()
    {
        // hide mouse
        Cursor.visible = false;
    }

    void Update()
    {
        // the position of this is equal to the mouse position
        var camPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(camPos.x, camPos.y, 0);
    }
}
