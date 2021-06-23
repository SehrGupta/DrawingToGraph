using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Enable : MonoBehaviour
{

    public static event Action<bool> LevelLoaded = delegate { };
    public Button LevelButtons;
    public Button LoadLevel1;
    public Button LoadLevel2;
    public Button LoadLevel3;
    public Button CreateLevel3;
    public Button CreateLevel2;
    //public Button CreateLevel1;
    private bool CreateLevel1;
   

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine (ButtonOn());

    }

    // Update is called once per frame
    void Update()
    {
        void ButtonOn()
        {
            if (CreateLevel1 == true && CreateLevel2.interactable == false)
            {
                //allows button to be used
                CreateLevel2.interactable = true;
            }
        }
        
    }

    /*IEnumerator ButtonOn()
    {
        yield return new WaitForSeconds(10f);

        CreateLevel2 = GameObject.FindGameObjectWithTag("CreateLevel2");
        CreateLevel2.GetComponent<Button>().interactable = true;
    }*/


    /*private void ButtonClicked()
    {
        levelUpdated = true;
        LevelLoaded(levelUpdated);
    }*/
}
