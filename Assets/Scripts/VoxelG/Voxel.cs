﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Code referenced from RC4_M3_C2
public class Voxel : IEquatable<Voxel>
{
    #region Public fields

    public Vector3Int Index;
    public List<Face> Faces = new List<Face>(6);
    public Vector3 Center => (Index + _voxelGrid.Origin) * _size *0.5f;
    public bool IsActive;
    
    public Room InRoom;
    public int resolution;

    //public VoxelSize voxelSize;
    //Vector3 voxelScale = Vector3.one * _size;
    public float VoxelSize { get; private set; }

    //public FunctionColor FColor;
    public Function VoxelFunction
    {
        get 
        {
            return _function;
        }
        set 
        {
            _function = value;
            var renderer = VoxelCollider.GetComponent<MeshRenderer>();
            renderer.material = _voxelGrid.FunctionColors[value];
        }
    }

    private Function _function;

    public GameObject VoxelCollider =  null;

    #endregion

    #region Protected fields

    public VoxelGrid _voxelGrid;             // changes, fix to protected
    protected float _size;
    //public float _size = 2f;

    public Voxel Voxels;
    public int voxelResolution = 8;

    #endregion

    private void Awake()
    {
        VoxelSize = 1f / resolution;

    }
   
    #region Contructors

    /// <summary>
    /// Creates a regular voxel on a voxel grid
    /// </summary>
    /// <param name="index">The index of the Voxel</param>
    /// <param name="voxelgrid">The <see cref="VoxelGrid"/> this <see cref="Voxel"/> is attached to</param>
    /// <param name="voxelGameObject">The <see cref="GameObject"/> used on the Voxel</param>
    public Voxel(Vector3Int index, VoxelGrid voxelGrid, bool createCollider = false, Transform parent = null)
    {
        
        Index = index;
        _voxelGrid = voxelGrid;
        _size = _voxelGrid.VoxelSize;
        IsActive = true;
        //FColor = FunctionColor.Empty;
        

        if (createCollider)
        {
            var colliderPrefab = Resources.Load<GameObject>("Prefabs/VoxelCollider");
            VoxelCollider = GameObject.Instantiate(colliderPrefab, parent, true);
            VoxelCollider.transform.localPosition = new Vector3(Index.x , Index.y , Index.z) * _size;
            
            VoxelCollider.transform.localScale = Vector3.one * _size;
            VoxelCollider.name = $"{Index.x}_{Index.y}_{Index.z}";
            VoxelCollider.tag = "Voxel";
            
           
            
        }

        VoxelFunction = Function.Empty;

    }

    /// <summary>
    /// Generic constructor, alllows the use of inheritance
    /// </summary>
    public Voxel() { }

    #endregion

    #region Public methods

    /// <summary>
    /// Get the neighbouring voxels at each face, if it exists
    /// </summary>
    /// <returns>All neighbour voxels</returns>
    public IEnumerable<Voxel> GetFaceNeighbours()
    {
        int x = Index.x;
        int y = Index.y;
        int z = Index.z;
        var s = _voxelGrid.GridSize;

        if (x != s.x - 1) yield return _voxelGrid.Voxels[x + 1, y, z];
        if (x != 0) yield return _voxelGrid.Voxels[x - 1, y, z];

        if (y != s.y - 1) yield return _voxelGrid.Voxels[x, y + 1, z];
        if (y != 0) yield return _voxelGrid.Voxels[x, y - 1, z];

        //if (z != s.z - 1) yield return _voxelGrid.Voxels[x, y, z + 1];
        //if (z != 0) yield return _voxelGrid.Voxels[x, y, z - 1];
    }

    /// <summary>
    /// Get the neighbouring voxels at each face, if it exists
    /// </summary>
    /// <returns>All neighbour voxels</returns>
    public IEnumerable<Voxel> GetFaceNeighboursXY()
    {
        int x = Index.x;
        int y = Index.y;
        int z = Index.z;
        var s = _voxelGrid.GridSize;

        if (x != s.x - 1) yield return _voxelGrid.Voxels[x + 1, y, z];
        if (x != 0) yield return _voxelGrid.Voxels[x - 1, y, z];

        if (y != s.y - 1) yield return _voxelGrid.Voxels[x, y + 1, z];
        if (y != 0) yield return _voxelGrid.Voxels[x, y - 1, z];

        if (z != s.z - 1) yield return _voxelGrid.Voxels[x, y, z + 1];
        if (z != 0) yield return _voxelGrid.Voxels[x, y, z - 1];

        //Debug.Log("GetFaceNeighbours");
    }

    public Voxel[] GetFaceNeighboursArray()
    {
        Voxel[] result = new Voxel[6];

        int x = Index.x;
        int y = Index.y;
        int z = Index.z;
        var s = _voxelGrid.GridSize;

        if (x != s.x - 1) result[0] = _voxelGrid.Voxels[x + 1, y, z];
        else result[0] = null;

        if (x != 0) result[1] = _voxelGrid.Voxels[x - 1, y, z];
        else result[1] = null;

        if (y != s.y - 1) result[2] = _voxelGrid.Voxels[x, y + 1, z];
        else result[2] = null;

        if (y != 0) result[3] = _voxelGrid.Voxels[x, y - 1, z];
        else result[3] = null;

        if (z != s.z - 1) result[4] = _voxelGrid.Voxels[x, y, z + 1];
        else result[4] = null;

        if (z != 0) result[5] = _voxelGrid.Voxels[x, y, z - 1];
        else result[5] = null;

        return result;
        
    }

    /// <summary>
    /// Activates the visibility of this voxel
    /// </summary>
    public void ActivateVoxel(bool state)
    {
        IsActive = state;
    }

    #endregion

    #region Equality checks

    /// <summary>
    /// Checks if two Voxels are equal based on their Index
    /// </summary>
    /// <param name="other">The <see cref="Voxel"/> to compare with</param>
    /// <returns>True if the Voxels are equal</returns>
    public bool Equals(Voxel other)
    {
        return (other != null) && (Index == other.Index);
       
    }

    /// <summary>
    /// Get the HashCode of this <see cref="Voxel"/> based on its Index
    /// </summary>
    /// <returns>The HashCode as an Int</returns>
    public override int GetHashCode()
    {
        return Index.GetHashCode();
    }

    #endregion

}