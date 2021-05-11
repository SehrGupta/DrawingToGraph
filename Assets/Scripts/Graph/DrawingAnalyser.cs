using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawingAnalyser
{

    private VoxelGrid _grid;
    [SerializeField]
    List<GameObject> _nodes;
    [SerializeField]
    List<Material> _materials;
    public Function Function;
    public GameObject VoxelCollider = null;
    public Vector3 CentrePoint;
    public int Area;//== amount of voxels
    public List<Voxel> Voxels;

    


    void Update ()
    {
        /* if (Input.GetMouseButton(0) && Physics.Raycast(ray, out hit))
        {
            (convert from drawing / colours to nodes))
        }*/
    }

    // Create the list to store the blob voxels
    List<Voxel> filledVoxels = new List<Voxel>();

    public void CreateGraph(Voxel origin, Function function)
    {

        

        ///To transform from drawing to graph, AFTER the use finishes drawing
        // Check if the origin is valid and add it to the list of voxels
        if (origin.VoxelFunction == Function)
        {
            new GameObject(); //create node
        }
        else return;

        //Debug.Log("filledVoxels");


        /// To check if voxels are filled
        /// To GetFaceNeighbours and create a group to be transformed to node
        /// VoxelsToCheck = Get all voxels that are not wall or connection voxels
        bool filled = true;
        while (filled)
        {
            List<Voxel> newVoxels = new List<Voxel>();
            foreach (var voxel in filledVoxels)
            {
                Voxel[] neighbours = voxel.GetFaceNeighboursXY().ToArray();
                foreach (var neighbour in neighbours)
                {
                    if (neighbour.VoxelFunction == Function &&
                        newVoxels.Contains(neighbour) &&
                        filledVoxels.Contains(neighbour))
                    {
                        //Create node of same colour;
                        voxel.VoxelFunction = function;
                    }
                }
            }

        }


        
        

        

     

      

    }

    //New list of DrawingNodes 

    //while voxelsToCheck is not empty
    ////take the first voxel
    ////Get function color
    ////Add a new Drawingnode to the list
    ////remove first voxel from voxelsToCheck
    /*public void BFS()
    {
        bool filled = true;
        {
          while (filled)
          {
                List<Voxel> newVoxels = new List<Voxel>();
                foreach (var voxel in filledVoxels)
                {

                }
          }
   


        }
    
    }*/
    ////newVoxels = true
    ////while newVoxels = true
    //////BFS the selected voxel in the group
    ////////Check if voxels are same color
    //////////add voxels to current drawing node
    //////////remove voxel from voxelsToCheck
    //////////new voxels = true

    //Analyse all DrawingNodes

      //public void GetConnections()
        //{
            //Get all voxels in connection color
            //BFS logic to group

            //BFS connections
            //remove wall voxels
            //Get DrawingNodes from connected voxels
            //Create edge between drawing nodes
       // }
}
