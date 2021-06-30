using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject LevelButtons;
    public GameObject LoadLevel1;
    public GameObject LoadLevel2;
    public GameObject LoadLevel3;
    public GameObject CreateLevel3;
    public GameObject CreateLevel2;
    public GameObject CreateLevel1;




    private void Start()
    {
        var levelButtons = GameObject.Find("LevelButtons").transform;

        var create1 = levelButtons.Find("CL1");
        var create2 = levelButtons.Find("CL2");
        var create3 = levelButtons.Find("CL3");
        var load1 = levelButtons.Find("LL1");
        var load2 = levelButtons.Find("LL2");
        var load3 = levelButtons.Find("LL3");

        create1.gameObject.SetActive(true);
        load1.gameObject.SetActive(SessionManager.CreatedScenes.ContainsKey(1));

        create2.gameObject.SetActive(SessionManager.CreatedScenes.ContainsKey(1));
        load2.gameObject.SetActive(SessionManager.CreatedScenes.ContainsKey(2));

        create3.gameObject.SetActive(SessionManager.CreatedScenes.ContainsKey(2));
        load3.gameObject.SetActive(SessionManager.CreatedScenes.ContainsKey(3));

    }




    //Get and Update Buttons

    public void ButtonLevel1()
    {

        if (LoadLevel1.activeInHierarchy == true)
        {
            LoadLevel1.SetActive(false);
        }
        else LoadLevel1.SetActive(true);

        if (CreateLevel2.activeInHierarchy == true)
        {
            CreateLevel2.SetActive(false);
        }
        else CreateLevel2.SetActive(true);
    }

    public void ButtonLevel2()
    {
        if (LoadLevel2.activeInHierarchy == true)
        {
            LoadLevel2.SetActive(false);
        }
        else LoadLevel2.SetActive(true);

        if (CreateLevel3.activeInHierarchy == false)
        {
            CreateLevel3.SetActive(true);
        }
        else CreateLevel3.SetActive(false);
    }

    public void ButtonLevel3()
    {
        if (LoadLevel3.activeInHierarchy == true)
        {
            LoadLevel3.SetActive(false);
        }
        else LoadLevel3.SetActive(true);
    }

    public void Buttons()
    {
        GetComponent<Button>().interactable = false;
        //if (level > 1 && !previousCleared)
        //{
        ///    levelButton.interactable = false;
        // }

    }

    public void LoadScene(int level)
    {
        SessionManager.CurrentLevel = level;
        SceneManager.LoadScene("APP");
    }

    public void CreateNewScene(int level)
    {
        SessionManager.CurrentLevel = level;
        SceneManager.LoadScene("APP");
    }

}
