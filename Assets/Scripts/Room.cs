using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using EasyGraph;

public class Room 
{

   // public Function RoomFunction;
    public Vector3 CentrePoint;
    public int Area;//== amount of voxels
    public List<Voxel> Voxels;
    public List<Room> Neighbours;
    public GameObject GONode;
    private Vector3 scaleChange, positionChange;
    public Function SelectedFunction { get; private set; }
    VoxelGrid _voxelGrid;
    public Function _selectedFunction;
    

    #region Constructor

    public Room(VoxelGrid grid, List<Voxel> voxels, Function function)
    {
        _voxelGrid = grid;
        Voxels = voxels;
        SelectedFunction = function;

        GONode = Resources.Load<GameObject>("Prefabs/GONode");
        
        var roomNode = GONode.AddComponent<RoomNode>();
        roomNode.ThisRoom = this;


        float avgX = (float)voxels.Average(v => v.Index.x);
        float avgY = (float)voxels.Average(v => v.Index.y);
        float avgZ = (float)voxels.Average(v => v.Index.z);

        CentrePoint = new Vector3(avgX, avgY, avgZ);

        ////////Create Sphere at origin
        ////////Create Sphere(Node) for each function
        //var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        var go = GameObject.Instantiate(GONode);
        //GONode = go.GetComponents<Material>(function);
        go.transform.position = CentrePoint + Voxels[0]._voxelGrid.Origin * Voxels[0]._voxelGrid.VoxelSize;
       

        ////Set minimum - maximum value for each function
        if (function != Function.Connector && 
             GameObject.Instantiate(GONode))
        {
            GONode.transform.position = CentrePoint + Voxels[0]._voxelGrid.Origin * Voxels[0]._voxelGrid.VoxelSize;

            if(SelectedFunction == Function.Bathroom)
            {
                scaleChange = new Vector3(1.5f, 2.4f, 1.5f);                     // convert to range
            }
            else if (SelectedFunction == Function.Bedroom)
            {
                scaleChange = new Vector3(4, 3, 4);
            }
            else if (SelectedFunction == Function.Closet)
            {
                scaleChange = new Vector3(2, 2, 2);
            }
            else if (SelectedFunction == Function.Dining)
            {
                scaleChange = new Vector3(2.5f, 3, 2.5f);
            }
            else if (SelectedFunction == Function.LivingRoom)
            {
                scaleChange = new Vector3(3.7f, 5.5f, 3.7f);
            }
            else if (SelectedFunction == Function.Kitchen)
            {
                scaleChange = new Vector3(3.4f, 3, 3.4f);
            }
            else 
            {
                scaleChange = new Vector3(5, 5, 5);
            }
        }

        GONode.GetComponent<MeshRenderer>().material = _voxelGrid.FunctionColors[SelectedFunction];


    }

    #endregion

    #region Public Methods

    public void AddRoomToVoxels()
    {
        foreach (var voxel in Voxels)
        {
            voxel.InRoom = this;
        }
    }


    //public List<Room> SetRoomsFromSaved(JsonScene scene)
    //{
    //    // Get the voxels indexes from the saved scene
    //    // set the according voxels to be part of a room
    //    List<Room> result = new List<Room>();
    //    foreach (var jvoxel in scene.JsonVoxels)
    //    {
    //        var index = jvoxel.Index;
    //        var voxel = _voxelGrid.Voxels[index.x, index.y, index.z];
    //        voxel.IsActive = jvoxel.IsActive;
    //        var room = (Room)Enum.Parse(typeof(Room), jvoxel.VoxelFunction);
    //        voxel.InRoom = room;
    //        result.Add(room);
    //    }
    //    return result;
    //}


    #endregion
}