using System;
using System.IO;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class JsonExportImport
{
    private static string _path
    {
        get
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
                return Application.dataPath + "/Saves/";
            else
                return Application.persistentDataPath + "/Saves/";
        }
    }
    //public static JsonScene ConvertToJsonScene(List<Block> blocks)
    //{
    //    return new JsonScene(blocks);
    //}
    public static JsonScene ConvertToJsonScene(VoxelGrid grid, List<Room> rooms)
    {
        return new JsonScene(grid, rooms);
    }

    public static void SaveScene(VoxelGrid grid, List<Room> rooms)
    {
        JsonScene jsonScene = ConvertToJsonScene(grid, rooms);
        string json = JsonUtility.ToJson(jsonScene);
        string filename = $"Level1.json";

        try
        {
            if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);
            //if (!File.Exists(_path + filename)) System.IO.File.WriteAllText(_path + filename, json);
            //else Debug.LogWarning("File allready exists");

            System.IO.File.WriteAllText(_path + filename, json);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
    }


    public static List<JsonScene> LoadScenes()
    {
        List<JsonScene> jsonScenes = new List<JsonScene>();
        try
        {
            if (!Directory.Exists(_path))
            {
                Debug.LogWarning("Directory does not exist");
                return null;
            }
            string[] fileNames = Directory.GetFiles(_path);


            foreach (var fileName in fileNames.Where(s => Path.GetExtension(s) == ".json"))
            {
                string json = System.IO.File.ReadAllText(fileName);
                jsonScenes.Add(JsonUtility.FromJson<JsonScene>(json));
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
        return jsonScenes;
    }

    
}

[Serializable]
public class JsonScene
{
    public int DesignID = -1;
    public string DesignCreator;
    public string DesignName;
    public string DesignDescription;
    public string DesignJSON;
    public int DesignPublic;
    public string DesignDateTimeCreation;
    public string DesignDateTimeUpdate;
    public string DesignActiveEditor;
    public string DesignImage;
    public int Level = 1;

    public List<JsonRoom> JsonRooms = new List<JsonRoom>();
    public List<JsonVoxel> JsonVoxels = new List<JsonVoxel>();

    //public string SceneName=>$"{Publisher} {Util.ToLocalDateTime(SubmitTime).ToString("d",CultureInfo.CreateSpecificCulture("en-GB"))}";

    public JsonScene(VoxelGrid grid, List<Room> rooms)
    {
        var jsonVoxelGrid = new JsonVoxelGrid(grid);
        foreach (var voxel in grid.Voxels)
        {
            JsonVoxels.Add(new JsonVoxel(voxel));
        }
        foreach (var room in rooms)
        {
            JsonRooms.Add(new JsonRoom(room, jsonVoxelGrid));
        }
    }

    //public JsonScene(List<Block> blocks)
    //{

    //    foreach (var block in blocks)
    //    {
    //        JsonBlocks.Add(new JsonBlock(block.Anchor, block.Rotation, (int)block.Type));
    //    }
    //}
    public JsonScene()
    {
    }

    //Used to create a JsonScene from a JSON string
    public static JsonScene CreateFromJSON(string jString)
    {
        JsonScene jScene = JsonUtility.FromJson<JsonScene>(jString);
        return jScene;
    }
}

public class JSONEqualityComparer : IEqualityComparer<JsonScene>
{
    public bool Equals(JsonScene x, JsonScene y)
    {
        return x.DesignID == y.DesignID;
    }

    public int GetHashCode(JsonScene obj)
    {
        unchecked
        {
            if (obj == null)
                return 0;
            int hashCode = obj.DesignID.GetHashCode();
            hashCode = (hashCode * 397) ^ obj.DesignID.GetHashCode();
            return hashCode;
        }
    }
}


[Serializable]
public class JsonVoxelGrid
{
    public Vector3Int GridSize;
    public List<JsonVoxel> Voxels = new List<JsonVoxel>();
    //public Corner[,,] Corners;
    //public Face[][,,] Faces = new Face[3][,,];
    //public Edge[][,,] Edges = new Edge[3][,,];
    public Vector3 Origin;
    public Vector3 Corner;

    public JsonVoxelGrid(VoxelGrid grid) 
    {
        GridSize = grid.GridSize;
        //Voxels = grid.Voxels;
        //Voxels = new JsonVoxel[GridSize.x, GridSize.y, GridSize.z];
        foreach (var voxel in grid.Voxels)
        {
            Voxels.Add(new JsonVoxel(voxel));
        }
        //Corners = grid.Corners;
        //Faces = grid.Faces;
        //Edges = grid.Edges;
        Origin = grid.Origin;
        Corner = grid.Corner;
    }
}

[Serializable]
public class JsonVoxel
{
    public Vector3Int Index;
    public bool IsActive;
    //public JsonRoom InRoom;
    //public float VoxelSize;

    //public FunctionColor FColor;
    public string VoxelFunction;

    public JsonVoxel(Voxel voxel)
    {
        Index = voxel.Index;
        IsActive = voxel.IsActive;
        VoxelFunction = voxel.VoxelFunction.ToString();
        //VoxelSize = voxel.VoxelSize;
    }
}

[Serializable]
public class JsonRoom
{
    public string RoomFunction;
    public Vector3 CentrePoint;
    public int Area;
    public List<Vector3Int> Voxels;
    public string SelectedFunction;

    public JsonRoom(Room room, JsonVoxelGrid grid)
    {
        RoomFunction = room.RoomFunction.ToString();
        CentrePoint = room.CentrePoint;
        Area = room.Area;
        //Voxels = room.Voxels.Select(v => new JsonVoxel(v)).ToList();
        Voxels = new List<Vector3Int>();
        foreach (var voxel in room.Voxels)
        {
            Vector3Int jvoxel = voxel.Index;
            Voxels.Add(jvoxel);
        }
        SelectedFunction = room.SelectedFunction.ToString();
    }
}




//[Serializable]
//public class JsonBlock
//{
//    public Vector3Int Anchor;
//    public Quaternion Rotation;
//    public int BlockType;

//    public JsonBlock(Vector3Int anchor, Quaternion rotation, int blockType)
//    {
//        Anchor = anchor;
//        Rotation = rotation;
//        BlockType = blockType;
//    }
//}
