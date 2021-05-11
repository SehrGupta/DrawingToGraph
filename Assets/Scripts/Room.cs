using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room
{
    public Function RoomFunction;
    public Vector3 CentrePoint;
    public int Area;//== amount of voxels
    public List<Voxel> Voxels;
    public List<Room> Neighbours;

#region Constructor
    public Room (List<Voxel> voxels)
    {
        Voxels = voxels;
        foreach(var voxel in Voxels)
        {
            voxel.InRoom = this;
        }

        float avgX = (float) voxels.Average(v => v.Index.x);
        float avgY = (float) voxels.Average(v => v.Index.y);
        float avgZ = (float) voxels.Average(v => v.Index.z);

        CentrePoint = new Vector3(avgX, avgY, avgZ);
    }

    public void AnalyseRoom()
    {
        //Get centrepoint from voxel list
    }

    #endregion


}