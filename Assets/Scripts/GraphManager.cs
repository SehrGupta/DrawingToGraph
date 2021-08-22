using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EasyGraph;

public class GraphManager : MonoBehaviour
{

    List<Room> _rooms;
    List<Connection> _connections;
    UndirecteGraph<Room, Edge<Room>> _graph;
    bool _relax = false;
    List<GameObject> _edgeLines = new List<GameObject>();


    // Update is called once per frame
    void Update()
    {
        if (_relax)
        {
            foreach (var room in _rooms)
            {
                room.CalculateVelocity(_graph, 0.1f);
                room.MoveRoom();
                CreateGraphLines();
            }
        }

    }

    public void CreateGraph(List<Room> rooms, List<Connection> connections)
    {
        _rooms = rooms;
        _connections = connections;
        //Check if there are actually rooms to make a graph
        if (_rooms.Count < 2)
        {
            return;
        }

        //UndirecteGraph<Room, Edge<Room>> graph;
        List<Edge<Room>> edges = new List<Edge<Room>>();

       foreach (var connection in _connections)
       {
             Edge<Room> edge = new Edge<Room>(connection.Source, connection.End);
             if (Util.FunctionAttraction.ContainsKey((connection.Source.SelectedFunction, connection.End.SelectedFunction)))
             {

                 edge.Weight = Util.FunctionAttraction[(connection.Source.SelectedFunction, connection.End.SelectedFunction)];
             }
             else if (Util.FunctionAttraction.ContainsKey((connection.End.SelectedFunction, connection.Source.SelectedFunction)))
             {
                 edge.Weight = Util.FunctionAttraction[(connection.End.SelectedFunction, connection.Source.SelectedFunction)];
             }
             else
             {
                 Debug.Log("Setting default weight");
                 edge.Weight = 5f;
             }


             edges.Add(edge);
       }

  

        _graph = new UndirecteGraph<Room, Edge<Room>>(edges);

        CreateGraphLines();
        _relax = true;
    }

    public void CreateGraphLines()
    {

        _edgeLines.ForEach(e => GameObject.Destroy(e));
        _edgeLines.Clear();
        List<Edge<Room>> edges = _graph.GetEdges();
        //Edge<Room> edge = new Edge<Room>(connection.Source, connection.End);

        foreach (var edge in edges)
        {
            GameObject edgeLine = new GameObject($"Edge {_edgeLines.Count}");
            LineRenderer renderer = edgeLine.AddComponent<LineRenderer>();
            renderer.SetPosition(0, edge.Source.CentrePoint);
            renderer.SetPosition(1, edge.Target.CentrePoint);
            renderer.startWidth = 0.2f;
            renderer.startColor = new Color(98f, 250f, 145f);
            renderer.endWidth = 0.2f;
            renderer.startColor = new Color(98f, 250f, 145f);
            _edgeLines.Add(edgeLine);
        }
    }
}
