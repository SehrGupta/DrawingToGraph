using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRenderer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private float counter;
    private float dist;

    public Transform Source;
    public Transform End;

    public float lineDarwSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        //Get component lineRenderer
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, Source.position);
        lineRenderer.SetWidth(0.45f, 0.45f);

        //Check distance between source to end to animate
        dist = Vector3.Distance(Source.position, End.position);
    }

    // Update is called once per frame
    /// <summary>
    ///To animate the line drawing
    /// </summary>
    void Update()
    {
        if(counter <dist)
        {
            //Adding to itself each frame
            counter += 0.1f / lineDarwSpeed;

            float x = Mathf.Lerp(0, dist, counter);

            Vector3 pointA = Source.position;
            Vector3 pointB = End.position;

            ////To calculate position on each point of line while animating
            ////Get unit vector in desired direction, multiply by the desired length and add the starting point
            Vector3 pointAlongLine = x * Vector3.Normalize(pointB - pointA) + pointA;

            lineRenderer.SetPosition(1, pointAlongLine);
        }
    }
}
