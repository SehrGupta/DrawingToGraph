using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coroutine : MonoBehaviour
{

    // Reference from Alexander Zotov (How to Slowly Fade from one color to another using Coroutine in Unity? Simple 2D tutorial)


    // Reference to SpriteRenderer component
    SpriteRenderer rend;

    // Variable to hold value to fade down to
    // Can be adjusted in inspector with slider
    [Range(0f, 1f)]
    
    public float fadeToWhiteAmount = 0f;

    // Variable to hold fading speed
    public float fadingSpeed = 0.05f;

    //For initiation
    void Start()
    {
        // Getting SpriteRenderer component
        rend = GetComponent<SpriteRenderer> ();

        // Getting access to colours
        Color c = rend.material.color;

        //Setting initial values for White channel
        c.r = 1f;

        // Set Sprite Color
        rend.material.color = c;
    }

    // Fadning to desirable colour
    IEnumerator FadeToWhite ()
    {
        // Loop for color amount
        for (float i = 1f; i>= fadeToWhiteAmount; i -= 0.05f)
        {
            // Getting access to colour
            Color c = rend.material.color;

            // Set colour to SpriteRenderer
            rend.material.color = c;

            // Pause for colour transition
            yield return new WaitForSeconds (fadingSpeed);
        }
    }

    // To Start fading with UI
    public void StartFadeToWhite()
    {
        StartCoroutine ("FadeToWhite");
    }
}
