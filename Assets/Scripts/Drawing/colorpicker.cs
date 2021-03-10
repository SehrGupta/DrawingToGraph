using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorpicker : MonoBehaviour
{
    
    // To capture colour to be picked
    void OnMouseDown()
    {
        Draw.currentColor = GetComponent<SpriteRenderer>().color;
        Debug.Log(Draw.currentColor);
    }
}
