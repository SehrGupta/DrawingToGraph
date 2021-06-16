using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Function RoomFunction;
    public Vector3 CentrePoint;
    public int Area;//== amount of voxels
    public List<Voxel> Voxels;
    public List<Room> Neighbours;
    public GameObject GONode;
    private Vector3 scaleChange, positionChange;
    private Function _selectedFunction;
    VoxelGrid _voxelGrid;


    #region Constructor
    public void Start()
    {
        GONode = Resources.Load<GameObject>("Prefabs/GONode") ;
    }
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

        ////////Create Sphere at origin
        ////////Create Sphere(Node) for each function
        //var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //var go = GameObject.Instantiate(GONode);
        //go.transform.position = CentrePoint + Voxels[0]._voxelGrid.Origin * Voxels[0]._voxelGrid.VoxelSize;

        ////Set minimum - maximum value for each function
        if (GameObject.CreatePrimitive(PrimitiveType.Sphere))
        {
            GONode.transform.position = CentrePoint + Voxels[0]._voxelGrid.Origin * Voxels[0]._voxelGrid.VoxelSize;

            if(_selectedFunction == Function.Bathroom)
            {
                scaleChange = new Vector3(1.5f, 2.4f, 1.5f);                     // convert to range
            }
            else if (_selectedFunction == Function.Bedroom)
            {
                scaleChange = new Vector3(4, 3, 4);
            }
            else if (_selectedFunction == Function.Closet)
            {
                scaleChange = new Vector3(2, 2, 2);
            }
            else if (_selectedFunction == Function.Dining)
            {
                scaleChange = new Vector3(2.5f, 3, 2.5f);
            }
            else if (_selectedFunction == Function.LivingRoom)
            {
                scaleChange = new Vector3(3.7f, 5.5f, 3.7f);
            }
            else if (_selectedFunction == Function.Kitchen)
            {
                scaleChange = new Vector3(3.4f, 3, 3.4f);
            }
            else 
            {
                scaleChange = new Vector3(5, 5, 5);
            }
        }
        

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