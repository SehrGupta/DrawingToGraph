using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    /*void Start()
    {
        GameObject thePlayer = GameObject.Find("drawing.png");
    }*/

    public void LoadScene(string Staircase)
    {
        SceneManager.LoadScene(Staircase);
    }
}
