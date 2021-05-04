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

    #endregion

    #region Button
    public void OnButtonPress()
    {
        n++;
        Debug.Log("Done" + n + "times.");
    }

    #endregion

    #region Constructors

    
    public class Room
    {
        public Function RoomFunction;
        public Vector3 CentrePoint;
        public int Area;//== amount of voxels
        public List<Voxel> Voxels;

        public void AnalyseRoom()
        {
            //Get centrepoint from voxel list
        }

    }
   

    #endregion

}
