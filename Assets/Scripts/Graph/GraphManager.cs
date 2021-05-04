using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyGraph;
using System.Linq;

public class GraphManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> _nodes;
    [SerializeField]
    List<Material> _materials;

    List<Edge<GameObject>> _edges;
    UndirecteGraph<GameObject, Edge<GameObject>> _undirectedGraph;
    List<GameObject> _edgeLines;
    
    // Start is called before the first frame update
    void Start()                                                  /// creation of nodes & connecting points
    {
        _edges = new List<Edge<GameObject>>();

        _edges.Add(new Edge<GameObject>(_nodes[0], _nodes[1]));
        _edges.Add(new Edge<GameObject>(_nodes[1], _nodes[2]));
        _edges.Add(new Edge<GameObject>(_nodes[2], _nodes[3]));
        _edges.Add(new Edge<GameObject>(_nodes[1], _nodes[3]));

        _undirectedGraph = new UndirecteGraph<GameObject, Edge<GameObject>>(_edges);
        _edgeLines = new List<GameObject>();
        ResetGraphLines();
    }

    // Update is called once per frame
    void Update()
    {

        ResetGraphLines();

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButton(0) && Physics.Raycast(ray, out hit))
        {
            GameObject objectHit = hit.transform.gameObject;
            if (objectHit.tag == "Node")
            {
                objectHit.GetComponent<MeshRenderer>().material = _materials[0];
                foreach (var vertex in _undirectedGraph.GetNeighbourVertices(objectHit))
                {
                    vertex.GetComponent<MeshRenderer>().material = _materials[1];
                }

                Dijkstra<GameObject, Edge<GameObject>> dijkstra = new Dijkstra<GameObject, Edge<GameObject>>(_undirectedGraph);
                dijkstra.DijkstraCalculateWeights(_nodes[0]);
                
            }

            // Do something with the object that was hit by the raycast.
        }
    }

    void ResetGraphLines()
    {
        _edgeLines.ForEach(e => GameObject.Destroy(e));
        _edgeLines.Clear();
        List<Edge<GameObject>> edges = _undirectedGraph.GetEdges();
        foreach (var edge in edges)
        {
            GameObject edgeLine = new GameObject($"Edge {_edgeLines.Count}");
            LineRenderer renderer = edgeLine.AddComponent<LineRenderer>();
            renderer.SetPosition(0, edge.Source.transform.position);
            renderer.SetPosition(1, edge.Target.transform.position);
            renderer.startWidth = 0.2f;
            renderer.startColor = new Color(1f, 0f, 0f);
            renderer.endWidth = 0.2f;
            renderer.startColor = new Color(0f, 1f, 0f);
            _edgeLines.Add(edgeLine);
        }
    }
}
