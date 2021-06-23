using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Connection
{
    public Room Source;
    public Room End;
    public List<Voxel> Voxels;
    public Connection(Room source, Room end, List<Voxel> voxels)
    {
        Voxels = voxels;
        Source = Source;
        End = End;
        //float avgX = voxels.Average(voxels => voxels.Index.x);
        //float avgY = voxels.Average(voxels => voxels.Index.y);
        //float avgZ = voxels.Average(voxels => voxels.Index.z);

        //GameObject.Instantiate()
    }
}
