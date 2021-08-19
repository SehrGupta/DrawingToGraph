﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;
using EasyGraph;

// Code referenced from RC4_M3_C2
public class EnvironmentManager : MonoBehaviour
{
    #region Fields and properties
    //public Voxel Voxels;
    //VoxelGrid[] _gridLevels;
    VoxelGrid _voxelGrid;
    int _currentLevel = 0;
    //VoxelGrid _gridLevels[_currentLevel];
    Connection _connection;
    public Vector3Int v;
    //public new Vector3Int origin;
    private Vector3Int origin;
    int _randomSeed = 666;
    public List<Voxel> filledVoxels;
    bool _showVoids = true;
    Room _room;
    private Function _selectedFunction;

    [SerializeField]
    private GameObject _errorWindow;

    private List<Room> _rooms;
    private List<Connection> _connections;
    public Room Source;
    public Room End;

    public List<Room> Neighbours;
    List<GameObject> _edgeLines;
    List<Edge<Room>> _edges;


    public float VoxelSize { get; private set; }

    Dictionary<(Function, Function), float> _functionAttraction = new Dictionary<(Function, Function), float>()
    {
        {(Function.Dining,Function.LivingRoom), 0f },
        {(Function.Kitchen,Function.Dining), 0f },
        {(Function.Kitchen,Function.Bedroom), 1f },
        {(Function.Bathroom,Function.Bedroom), 0f },
        {(Function.LivingRoom,Function.Kitchen), 0f },
        {(Function.Bedroom,Function.Closet), 0f },
        {(Function.Bathroom,Function.Dining), 0f },
        {(Function.Kitchen,Function.Courtyard), 1f },
        {(Function.LivingRoom,Function.Bedroom), 0f },
        {(Function.LivingRoom,Function.Closet), 2f },
        {(Function.Dining,Function.Bedroom), 2f },
        {(Function.Kitchen,Function.Closet), 2f },
        {(Function.Kitchen,Function.Bathroom), 3f },
        {(Function.Closet,Function.Courtyard), 4f },
        {(Function.Bathroom,Function.Courtyard), 4f },
        {(Function.Bedroom,Function.Courtyard), 3f },
        {(Function.Dining,Function.Closet), 4f }
    };

    #endregion

    #region Unity Standard Methods

    void Start()
    {

        _errorWindow.SetActive(false);
        _selectedFunction = Function.Wall;
        // Initialise the voxel grid
        Vector3Int gridSize = new Vector3Int(120, 80, 1);

        // Check which level you are working on
        // check if the top and bottom levels have been create
        // create a dummy voxelgrid for each of them, if they exist
        // check for a staircase voxel in each one
        // store the index of the stair voxel
        // Delete the dummy voxel grids 
        //// Destroy(Voxel.VoxelCollider)

        _voxelGrid = new VoxelGrid(gridSize, Vector3.zero, 2, parent: this.transform);

        int level = SessionManager.CurrentLevel;
        if (SessionManager.CreatedScenes.ContainsKey(level))
        {
            var scene = SessionManager.CreatedScenes[level];
            _voxelGrid.SetGridFromSaved(scene);
            _rooms = _voxelGrid.SetRoomsFromSaved(scene);
        }

        // Set the voxels of the staircase to _voxelGrid

        // Set the random engine's seed
        Random.InitState(_randomSeed);

    }

    void Update()
    {
        // Draw the voxels according to their Function Colors
        //DrawVoxels();
        //DrawVoxelsFunction();

        // Use the V key to switch between showing voids
        if (Input.GetKeyDown(KeyCode.V))
        {
            _showVoids = !_showVoids;
        }

        if (Input.GetMouseButton(0))
        {
            // Check if you are drawing a wall or a space
            if (_selectedFunction == Function.Wall ||
                _selectedFunction == Function.SharableSpace ||
                _selectedFunction == Function.Connector ||
                _selectedFunction == Function.Eraser ||
                _selectedFunction == Function.Empty)
            {
                // If it is a wall, private, shared or door, do this
                var voxel = SelectVoxel();
                if (voxel != null)
                {
                    voxel.VoxelFunction = _selectedFunction;
                    //DrawWalls(voxel);
                }
                else
                {
                    SelectFunction();
                }

            }
            else
            {
                var voxel = SelectVoxel();
                if (voxel != null)
                {
                    if (_selectedFunction != Function.Wall &&
                    _selectedFunction != Function.SharableSpace &&
                    _selectedFunction != Function.Connector &&
                    _selectedFunction != Function.Eraser &&
                    _selectedFunction != Function.Empty)
                    {
                        //_gridLevels[_currentLevel].FillBucket(voxel, _selectedFunction);
                        _voxelGrid.FillBucket(voxel, _selectedFunction);
                    }
                }
                else
                {
                    SelectFunction();

                }
            }

        }


    }

