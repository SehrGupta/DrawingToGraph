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

    bool _showVoids = true;

    private Function _selectedFunction;

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
        DrawVoxelsFunction();

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
                    voxel.Function = _selectedFunction;
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


    void DrawVoxelsFunction()
    {
        foreach (var voxel in _voxelGrid.Voxels)
        {
            if (voxel.IsActive)
            {
                Vector3 pos = (Vector3)voxel.Index * _voxelGrid.VoxelSize + transform.position;
                var color = _voxelGrid.FunctionColors[voxel.Function];
                Drawing.DrawCube(pos, _voxelGrid.VoxelSize, color, sizeFactor: 1f);

                // Debug.Log(_voxelGrid.Voxels.Length); 
            }
        }
        
    }

    
    

    #endregion
}
