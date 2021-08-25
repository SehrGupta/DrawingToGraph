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
    public float Area;
    public List<Voxel> Voxels;
    public List<Room> Neighbours;
    public GameObject GONode;
    private Vector3 scaleChange, positionChange;
    public Function SelectedFunction { get; private set; }
    VoxelGrid _voxelGrid;
    public Function _selectedFunction;
    //public Vector3 position;
    public Vector3 velocity;
    public Function Function;
    private List<Connection> _connections;
    public Room Source;
    public Room End;
    private float _regionSize = 30f;
    private float _centerX;
    private float _centerY;
    private float _radius;

    #region Constructor

    public Room(VoxelGrid grid, List<Voxel> voxels, Function function)
    {
        _voxelGrid = grid;
        Voxels = voxels;
        SelectedFunction = function;

        var nodePrefab = Resources.Load<GameObject>("Prefabs/GONode");
        GONode = GameObject.Instantiate(nodePrefab);
        //GONode.GetComponent<Renderer>().material = _voxelGrid.FunctionColors[_selectedFunction];

        var roomNode = GONode.AddComponent<RoomNode>();
        roomNode.ThisRoom = this;

        

        float rndX = UnityEngine.Random.Range(0, _regionSize);
        float rndY = UnityEngine.Random.Range(0, _regionSize);
        float rndZ = UnityEngine.Random.Range(0, _regionSize);

        CentrePoint = new Vector3(rndX, rndY, rndZ);

        GONode.transform.position = CentrePoint;


        


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

    #region PrivateFields

    public Vector3 Position
    {
        get
        {
            return GONode.transform.position;
        }
    }

    private bool _relax;
    private Vector3 _velocity;
  
    #endregion

    public void Update()
    { 
        foreach (var room in Neighbours)
        {
            room.CentrePoint += room.velocity * Time.deltaTime;
        }
    }



    #region public Methods
    public void CalculateVelocity(UndirecteGraph<Room, Edge<Room>> graph, float speed)
    {

        List<Edge<Room>> connectedEdges = graph.GetConnectedEdges(this);
        Vector3 velocity = Vector3.zero;

        foreach (var edge in connectedEdges)
        {
            Room connectedVertex = edge.GetOtherVertex(this);
            float weight = (float)edge.Weight;
            Vector3 direction = connectedVertex.Position - this.Position;
            velocity += direction * (direction.magnitude - weight);
        }
        _velocity = velocity * speed;
    }

    public void MoveRoom()
    {
        GONode.transform.Translate(_velocity * Time.deltaTime, Space.World);
        CentrePoint = GONode.transform.position;
    }

    /*public class GONode
    {
        public Vector3 position;
        public Vector3 velocity;
        public List<GONode> children;

    }*/

    /*public void GenerateGraph()
    {
        Edge<Room> edge = new Edge<Room>(connection.Source, connection.End);
        foreach (var nodes in Neighbours)
        {
            
            Gizmos.DrawLine(connection.Source, connection.End);
        }
    }*/

    /*typedef strcut()
    {
        float centerX;
        float centerY;
        float radius;
    }*/

    public void AreaGeneration()
    {       
        Collider m_Collider;
        Vector3 m_Center;
        Vector3 m_Size, m_Min, m_Max;

        //Fetch the Collider from the GameObject
        m_Collider = GONode.GetComponent<Collider>();
        //Fetch the center of the Collider volume
        m_Center = m_Collider.bounds.center;
        //Fetch the size of the Collider volume
        m_Size = m_Collider.bounds.size;
        //Fetch the minimum and maximum bounds of the Collider volume
        m_Min = m_Collider.bounds.min;
        m_Max = m_Collider.bounds.max;

    }


    #endregion
}