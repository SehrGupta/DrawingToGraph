using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine;

public class CreateError : MonoBehaviour
{
    public GameObject errorWindow;
    Text errorText;
    InputField myField;
    
    public void OnStoppedEditing(string text)
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
