using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection
{
    public Room Source;
    public Room End;
    public List<Voxel> Voxels;
    public Connection(Room Source, Room End, List<Voxel> voxels)
    {
        Voxels = voxels;
    }
}
