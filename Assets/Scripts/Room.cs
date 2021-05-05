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

    //Constructor
    public Room (List<Voxel> voxels)
    {
        Voxels = voxels;
    }

    public void AnalyseRoom()
    {
        //Get centrepoint from voxel list
    }

}