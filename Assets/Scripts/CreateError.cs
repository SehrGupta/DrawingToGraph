using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine;

public class CreateError : MonoBehaviour
{
    //public GameObject errorWindow;
    //Text errorText;
    //InputField myField;
    public static event Action<bool> Error = delegate { };
    public GameObject SaveandReturn;
    public GameObject MakeError;
    //public Animator animator;
    //public TMP_Text popUpText;

    /*public void OnStoppedEditing(string text)
    {
        errorText.text = "Mark Staircase on one of your space";
    }
    // Start is called before the first frame update
    void Start()
    {
        //adds a listener that runs OnStoppedEditing when editing is stopped 
        myField.onEndEdit.AddListener(delegate { OnStoppedEditing(errorText.text); });

        myField = gameObject.GetComponent<InputField>();
        errorText = errorWindow.GetComponent<Text>();

        //userOutline = InputUser.AddComponent<Outline>();
        //userOutline.effectColor = Color.red;
        //userOutline.effectDistance = new Vector2(3, -3);
    }*/
    /*public void Error(string text)
    {
        MakeError.SetActive(true);
        animator.SetTrigger("SaveandReturn");
    }*/
    /*public void OnClick()
    {
         //Gameobject current = ActionManager
    }*/
    public void ButtonClick()
    {
        if (SaveandReturn == true && MakeError.activeInHierarchy == false)
        {
            //allows button to be used
            MakeError.active = true;
        }
    }
    public void Update()
    {
        
    }
    /*public void Error()
    {
        if (SaveandReturn.activeInHierarchy == true)
        {
            MakeError.SetActive(false);
        }
        else MakeError.SetActive(true);
    }*/



}
