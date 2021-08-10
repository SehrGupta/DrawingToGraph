using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGraphs : MonoBehaviour
{
    //public Function RoomFunction;
    public Vector3 CentrePoint;
    public int Area;//== amount of voxels
    public List<Voxel> Voxels;
    public List<Room> Neighbours;
    public GameObject GONode;
    private Vector3 scaleChange, positionChange;
    public Function SelectedFunction { get; private set; }
    VoxelGrid _voxelGrid;
    public Function _selectedFunction;

    List<GameObject> _edgeLines;
    Material _lineRenderMaterial;


    public void RoomFunction(Function function)
    {

        //GONode = GetComponents<Material>(_selectedFunction);

        ////Set minimum - maximum value for each function
        if (function != Function.Connector &&
             GameObject.Instantiate(GONode))
        {
            GONode.transform.position = CentrePoint + Voxels[0]._voxelGrid.Origin * Voxels[0]._voxelGrid.VoxelSize;

            if (SelectedFunction == Function.Bathroom)
            {
                scaleChange = new Vector3(1.5f, 2.4f, 1.5f);                     // convert to range
            }
            else if (SelectedFunction == Function.Bedroom)
            {
                scaleChange = new Vector3(4, 3, 4);
            }
            else if (SelectedFunction == Function.Closet)
            {
                scaleChange = new Vector3(2, 2, 2);
            }
            else if (SelectedFunction == Function.Dining)
            {
                scaleChange = new Vector3(2.5f, 3, 2.5f);
            }
            else if (SelectedFunction == Function.LivingRoom)
            {
                scaleChange = new Vector3(3.7f, 5.5f, 3.7f);
            }
            else if (SelectedFunction == Function.Kitchen)
            {
                scaleChange = new Vector3(3.4f, 3, 3.4f);
            }
            else
            {
                scaleChange = new Vector3(5, 5, 5);
            }
        }

        void ResetGraphLines()
        {
            _edgeLines.ForEach(e => GameObject.Destroy(e));
            _edgeLines.Clear();
            /*List<Edge<HouseNode>> edges = _voxelGrid.GetEdges();
            foreach (var edge in edges)
            {
                //Calculate the difference between the edge length and the desired length
                float relaxedDistance = Mathf.Abs((float)edge.Weight - (edge.Source.Position - edge.Target.Position).magnitude);
                float colour = Mathf.Clamp01(relaxedDistance / 2);

                //Draw lines
                GameObject edgeLine = new GameObject($"Edge {_edgeLines.Count}");
                LineRenderer renderer = edgeLine.AddComponent<LineRenderer>();
                renderer.SetPosition(0, edge.Source.Position);
                renderer.SetPosition(1, edge.Target.Position);

                //Set colours
                renderer.material = _lineRenderMaterial;
                renderer.startWidth = 0.2f;
                renderer.startColor = new Color(colour, 1 - colour, 0f);
                renderer.endWidth = 0.2f;
                renderer.endColor = new Color(colour, 1 - colour, 0f);
                _edgeLines.Add(edgeLine);
            }*/
        }
    }
}
