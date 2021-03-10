using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Diagnostics;
using System.Linq;

// Code referenced from RC4_M3_C2
public class EnvironmentManager : MonoBehaviour
{
    #region Fields and properties

    VoxelGrid _voxelGrid;
    int _randomSeed = 666;

    bool _showVoids = true;

    #endregion

    #region Unity Standard Methods

    void Start()
    {
        // Initialise the voxel grid
        Vector3Int gridSize = new Vector3Int(50, 1, 30);
        _voxelGrid = new VoxelGrid(gridSize, Vector3.zero, 1, parent: this.transform);

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

            if(objectHit.CompareTag("Voxel"))
            {
                string voxelName =  objectHit.name;
                var index = voxelName.Split('_').Select(v => int.Parse(v)).ToArray();

                selected = _voxelGrid.Voxels[index[0], index[1], index[2]];
            }
        }
        return selected;
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
                var color = _voxelGrid.FunctionColors[voxel.VoxelFunction];
                Drawing.DrawCube(pos, _voxelGrid.VoxelSize, color);
                
               // Debug.Log(_voxelGrid.Voxels.Length); 
            }
        }
    }

    #endregion
}
