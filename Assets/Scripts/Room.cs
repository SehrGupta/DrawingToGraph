using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room
{
    public Function RoomFunction;
    public Vector3 CentrePoint;
    public int Area;//== amount of voxels
    public List<Voxel> Voxels;
    public List<Room> Neighbours;
    public GameObject GONode;
    private Vector3 scaleChange, positionChange;
    private Function _selectedFunction;


    #region Constructor

    public Room(List<Voxel> voxels)
    {
        Voxels = voxels;
        foreach (var voxel in Voxels)
        {
            voxel.InRoom = this;

        }

        float avgX = (float)voxels.Average(v => v.Index.x);
        float avgY = (float)voxels.Average(v => v.Index.y);
        float avgZ = (float)voxels.Average(v => v.Index.z);

        CentrePoint = new Vector3(avgX, avgY, avgZ);

        ////Create Sphere(Node) for each function
        //var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        var go = GameObject.Instantiate(GONode);
        go.transform.position = CentrePoint + Voxels[0]._voxelGrid.Origin * Voxels[0]._voxelGrid.VoxelSize;

        ////Set minimum - maximum value for each function
        //scaleChange =  new Vector3 (,,)

    }

    public void Awake()
    {

    }

    public void Update()
    {
        ////Take percentage from scale of space drawn by user
        ///assign percentage to space based on its minimum - maximum value based on scale by user
    }


    

    #endregion
}