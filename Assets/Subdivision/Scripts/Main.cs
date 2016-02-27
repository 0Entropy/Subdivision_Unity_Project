using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using System.Collections;
using Subdivision.Core;

public class Main : MonoBehaviour {

    public Text text;
    public MeshFilter meshFilter;
    MeshConverter converter;
    LoopSubdivider subdivider;

    Shape shape;

    // Use this for initialization
    void Start()
    {

        converter = new MeshConverter();
        subdivider = new LoopSubdivider();
        shape = converter.OnConvert(meshFilter.mesh);
        text.text = shape.AllPoints.Count.ToString();
    } 
    
    public void HandleOnSundive()
    {
        shape = subdivider.Subdivide(shape);
        text.text = shape.AllPoints.Count.ToString();
        meshFilter.mesh = converter.ConvertToMesh(shape);
    }
}

