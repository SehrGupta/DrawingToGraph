using System.Collections;
using System.Collections.Generic;
using EasyGraph;
using UnityEngine;


//public enum Function { Empty, Bathroom, Bedroom, Kitchen, Living, Dining }
public class HouseNode
{

    #region Public Fields
    public GameObject GONode;
    public int Area;
    public List<Voxel> Voxels;

    public float Radius
    {
        get
        {
            return GONode.transform.localScale.x;
        }
        set
        {
            GONode.transform.localScale = Vector3.one * value;
        }
    }

    public Vector3 Position
    {
        get
        {
            return GONode.transform.position;
        }
    }


    #endregion

    #region PrivateFields

    private bool _relax;
    private Vector3 _velocity;

    #endregion

    #region Constructor
    public HouseNode(float radius, Function Function, Vector3 location)
    {
        var nodePrefab = Resources.Load("Prefabs/Node") as GameObject;
        GONode = GameObject.Instantiate(nodePrefab, location, Quaternion.identity);
        GONode.transform.localScale = Vector3.one * radius;
    }

    #endregion

    #region public Methods
    public void CalculateVelocity(UndirecteGraph<HouseNode, Edge<HouseNode>> graph, float speed)
    {

        List<Edge<HouseNode>> connectedEdges = graph.GetConnectedEdges(this);
        Vector3 velocity = Vector3.zero;

        foreach (var edge in connectedEdges)
        {
            HouseNode connectedVertex = edge.GetOtherVertex(this);
            float weight = (float)edge.Weight;
            Vector3 direction = connectedVertex.Position - this.Position;
            velocity += direction * (direction.magnitude - weight);
        }
        _velocity = velocity * speed;
    }

    public void MoveHouseNode()
    {
        if (_relax) GONode.transform.Translate(_velocity * Time.deltaTime, Space.World);
    }

    public void DrawLine()
    {
        //draw a line in a random orientation on the node (the length of the line relates to the node area, the anchorpoint is probably in the centre of the line)

    }

    public void VoxeliseLine()
    {
        //Find all the voxels this line is crossing
        ////shoot a ray from the line startpoint in the direction of the endpoint
        ////While ray didn't pass the endpoint
        //////Add the voxel it's hitting to the linevoxels
        //////move the ray startpoint very little from the hit point in the direction of the endpoint
        //////shoot new ray from new startoint to endpoint
    }

    public void GrowLines()
    {
        //Get voxelines
        ////BFS voxellines for a few iterations
    }

    public void GetPrintShell()
    {
        //Get interior volumes
        //Shell = BFS on interio volumes
    }
    #endregion
}
