using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Code referenced from RC4_M3_C2
public class VoxelGrid
{
    //float degrees = 180;

    #region Public fields

    public Vector3Int GridSize;
    public Voxel[,,] Voxels;
    public Corner[,,] Corners;
    public Face[][,,] Faces = new Face[3][,,];
    public Edge[][,,] Edges = new Edge[3][,,];
    public Vector3 Origin;
    public Vector3 Corner;
    public float VoxelSize { get; private set; }

    public Dictionary<Function, Color> FunctionColors = new Dictionary<Function, Color>()
    {
        { Function.Empty, Color.white },
        { Function.Kitchen, new Color(55, 93, 126) },
        { Function.LivingRoom, new Color(104, 227, 189) },
        { Function.Bathroom, new Color(198, 109, 134) },
        { Function.Bedroom, new Color(255, 100, 132) },
        { Function.Closet, new Color(239, 131, 41) },
        { Function.Courtyard, new Color(255, 206, 88) },
        { Function.Dining, new Color(114, 90, 122) }
    };

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor for a basic <see cref="VoxelGrid"/>
    /// Adds a game object containing a collider to each of first layer voxels
    /// </summary>
    /// <param name="size">Size of the grid</param>
    /// <param name="origin">Origin of the grid</param>
    /// <param name="voxelSize">The size of each <see cref="Voxel"/></param>
    public VoxelGrid(Vector3Int size, Vector3 origin, float voxelSize, Transform parent = null)
    {
        GridSize = size;
        Origin = origin;
        VoxelSize = voxelSize;

        Voxels = new Voxel[GridSize.x, GridSize.y, GridSize.z];

        for (int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                for (int z = 0; z < GridSize.z; z++)
                {
                    if (y == 0)
                    {
                        Voxels[x, y, z] = new Voxel(
                            new Vector3Int(x, y, z),
                            this,
                            createCollider: true,
                            parent: parent);
                    }
                    else
                    {
                        Voxels[x, y, z] = new Voxel(
                            new Vector3Int(x, y, z),
                            this);
                    }
                    //Vector3 Voxel = new Vector3(0, degrees, 0);       //here
                }
            }
        }

