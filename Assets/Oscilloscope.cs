using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscilloscope : MonoBehaviour
{

    // Creates a line renderer that follows a Sin() function
    // and animates it.

    //public Color c1 = Color.yellow;
    //public Color c2 = Color.red;
    [Range(1, 500)]
    public int lengthOfLineRenderer = 20;

    //Multipliers
    [Range(0f, 1f)]
    public float widthMultiplier = 0.5f;
    [Range(0.01f, 360f)]
    public float heightMultiplier = 1f;
    public AudioSource A;
    private float[] sound;
    public LineRenderer lineRenderer;
    void Start()
    {
        
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.widthMultiplier = 0.2f;

    }

    void Update()
    {
        lineRenderer.positionCount = lengthOfLineRenderer;
        var t = Time.time;
        sound = new float[lineRenderer.positionCount];
        A.GetOutputData(sound, 1);
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(i * widthMultiplier, Mathf.Sin(((i * heightMultiplier) + t)), 0.0f));
            lineRenderer.SetPosition(i, (new Vector3(i * widthMultiplier, sound[i], 0.0f)));
        }
    }
}