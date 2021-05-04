using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{

  // Code referenced from DitzelGames, created on April 11,2018: Implement Drawing and Saving in Unity 2018 HD
  public GameObject Brush;
  public float BrushSize = 0.6f;
  public static string toolType;
  public static Color currentColor;
  public Input mousePosition;

  public Vector3Int v;
  //public new Vector3Int origin;
  private Vector3Int origin;
  


    // For Intiialization
    /*void Start()
    {
        GetComponent<SpriteRenderer> ().Color = Draw.currentColor;
        //SpriteRenderer sr = GetComponent<SpriteRenderer>();
        //Debug.Log(sr.bounds);

    }*/


    //For brush application 
    void Update()
    {

        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);  // added 9/3
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);                // added 9/3
        //origin = new Vector3Int(Mathf.RoundToInt(v.x), (v.y), (v.z));

        if (Input.GetMouseButton(0))
        {
            var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(Ray, out hit))
            {
                var go = Instantiate(Brush, hit.point + Vector3.up * 0.6f, Quaternion.identity, transform);
                go.transform.localScale = Vector3.one * BrushSize;
                
			    //var toolType = transform.InverseTransformPoint(hit.point); // Added 3/7
            }

            

        }

        

        




        // sprite has been drawn                                           // Reference code from David's discussion
        // get coordinate from sprite
        // get coordinate from mouse
        //v = Vector3;
        //origin = new Vector3Int(Mathf.RoundToInt(Vector3Int.x), (Vector3Int.y), (Vector3Int.z));
        //VoxelGrid.FillBucket(origin, currentColor);  
        /*v = Vector3;*/


    }

    /*private void FillBucket(Vector3Int origin)
    {
        //Vector3Int v = (160, 100, 1);
        origin = new Vector3Int(Mathf.RoundToInt(v.x), (v.y), (v.z));
        VoxelGrid.FillBucket(origin, objPosition);

    }*/
    /*private void Awake()                                              // Added 3/7
    {

        BoxCollider box = gameObject.AddComponent<BoxCollider>();
        box.size = new Vector3();
        //VoxelGrid.FillBucket(origin, Vector3);

    }*/




    /*IEnumerator FillBucket()
    {
        WaitForSeconds wait = new WaitForSeconds(0.05f);
    }

    public void StartFillBucket()
    {
        StartCoroutine(FillBucket);
    }*/


    //Input points to voxel coordinates
    /*private void toolType ();                                  // Added 3/7
    {
            int voxelX = (int)(origin.x /  currentColor);
            int voxelY = (int)(origin.y /  currentColor);
          
    }*/
}