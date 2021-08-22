using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;
using EasyGraph;

// Code referenced from RC4_M3_C2
public class EnvironmentManager : MonoBehaviour
{
    //Cameras
    public GameObject mainUICam;
    public GameObject nodeViewCam;
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
    List<GameObject> _edgeLines = new List<GameObject>();
    List<Edge<Room>> _edges;
    public GameObject GONode;
    UndirecteGraph<Room, Edge<Room>> _graph;
    VoxelGrid voxelGrid;


    public float VoxelSize { get; private set; }

    Dictionary<(Function, Function), float> _functionAttraction = new Dictionary<(Function, Function), float>()
    {
        {(Function.Dining,Function.LivingRoom), 0f },
        {(Function.Kitchen,Function.Dining), 0f },
        {(Function.Kitchen,Function.Bedroom), 10f },
        {(Function.Bathroom,Function.Bedroom), 0f },
        {(Function.LivingRoom,Function.Kitchen), 0f },
        {(Function.Bedroom,Function.Closet), 0f },
        {(Function.Bathroom,Function.Dining), 0f },
        {(Function.Kitchen,Function.Courtyard), 10f },
        {(Function.LivingRoom,Function.Bedroom), 0f },
        {(Function.LivingRoom,Function.Closet), 20f },
        {(Function.Dining,Function.Bedroom), 20f },
        {(Function.Kitchen,Function.Closet), 020f },
        {(Function.Kitchen,Function.Bathroom), 30f },
        {(Function.Closet,Function.Courtyard), 40f },
        {(Function.Bathroom,Function.Courtyard), 40f },
        {(Function.Bedroom,Function.Courtyard), 30f },
        {(Function.Dining,Function.Closet), 40f }
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
      
        Debug.Log(" this part is  running");
        AnalyseRooms();
        Debug.Log("Is this really running?");
        AnalyseConnections();

        /*Camera mCam = mainUICam.GetComponent<Camera>();
        Camera nCam = nodeViewCam.GetComponent<Camera>();
        mCam.enabled = false;
        nCam.enabled = true;*/

       
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
        CalculateRoomsAreas();
    }

    private void CalculateRoomsAreas()
    {
        float houseVoxels = _rooms.Sum(r => r.Voxels.Count);
        float houseArea = 0;
        foreach (var room in _rooms)
        {
            float min = Util.AreasPerFunction[room.Function].Item1;
            float max = Util.AreasPerFunction[room.Function].Item2;
            float functionTargetArea = min + (max - min) / 2;
            houseArea += functionTargetArea;
        }

        foreach (var room in _rooms)
        {
            float min = Util.AreasPerFunction[room.Function].Item1;
            float max = Util.AreasPerFunction[room.Function].Item2;
            float area = (room.Voxels.Count / houseVoxels) * houseArea;

            float clamped = Mathf.Clamp(area, min, max);
            room.Area = clamped;
        }
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
        Debug.Log("Voxel is To Check:" + voxelsToCheck);
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

            Debug.Log("Connection Voxels:" + connectionVoxels.Count);

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

        }
        Debug.Log(_connections.Count);
        //return new List<Connection>();

    }
    #endregion

    #region Public Methods

    public void SaveAndReturn()
    {
        //Change camera to view nodes
        /*Camera mCam = mainUICam.GetComponent<Camera>();
        Camera nCam = nodeViewCam.GetComponent<Camera>();
        nodeViewCam.SetActive(true);
        mCam.enabled = false;
        nCam.enabled = true;*/
     
        AnalyseDrawing();
       
        //JsonExportImport.SaveScene(_gridLevels[_currentLevel], _rooms);
        //JsonExportImport.SaveScene(_voxelGrid, _rooms);
        var scene = JsonExportImport.ConvertToJsonScene(_voxelGrid, _rooms);
        // COMMENTED OUT THE LOAD SCENE ----------------------------------------------------------------------
        //SessionManager.AddScene(scene);
        //SceneManager.LoadScene("Nodes");

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
    /*public void ColorNode(VoxelGrid grid, List<Voxel> voxels, Function function)
    {
        _voxelGrid = grid;
        _selectedFunction = function;
        GONode.GetComponent<Renderer>().material = _voxelGrid.FunctionColors[function];
    }*/
    
    public void CreateGraph()
    {
        //var scene = JsonExportImport.ConvertToJsonScene(_voxelGrid, _rooms);
        //SessionManager.AddScene(scene);
        //SceneManager.LoadScene("Nodes");
        var graphManager = GameObject.Find("GraphManager").GetComponent<GraphManager>();
        graphManager.CreateGraph(_rooms, _connections);
        Debug.Log(_connections.Count);
    }

    public void ColorNode()
    {
        Camera mCam = mainUICam.GetComponent<Camera>();
        Camera nCam = nodeViewCam.GetComponent<Camera>();
        nodeViewCam.SetActive(true);
        mCam.enabled = false;
        nCam.enabled = true;
        //GONode.GetComponent<Renderer>().material = voxelGrid.FunctionColors[key:_selectedFunction] ; 
    }
    #endregion
}