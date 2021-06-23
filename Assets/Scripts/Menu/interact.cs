using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine;

public class interact : MonoBehaviour
{
    private Button thisButton;

    // Start is called before the first frame update
    void Start()
    {
        Enable.LevelLoaded += TurnButtonOn;
        thisButton = GetComponent<Button>();
    }

    private void TurnButtonOn(bool isOn)
    {
        thisButton.interactable = isOn;
    }
}
