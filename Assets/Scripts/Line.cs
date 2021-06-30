using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line
{
    private LineRenderer _lineRenderer;
    private float _counter;
    private float _dist;

    public Vector3 Source;
    public Vector3 End;

    public float lineDarwSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        //Get component lineRenderer
        //_lineRenderer = GetComponent<LineRenderer>();
        //_lineRenderer.SetPosition(0, Source.position);
        //_lineRenderer.SetWidth(0.45f, 0.45f);

        //Check distance between source to end to animate
        //_dist = Vector3.Distance(Source.position, End.position);
    }

    // Update is called once per frame
    /// <summary>
    ///To animate the line drawing
    /// </summary>
    void Update()
    {
        if (_counter < _dist)
        {
            //Adding to itself each frame
            _counter += 0.1f / lineDarwSpeed;

            float x = Mathf.Lerp(0, _dist, _counter);

            Vector3 pointA = Source;
            Vector3 pointB = End;

            ////To calculate position on each point of line while animating
            ////Get unit vector in desired direction, multiply by the desired length and add the starting point
            Vector3 pointAlongLine = x * Vector3.Normalize(pointB - pointA) + pointA;

            _lineRenderer.SetPosition(1, pointAlongLine);
        }
    }
}
