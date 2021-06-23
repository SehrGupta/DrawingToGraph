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
        
        

        /*LevelButtons = GetComponent<Button>();
        LoadLevel1 = GetComponent<Button>();
        LoadLevel2 = GetComponent<Button>();
        LoadLevel3 = GetComponent<Button>();
        CreateLevel1 = GetComponent<Button>();
        CreateLevel2 = GetComponent<Button>();
        CreateLevel3 = GetComponent<Button>();*/
        
        //LevelButtons.interactable = false;
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



    public void CreateNewScene()
    {
        SceneManager.LoadScene("APP");
    }

}