    public void AdjustVoxelSize(float newVoxelSize)                      //For slider : voxel selection increase/decrease
    {
        var voxel = SelectVoxel();

        if (_selectedFunction == Function.Wall ||
                _selectedFunction == Function.SharableSpace ||
                _selectedFunction == Function.Connector ||
                _selectedFunction == Function.Eraser)
        {
            VoxelSize = newVoxelSize;
        }

    }

    private void DrawWalls(Voxel target)
    {
        var neighbours = target.GetFaceNeighbours();
        target.VoxelFunction = _selectedFunction;
        foreach (var neighbour in neighbours)
        {
            neighbour.VoxelFunction = _selectedFunction;
        }
    }

    #endregion

    #region Private Methods

    Voxel SelectVoxel()
    {
        Voxel selected = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Transform objectHit = hit.transform;

            if (objectHit.CompareTag("Voxel"))
            {
                string voxelName = objectHit.name;
                var index = voxelName.Split('_').Select(v => int.Parse(v)).ToArray();

                //selected = _gridLevels[_currentLevel].Voxels[index[0], index[1], index[2]];
                selected = _voxelGrid.Voxels[index[0], index[1], index[2]];
            }

        }
        return selected;

    }




    void SelectFunction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Transform objectHit = hit.transform;

            if (objectHit.name == "Bedroom")
            {
                _selectedFunction = Function.Bedroom;
            }
            else if (objectHit.name == "Wall")
            {
                _selectedFunction = Function.Wall;
            }
            else if (objectHit.name == "Bathroom")
            {
                _selectedFunction = Function.Bathroom;
            }
            else if (objectHit.name == "Kitchen")
            {
                _selectedFunction = Function.Kitchen;
            }
            else if (objectHit.name == "LivingRoom")
            {
                _selectedFunction = Function.LivingRoom;
            }
            else if (objectHit.name == "Closet")
            {
                _selectedFunction = Function.Closet;
            }
            else if (objectHit.name == "Dining")
            {
                _selectedFunction = Function.Dining;
            }
            else if (objectHit.name == "Courtyard")
            {
                _selectedFunction = Function.Courtyard;
            }
            else if (objectHit.name == "SharableSpace")
            {
                _selectedFunction = Function.SharableSpace;
            }
            else if (objectHit.name == "Eraser")
            {
                _selectedFunction = Function.Eraser;
            }
            else
            {
                _selectedFunction = Function.Connector;
            }

            Debug.Log($"Current function is {_selectedFunction}");

        }
        //Coroutine
        if (Input.GetMouseButton(0))
        {
            StartCoroutine("FillBucket");
        }

    }
    IEnumerator FillBucket()
    {
        for (float ft = 1f; ft >= 0; ft -= 0.1f)
        {
            WaitForSeconds wait = new WaitForSeconds(0.1f);
        }
        yield return false;

        // Debug.Log("FillBucket");

    }

    /// <summary>
    /// Draws the voxels according to it's state and Function Corlor
    /// </summary>

    public void Thumbnail()
    {
        IEnumerator TakeScreenshot()
        {
            yield return new WaitForEndOfFrame();

            // create a texture to pass to encoding
            Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

            // put buffer into texture
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

            yield return 0;

            texture.Apply();

            yield return 0;

            byte[] bytes = texture.EncodeToPNG();

            ////// save the image
            /*Sprite itemBGSprite = Resources.Load<Sprite>("Material/Screenshots");
            Texture2D itemBGTex = itemBGSprite.texture;
            byte[] itemBGBytes = itemBGTex.EncodeToPNG();*/
            //File.WriteAllBytes(formattedCampaignPath + "Material/Screenshots.png", itemBGBytes);


            string Resources = "drawing.png";
            File.WriteAllBytes(Resources, bytes);
            //Resources.UnloadAsset<Material>("Material/Screenshots");
            //WriteAllBytes(imagePath, bytes);

            DontDestroyOnLoad(transform.gameObject);
        }
    }

    public void AnalyseDrawing()
    {
        AnalyseRooms();
        AnalyseConnections();
    }


    public void AnalyseRooms()
    {
        _rooms = new List<Room>();

        //var voxelsToCheck = _gridLevels[_currentLevel].GetFlattenedVoxels().Where(v => v.VoxelFunction != Function.Wall &&
        var voxelsToCheck = _voxelGrid.GetFlattenedVoxels().Where(v => v.VoxelFunction != Function.Wall &&
        v.VoxelFunction != Function.Empty &&
        v.VoxelFunction != Function.Connector &&
        v.VoxelFunction != Function.Eraser &&
        v.VoxelFunction != Function.SharableSpace).ToList();

        while (voxelsToCheck.Count > 0)
        {
            var start = voxelsToCheck[0];
            List<Voxel> roomVoxels = new List<Voxel>();
            roomVoxels.Add(start);

            List<List<Voxel>> bfsStepVoxels = new List<List<Voxel>>();
            bfsStepVoxels.Add(roomVoxels);


            bool foundNewVoxels = true;
            while (foundNewVoxels)
            {
                foundNewVoxels = false;

                List<Voxel> newVoxels = new List<Voxel>();
                foreach (var voxel in bfsStepVoxels.Last())
                {
                    var neighbours = voxel.GetFaceNeighboursXY();
                    foreach (var neighbour in neighbours)
                    {
                        if (neighbour.VoxelFunction == start.VoxelFunction && voxelsToCheck.Contains(neighbour) && !newVoxels.Contains(neighbour))
                        {
                            foundNewVoxels = true;
                            newVoxels.Add(neighbour);
                            voxelsToCheck.Remove(neighbour);
                        }
                    }
                }
                bfsStepVoxels.Add(newVoxels);
            }

            roomVoxels = bfsStepVoxels.SelectMany(l => l.Select(v => v)).ToList();

            _rooms.Add(new Room(_voxelGrid, roomVoxels, _selectedFunction));
            _rooms.Last().AddRoomToVoxels();

        }
        Debug.Log($"Found {_rooms.Count} rooms");
    }

    public void AnalyseConnections()
    {
        _connections = new List<Connection>();

        // Find the voxels of type connection
        //var voxelsToCheck = _gridLevels[_currentLevel].GetFlattenedVoxels().Where(v =>
        var voxelsToCheck = _voxelGrid.GetFlattenedVoxels().Where(v =>
        v.VoxelFunction == Function.Connector).ToList();

        // Get the voxels that make up this connector
        // Get the neighbours of the connector

        while (voxelsToCheck.Count > 0)
        {
            var start = voxelsToCheck[0];
            List<Voxel> connectionVoxels = new List<Voxel>();
            connectionVoxels.Add(start);
            List<List<Voxel>> bfsStepVoxels = new List<List<Voxel>>();
            bfsStepVoxels.Add(connectionVoxels);

            bool foundNewVoxels = true;
            while (foundNewVoxels)
            {
                foundNewVoxels = false;

                List<Voxel> newVoxels = new List<Voxel>();
                foreach (var voxel in bfsStepVoxels.Last())
                {
                    var neighbours = voxel.GetFaceNeighboursXY();
                    foreach (var neighbour in neighbours)
                    {
                        if (voxelsToCheck.Contains(neighbour) && !newVoxels.Contains(neighbour))
                        {
                            foundNewVoxels = true;
                            newVoxels.Add(neighbour);
                            voxelsToCheck.Remove(neighbour);
                        }
                    }
                }
                bfsStepVoxels.Add(newVoxels);
            }

            connectionVoxels = bfsStepVoxels.SelectMany(l => l.Select(v => v)).ToList();


            List<Room> connectedRooms = new List<Room>();
            foreach (var voxel in connectionVoxels)
            {
                foreach (var neighbour in voxel.GetFaceNeighboursXY())
                {
                    if ((int)neighbour.VoxelFunction > 0)
                    {
                        connectedRooms.Add(neighbour.InRoom);
                    }
                }
            }

            connectedRooms = connectedRooms.Distinct().ToList();
            Debug.Log($"{connectedRooms.Count} connected rooms found");
            if (connectedRooms.Count >= 2)
                _connections.Add(new Connection(connectedRooms[0], connectedRooms[1], connectionVoxels));


            // Find out which rooms the neighbours belong to
            // Store in the rooms their neighbouring rooms
            /*foreach (var connectionVoxel in connectionVoxels)
            {
                
                foreach (var voxel in _room.Voxels)
                {
                    if (connectionVoxel.InRoom == voxel.InRoom)
                    {
                        Debug.Log($"Found connection for voxel {connectionVoxel}");
                    }
                }
                
            }
            var roomsToCheck = _room.Voxels.Where(v => v.VoxelFunction == Function.Connector).ToList();

            //(_connection.Source && _connection.End => Function.Connector).ToList();
            foreach (var neighbour in _rooms)
            {
                if (neighbour.RoomFunction == Function.Connector)
                {
                    foundNewVoxels = true;
                    _rooms.Add(neighbour);
                    // roomsToCheck.Remove();
                }

                _rooms.Insert(1, _connection.Source);
                _rooms.Insert(2, _connection.End);
            }

            */
        }

        //return new List<Connection>();

    }
    #endregion

    #region Public Methods

    public void SaveAndReturn()
    {
        AnalyseDrawing();
        //JsonExportImport.SaveScene(_gridLevels[_currentLevel], _rooms);
        //JsonExportImport.SaveScene(_voxelGrid, _rooms);
        var scene = JsonExportImport.ConvertToJsonScene(_voxelGrid, _rooms);
        SessionManager.AddScene(scene);
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadFromFile(int level)
    {
        var scenes = JsonExportImport.LoadScenes();
        var scene = scenes.First(s => s.Level == level);
        Debug.Log(scene.JsonVoxels[0].Index);
        Debug.Log(scene.JsonVoxels[0].VoxelFunction);
        //_gridLevels[_currentLevel].SetGridFromSaved(scene);
        _voxelGrid.SetGridFromSaved(scene);
    }

    public void FinishAndShowLines()
    {
        var parent = GameObject.Find("LineRenderers");
        foreach (var connection in _connections)
        {
            var go = new GameObject();
            go.transform.parent = parent.transform;
            var renderer = go.AddComponent<LineRenderer>();

            renderer.positionCount = 2;

            Vector3 source = connection.Source.CentrePoint; // visualise the nodes
            Vector3 end = connection.End.CentrePoint; // visualise the nodes

            renderer.SetPosition(0, source);
            renderer.SetPosition(1, end);
        }
        // For every Connection (_connections), create an empty GO
        // Creat a LineRenderer in that GO
        // Add the GO to the LineRenderers game object
        // Set the Connection source and end as the line renderer points
    }
    public void ColorNode(VoxelGrid grid, List<Voxel> voxels, Function function)
    {
        _voxelGrid = grid;
        _selectedFunction = function;
        //GONode.GetComponent<Renderer>().material = _voxelGrid.FunctionColors[function];
    }
    
    public void CreateGraph()
    {
        //Check if there are actually rooms to make a graph
        if (_rooms.Count < 2) return;
        UndirecteGraph<Room, Edge<Room>> graph;
        List<Edge<Room>> edges = new List<Edge<Room>>();

        foreach (var connection in _connections)
        {
            Edge<Room> edge = new Edge<Room>(connection.Source, connection.End);
            if (_functionAttraction.ContainsKey((connection.Source.SelectedFunction, connection.End.SelectedFunction)))
            {
                edge.Weight = _functionAttraction[(connection.Source.SelectedFunction, connection.End.SelectedFunction)];
            }
            else if (_functionAttraction.ContainsKey((connection.End.SelectedFunction, connection.Source.SelectedFunction)))
            {
                edge.Weight = _functionAttraction[(connection.End.SelectedFunction, connection.Source.SelectedFunction)];
            }
            else
            {
                Debug.Log("Weight not defined");
                edge.Weight = 1f;
            }


            edges.Add(edge);
        }

        graph = new UndirecteGraph<Room, Edge<Room>>(edges);

    }
    
    public void CreateGraphLines()
    {
        
        _edgeLines.ForEach(e => GameObject.Destroy(e));
        _edgeLines.Clear();
        List<Edge<Room>> edges = new List<Edge<Room>>();
        //Edge<Room> edge = new Edge<Room>(connection.Source, connection.End);
        foreach (var connection in _connections)
        {
            
            //Calculate the difference between the edge length and the desired length
            float relaxedDistance = Mathf.Abs((float) - (connection.Source.Position - connection.End.Position).magnitude);
            float colour = Mathf.Clamp01(relaxedDistance / 2);

            //Draw lines
            GameObject edgeLine = new GameObject($"Edge {_edgeLines.Count}");
            LineRenderer renderer = edgeLine.AddComponent<LineRenderer>();
            renderer.SetPosition(0, connection.Source.Position);
            renderer.SetPosition(1, connection.End.Position);

            //Set colours
            renderer.material = Resources.Load<Material>("Material/LineRenderer");
            renderer.startWidth = 0.2f;
            renderer.startColor = new Color(colour, 1 - colour, 0f);
            renderer.endWidth = 0.2f;
            renderer.endColor = new Color(colour, 1 - colour, 0f);
            _edgeLines.Add(edgeLine);
        }
    }
    #endregion
}