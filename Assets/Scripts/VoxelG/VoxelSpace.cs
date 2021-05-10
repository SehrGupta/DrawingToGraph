using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelSpace : MonoBehaviour
{
    #region fields 

    public Voxel Voxels;
    public Function Function;
    int n;
    VoxelGrid _voxelGrid;
    public GameObject GONode;
    public GameObject Done;

    #endregion

    #region Button
   void Update()
    {
        n++;
        var done = GameObject.Find("Plane/Canvas/Done");
       /* if (Input.GetKeyDown(Plane.Canvas.Done))
            EnvironmentManager.GetAllRooms();*/
        Debug.Log("Done" + n + "times.");
    }

    void CreateNode()
    {
        var node = GameObject.Find("Node");
    }

    #endregion

    #region Constructors

    
   
   

    #endregion

}
