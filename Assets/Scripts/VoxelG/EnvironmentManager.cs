﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;

// Code referenced from RC4_M3_C2
public class EnvironmentManager : MonoBehaviour
{
    #region Fields and properties
    //public Voxel Voxels;
    VoxelGrid _voxelGrid;
    Connection _connection;
    public Vector3Int v;
    //public new Vector3Int origin;
    private Vector3Int origin;
    int _randomSeed = 666;
    public List<Voxel> filledVoxels;
    bool _showVoids = true;
    Room _room;
    private Function _selectedFunction;

    private List<Room> _rooms;
    private List<Connection> _connections;
    public Room Source;
    public Room End;

    public float VoxelSize { get; private set; }
    #endregion

    #region Unity Standard Methods

    void Start()
    {
        _selectedFunction = Function.Wall;
        // Initialise the voxel grid
        Vector3Int gridSize = new Vector3Int(120, 80, 1);
 
        _voxelGrid = new VoxelGrid(gridSize, Vector3.zero, 2, parent: this.transform);

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
            if(_selectedFunction == Function.Wall || 
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
            StartCoroutine ("FillBucket");
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

            _rooms.Add(new Room(roomVoxels, _selectedFunction));

        }
        Debug.Log($"Found {_rooms.Count} rooms");
    }

    public void AnalyseConnections()
    {
        _connections = new List<Connection>();

        // Find the voxels of type connection
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

            connectionVoxels = bfsStepVoxels.SelectMany(l => l.Select(v => v)).ToList();

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
                
            }*/
            var roomsToCheck = _room.Voxels.Where(v => v.VoxelFunction == Function.Connector).ToList();
                
                //(_connection.Source && _connection.End => Function.Connector).ToList();
            foreach ( var neighbour in _rooms)
            {
                if (neighbour.RoomFunction  == Function.Connector)
                {
                    foundNewVoxels = true;
                    _rooms.Add(neighbour);
                   // roomsToCheck.Remove();
                }

                _rooms.Insert(1, _connection.Source);
                _rooms.Insert(2, _connection.End);
            }

            
        }

        //return new List<Connection>();

    }
    #endregion

    #region Public Methods

    public void SaveAndReturn()
    {
        JsonExportImport.SaveScene(_voxelGrid, _rooms);
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadFromFile(int level)
    {
        var scenes = JsonExportImport.LoadScenes();
        var scene = scenes.First(s => s.Level == level);
        Debug.Log(scene.JsonVoxels[0].Index);
        Debug.Log(scene.JsonVoxels[0].VoxelFunction);
        _voxelGrid.SetGridFromSaved(scene);
    }


    #endregion
}
