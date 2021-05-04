using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EasyGraph;
using UnityEngine;

public class HouseGraphManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> _goNodes;
    [SerializeField]
    List<Material> _materials;

    List<HouseNode> _nodes;
    List<Edge<HouseNode>> _edges;
    UndirecteGraph<HouseNode, Edge<HouseNode>> _undirectedGraph;

    List<GameObject> _edgeLines;
    Material _lineRenderMaterial;

    // Start is called before the first frame update
    void Start()
    {
        _lineRenderMaterial = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));

        //Extract the HouseNodes from the node gameobjects to be used in the graph
        _nodes = new List<HouseNode>(_goNodes.Select(g => g.GetComponent<HouseNode>()));

        //Initialise lists
        _edges = new List<Edge<HouseNode>>();
        _edgeLines = new List<GameObject>();

        //Set all the edges between nodes, including their weights
        _edges.Add(new Edge<HouseNode>(_nodes[0], _nodes[1], 7));
        _edges.Add(new Edge<HouseNode>(_nodes[1], _nodes[2], 3));
        _edges.Add(new Edge<HouseNode>(_nodes[2], _nodes[3], 4));
        _edges.Add(new Edge<HouseNode>(_nodes[1], _nodes[3], 2));
        _edges.Add(new Edge<HouseNode>(_nodes[4], _nodes[3], 3));
        _edges.Add(new Edge<HouseNode>(_nodes[4], _nodes[0], 15));

        //Create Graph out of edges
        _undirectedGraph = new UndirecteGraph<HouseNode, Edge<HouseNode>>(_edges);
        
        //Draw graphlines
        ResetGraphLines();
    }

    // Update is called once per frame
    void Update()
    {
        var nodes = _undirectedGraph.GetAllVertices();
        foreach (var node in nodes)
        {
            node.CalculateVelocity(_undirectedGraph, 0.1f);
            node.MoveHouseNode();
        }


        ResetGraphLines();

        RayCast();
    }

    void RayCast()
    {
        /*RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButton(0) && Physics.Raycast(ray, out hit))
        {
            GameObject objectHit = hit.transform.gameObject;
            if (objectHit.tag == "Node")
            {
                objectHit.GetComponent<MeshRenderer>().material = _materials[0];
                foreach (var vertex in _undirectedGraph.GetNeighbourVertices(objectHit.GetComponent<HouseNode>()))
                {
                    vertex.gGetComponent<MeshRenderer>().material = _materials[1];
                }

                Dijkstra<HouseNode, Edge<HouseNode>> dijkstra = new Dijkstra<HouseNode, Edge<HouseNode>>(_undirectedGraph);
                dijkstra.DijkstraCalculateWeights(_nodes[0]);

            }

            // Do something with the object that was hit by the raycast.
        }*/
    }

    private void OnGUI()
    {
        float width = 200f;
        float height = 20f;
        float padding = 10f;
        int counter = 0;

        for (int i = 0; i < _edges.Count; i++)
        {
            _edges[i].Weight = GUI.HorizontalSlider(new Rect(padding, padding + (padding + height) * ++counter, width, height), (float)_edges[i].Weight, 0f, 20f);
        }
    }
    void ResetGraphLines()
    {
        _edgeLines.ForEach(e => GameObject.Destroy(e));
        _edgeLines.Clear();
        List<Edge<HouseNode>> edges = _undirectedGraph.GetEdges();
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
        }
    }

    
}