        MakeFaces();
        MakeCorners();
        MakeEdges();
    }

    

    #endregion

    #region Grid elements constructors

    /// <summary>
    /// Creates the Faces of each <see cref="Voxel"/>
    /// </summary>
    private void MakeFaces()
    {
        // make faces
        Faces[0] = new Face[GridSize.x + 1, GridSize.y, GridSize.z];

        for (int x = 0; x < GridSize.x + 1; x++)
            for (int y = 0; y < GridSize.y; y++)
                for (int z = 0; z < GridSize.z; z++)
                {
                    Faces[0][x, y, z] = new Face(x, y, z, Axis.X, this);
                }

        Faces[1] = new Face[GridSize.x, GridSize.y + 1, GridSize.z];

        for (int x = 0; x < GridSize.x; x++)
            for (int y = 0; y < GridSize.y + 1; y++)
                for (int z = 0; z < GridSize.z; z++)
                {
                    Faces[1][x, y, z] = new Face(x, y, z, Axis.Y, this);
                }

        Faces[2] = new Face[GridSize.x, GridSize.y, GridSize.z + 1];

        for (int x = 0; x < GridSize.x; x++)
            for (int y = 0; y < GridSize.y; y++)
                for (int z = 0; z < GridSize.z + 1; z++)
                {
                    Faces[2][x, y, z] = new Face(x, y, z, Axis.Z, this);
                }
    }

    /// <summary>
    /// Creates the Corners of each Voxel
    /// </summary>
    private void MakeCorners()
    {
        Corner = new Vector3(Origin.x - VoxelSize / 2, Origin.y - VoxelSize / 2, Origin.z - VoxelSize / 2);

        Corners = new Corner[GridSize.x + 1, GridSize.y + 1, GridSize.z + 1];

        for (int x = 0; x < GridSize.x + 1; x++)
            for (int y = 0; y < GridSize.y + 1; y++)
                for (int z = 0; z < GridSize.z + 1; z++)
                {
                    Corners[x, y, z] = new Corner(new Vector3Int(x, y, z), this);
                }
    }

    /// <summary>
    /// Creates the Edges of each Voxel
    /// </summary>
    private void MakeEdges()
    {
        Edges[2] = new Edge[GridSize.x + 1, GridSize.y + 1, GridSize.z];

        for (int x = 0; x < GridSize.x + 1; x++)
            for (int y = 0; y < GridSize.y + 1; y++)
                for (int z = 0; z < GridSize.z; z++)
                {
                    Edges[2][x, y, z] = new Edge(x, y, z, Axis.Z, this);
                }

        Edges[0] = new Edge[GridSize.x, GridSize.y + 1, GridSize.z + 1];

        for (int x = 0; x < GridSize.x; x++)
            for (int y = 0; y < GridSize.y + 1; y++)
                for (int z = 0; z < GridSize.z + 1; z++)
                {
                    Edges[0][x, y, z] = new Edge(x, y, z, Axis.X, this);
                }

        Edges[1] = new Edge[GridSize.x + 1, GridSize.y, GridSize.z + 1];

        for (int x = 0; x < GridSize.x + 1; x++)
            for (int y = 0; y < GridSize.y; y++)
                for (int z = 0; z < GridSize.z + 1; z++)
                {
                    Edges[1][x, y, z] = new Edge(x, y, z, Axis.Y, this);
                }
    }

    #endregion

    #region Grid operations


    /// <summary>
    /// Get the Faces of the <see cref="VoxelGrid"/>
    /// </summary>
    /// <returns>All the faces</returns>
    public IEnumerable<Face> GetFaces()
    {
        for (int n = 0; n < 3; n++)
        {
            int xSize = Faces[n].GetLength(0);
            int ySize = Faces[n].GetLength(1);
            int zSize = Faces[n].GetLength(2);

            for (int x = 0; x < xSize; x++)
                for (int y = 0; y < ySize; y++)
                    for (int z = 0; z < zSize; z++)
                    {
                        yield return Faces[n][x, y, z];
                    }
        }
    }

    /// <summary>
    /// Get the Voxels of the <see cref="VoxelGrid"/>
    /// </summary>
    /// <returns>All the Voxels</returns>
    public IEnumerable<Voxel> GetVoxels()
    {
        for (int x = 0; x < GridSize.x; x++)
            for (int y = 0; y < GridSize.y; y++)
                for (int z = 0; z < GridSize.z; z++)
                {
                    yield return Voxels[x, y, z];
                }
    }

    /// <summary>
    /// Get the Corners of the <see cref="VoxelGrid"/>
    /// </summary>
    /// <returns>All the Corners</returns>
    public IEnumerable<Corner> GetCorners()
    {
        for (int x = 0; x < GridSize.x + 1; x++)
            for (int y = 0; y < GridSize.y + 1; y++)
                for (int z = 0; z < GridSize.z + 1; z++)
                {
                    yield return Corners[x, y, z];
                }
    }

    /// <summary>
    /// Get the Edges of the <see cref="VoxelGrid"/>
    /// </summary>
    /// <returns>All the edges</returns>
    public IEnumerable<Edge> GetEdges()
    {
        for (int n = 0; n < 3; n++)
        {
            int xSize = Edges[n].GetLength(0);
            int ySize = Edges[n].GetLength(1);
            int zSize = Edges[n].GetLength(2);

            for (int x = 0; x < xSize; x++)
                for (int y = 0; y < ySize; y++)
                    for (int z = 0; z < zSize; z++)
                    {
                        yield return Edges[n][x, y, z];
                    }
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Tries to create a black rectangle from the
    /// specified origin and with the specified size
    /// </summary>
    /// <param name="origin">The index of the origin</param>
    /// <param name="width">The width of the rectangle, in X</param>
    /// <param name="depth">The depth of the rectangle, in Z</param>
    /// <param name="layer">The layer to draw in, default is 0</param>
    /// <returns>If the process was successful</returns>
    public bool CreateBlackRectangle(Vector3Int origin, int width, int depth, int layer = 0)
    {
        FunctionColor fcolor = FunctionColor.Black;

        int oX = origin.x;
        int oZ = origin.z;

        List<Voxel> recVoxels = new List<Voxel>();

        for (int x = oX; x < oX + width; x++)
        {
            for (int z = oZ; z < oZ + depth; z++)
            {
                Vector3Int idx = new Vector3Int(x, 0, z);
                if (Util.ValidateIndex(GridSize, idx))
                {
                    var voxel = Voxels[x, layer, z];
                    if (voxel.FColor == FunctionColor.Empty)
                    {
                        recVoxels.Add(voxel);
                    }
                    else return false;
                }
                else return false;
            }
        }

        foreach (var voxel in recVoxels)
        {
            voxel.FColor = fcolor;
        }

        return true;
    }

    /// <summary>
    /// Tries to create a black blob from the
    /// specified origin and with the specified size
    /// </summary>
    /// <param name="origin">The index of the origin</param>
    /// <param name="radius">The radius of the blob in voxels</param>
    /// <param name="picky">If the blob should skip voxels randomly as it expands</param>
    /// <param name="flat">If the blob should be located on the first layer or use all</param>
    /// <returns></returns>
    
    public void FillBucket(Vector3Int origin, Function function)
    {
        // Create the list to store the blob voxels
        List<Voxel> filledVoxels = new List<Voxel>();
        
        // Check if the origin is valid and add it to the list of voxels
        if (Util.ValidateIndex(GridSize, origin))
        {
            filledVoxels.Add(Voxels[origin.x, origin.y, origin.z]);
        }
        else return;

        bool filled = false;
        while (!filled)
        {
            List<Voxel> newVoxels = new List<Voxel>();
            foreach (var voxel in filledVoxels)
            {
                Voxel[] neighbours = voxel.GetFaceNeighboursXY().ToArray();
                foreach (var neighbour in neighbours)
                {
                    if (neighbour.Function == Function.Empty &&
                        !newVoxels.Contains(neighbour) &&
                        !filledVoxels.Contains(neighbour))
                    {
                        newVoxels.Add(neighbour);
                    }
                }
            }
            if (newVoxels.Count == 0)
            {
                filled = true;
            }
            else
            {
                foreach (var newVoxel in newVoxels)
                {
                    filledVoxels.Add(newVoxel);
                }
            }
        }
        
        foreach (var voxel in filledVoxels)
        {
            voxel.Function = function;
        }
            //Coroutine here!
    }

    
    

    /// <summary>
    /// Reads an image pixel data and set the red pixels to the grid
    /// </summary>
    /// <param name="image">The reference image</param>
    /// <param name="layer">The target layer</param>
    public void SetStatesFromImage(Texture2D image, int layer = 0)
    {
        // Iterate through the XY plane
        for (int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                // Get the pixel color from the image
                var pixel = image.GetPixel(x, y);

                // Check if pixel is red
                if (pixel.r >pixel.g)
                {
                    // Set respective color to voxel
                    Voxels[x, y, layer].FColor = FunctionColor.Red;
                }
            }
        }
    }


    #endregion
}

/// <summary>
/// Color coded values
/// </summary>
public enum FunctionColor
{
    Empty = -1,
    Black = 0,
    Red = 1,
    Yellow = 2,
    Green = 3,
    Cyan = 4,
    Magenta = 5
}

public enum Function
{
    Empty,
    Kitchen,
    Bedroom,
    Bathroom,
    Door,
    LivingRoom,
    Closet,
    Dining,
    Courtyard
}