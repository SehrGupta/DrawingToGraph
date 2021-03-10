using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dotcontrol : MonoBehaviour
{
    //Reference from Design & Deploy (Unity 5 - How to Make a Paint Program - Part 1)    

    // Start is called before the first frame update
    void Start()
    {
       GetComponent<SpriteRenderer>().color = Draw.currentColor; 
    }
}
