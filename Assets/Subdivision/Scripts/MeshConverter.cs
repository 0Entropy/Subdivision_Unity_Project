using UnityEngine;
using System.Collections.Generic;
using Subdivision.Core;

public class MeshConverter
{ 
    public Shape OnConvert(Mesh mesh )
    {
        MeshNormalizer normalizer = new MeshNormalizer(mesh);
        List<UnityEngine.Vector3> vertices = normalizer.UniqueVertices;
        List<int> triangles = normalizer.UniqueTriangles;

        List<Subdivision.Core.Point> points = new List<Subdivision.Core.Point>();

        foreach(var v in normalizer.UniqueVertices)
        {
            points.Add(new Subdivision.Core.Point(v.x, v.y, v.z));
        }

        List<Subdivision.Core.Edge> edges = new List<Subdivision.Core.Edge>();

        Shape shape = new Shape();
        for (int i = 0; i < normalizer.UniqueTriangles.Count / 3; i++)
        {
            Subdivision.Core.Point p0 = points[triangles[3 * i]];
            Subdivision.Core.Point p1 = points[triangles[3 * i + 1]];
            Subdivision.Core.Point p2 = points[triangles[3 * i + 2]];

            Subdivision.Core.Edge e0 = new Subdivision.Core.Edge(p0, p1);
            Subdivision.Core.Edge e1 = new Subdivision.Core.Edge(p1, p2);
            Subdivision.Core.Edge e2 = new Subdivision.Core.Edge(p2, p0);

            bool b0 = true;
            bool b1 = true;
            bool b2 = true;
            foreach (Subdivision.Core.Edge ee in edges)
            {
                if (e0.IsMatchFor(ee)) { e0 = ee; b0 = false; }
                if (e1.IsMatchFor(ee)) { e1 = ee; b1 = false; }
                if (e2.IsMatchFor(ee)) { e2 = ee; b2 = false; }
            }

            if (b0) edges.Add(e0);
            if (b1) edges.Add(e1);
            if (b2) edges.Add(e2);

            shape.AddFace(new Face(e0, e1, e2));

        }

        return shape;

    }

    public Mesh ConvertToMesh(Shape shape)
    {
        Mesh mesh = new Mesh();
        List<UnityEngine.Vector3> vertices = new List<UnityEngine.Vector3>();
        List<int> triangles = new List<int>();

        int n = 0;
        foreach (var face in shape.Faces)
        {
            //Debug.Log(face.AllPoints.Count);
            vertices.AddRange(ConvertToVectors(face.AllPoints.ToArray()));
            if (face.AllPoints.Count == 3)
            {
                triangles.AddRange(new int[] { 3 * n, 3 * n + 1, 3 * n + 2 });
            }
            else if (face.AllPoints.Count == 4)
            {

                triangles.AddRange(new int[] { 4 * n, 4 * n + 1, 4 * n + 2, 4 * n, 4 * n + 2, 4 * n + 3 });
            }
            n++;
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();


        return mesh;
    }

    UnityEngine.Vector3 ConvertToVector(Subdivision.Core.Point pt)
    {
        return new UnityEngine.Vector3((float)pt.Position.X, (float)pt.Position.Y, (float)pt.Position.Z);
    }

    UnityEngine.Vector3[] ConvertToVectors(params Subdivision.Core.Point[] pts)
    {
        List<UnityEngine.Vector3> v3s = new List<UnityEngine.Vector3>();
        foreach (var pt in pts)
        {
            v3s.Add(ConvertToVector(pt));
        }

        return v3s.ToArray();
    }

    Subdivision.Core.Point ConvertToPoint(UnityEngine.Vector3 v3)
    {
        return new Subdivision.Core.Point(v3.x, v3.y, v3.z);
    }
}
