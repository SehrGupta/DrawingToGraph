using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ActivateConnector : MonoBehaviour
{
    public GameObject targetObject;
    //public Room Source;
    //public Room End;

    Vector2 Source;
    Vector2 End;


    static Material _lineMaterial;

    public void Start()
    {
        targetObject.SetActive(false);
    }

    /*private void OnTriggerStay(Collider other)
    {
        targetObject.GetComponent<BoxCollider>().isTrigger = true;
    }*/

    void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Connector"))
        {
            targetObject.SetActive(true);
        }
    }

   

}
