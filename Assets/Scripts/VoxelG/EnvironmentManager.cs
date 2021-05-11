using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Diagnostics;
using System.Linq;

// Code referenced from RC4_M3_C2
public class EnvironmentManager : MonoBehaviour
{
    #region Fields and properties
    //public Voxel Voxels;
    VoxelGrid _voxelGrid;
    public Vector3Int v;
    //public new Vector3Int origin;
    private Vector3Int origin;
    int _randomSeed = 666;
    public List<Voxel> filledVoxels;

    bool _showVoids = true;

    private Function _selectedFunction;

    private List<Room> _rooms;

    #endregion

    #region Unity Standard Methods

    void Start()
    {
        _selectedFunction = Function.Wall;
        // Initialise the voxel grid
        Vector3Int gridSize = new Vector3Int(80, 50, 1);
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
                _selectedFunction == Function.Empty)
            {
                // If it is a wall, private, shared or door, do this
                var voxel = SelectVoxel();
                if (voxel != null)
                {
                    voxel.VoxelFunction = _selectedFunction;
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

           

            // If not, it means that you are creating a space
            // So you should:
            // Get the voxel
            // Use the fill bucket from that voxel

            //origin = new Vector3Int(Mathf.RoundToInt(v.x), (v.y), (v.z));
            //VoxelGrid.FillBucket(origin, Vector3Int);
            ///_voxelGrid.FillBucket(Voxels.Index, Function.Bathroom);
            //_voxelGrid.FillBucket(Voxels.Index, Function.Bedroom);
            //_voxelGrid.FillBucket(Voxels.Index, Function.Closet);
            //_voxelGrid.FillBucket(Voxels.Index, Function.Dining);
            // _voxelGrid.FillBucket(Voxels.Index, Function.Kitchen);
            //_voxelGrid.FillBucket(Voxels.Index, _selectedFunction);  
        }

        


        /* void StartFillBucket()
        {
         StartCoroutine(FillBucket);
        }*/

        //if (Input.GetMouseButtonDown(0))
        //{
        //    var voxel = SelectVoxel();
        //    if (voxel != null)
        //    {
        //        if (_selectedFunction != Function.Wall &&
        //        _selectedFunction != Function.SharableSpace &&
        //        _selectedFunction != Function.Connector)
        //        {
        //            _voxelGrid.FillBucket(voxel.Index, _selectedFunction);
        //        }
        //    }
        //    else
        //    {
        //        SelectFunction();
        //    }
        //}
        /*if (Input.GetMouseButtonDown(0))
        {
            var voxel = SelectVoxel();

            if (voxel != null)
            {
                //print(voxel.Index);
                _voxelGrid.CreateBlackBlob(voxel.Index, 6, picky: true, flat: true);
            }
        }*/

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
            /* if (true)
             {
             _selectedFunction = Function.Bedroom;
             _selectedFunction = Function.Bathroom;
             _selectedFunction = Function.Kitchen;
             _selectedFunction = Function.Dining;
             _selectedFunction = Function.LivingRoom;
             _selectedFunction = Function.Closet;
             _selectedFunction = Function.Courtyard;
             }*/
            
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


    

    
    public void AnalyseRooms()
    {
        _rooms = new List<Room>();

        var voxelsToCheck = _voxelGrid.GetFlattenedVoxels().Where(v => v.VoxelFunction != Function.Wall &&
        v.VoxelFunction != Function.Empty &&
        v.VoxelFunction != Function.Connector &&
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

            _rooms.Add(new Room(roomVoxels));

        }
        Debug.Log($"Found {_rooms.Count} rooms");
    }

    public void AnalyseConnections()
    {
        // Find the voxels of type connection
        
        // Get the voxels that make up this connector
        // Get the neighbours of the connector
        // Find out which rooms the neighbours belong to
        // Store in the rooms their neighbouring rooms
       

    }

    

    

    void BFS (Vector3 Source, Vector3 End, Function function)
    {
        Queue<Vector3> queue = new Queue<Vector3>();
        HashSet<Vector3> filledVoxels = new HashSet<Vector3>();
        queue.Enqueue(Source);

        while (queue.Count != 0)
        {
            Vector3 Function = queue.Dequeue();
            if(Function == End)
            {
                //return function;
            }
        }
        
    }


    #endregion
}
