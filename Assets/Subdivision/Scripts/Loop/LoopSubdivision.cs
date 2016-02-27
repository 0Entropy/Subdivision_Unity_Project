
using UnityEngine;
using System.Collections.Generic;

/*

    I am trying to implement Loop Subdivision algorithm in C#. 
    http://www.cs.cmu.edu/afs/cs/academic/class/15462-s13/www/lec_slides/project2_slides.pdf


    How can i keep track of all the edges in which I have found out the new vertex. Here's my code to load Json file of the wireframe. 
    I have created a new class subdivision to subdivide the mesh, but it is not working 
    as there are errors. Can anyone please throw some light on how can I subdivide the mesh.

    http://stackoverflow.com/questions/32937044/loop-subdivision-in-c-sharp-and-opentk
*/
class LoopSubdivision
{
    /*public static Mesh[] subdivsion(Mesh[] meshsub)
    {
        
        var meshes = new List<Mesh>();
        //Vertices of a mesh
        //Hashtable edges = new Hashtable();


        int verticescount = getnumvertices(meshsub);
        //Console.WriteLine(verticescount);
        int facecount = getnumfaces(meshsub);
        int edgecount = verticescount + facecount - 2;
        int newvercount = verticescount + edgecount;
        int newfacecount = facecount * 4;
        Vector3[] NewVertices = new Vector3[newvercount];
        var meshnew = new Mesh("subdmesh", newvercount, newfacecount);


        foreach (Mesh mesh in meshsub)
        {

            //for (var j = 0; j < verticescount; j++)
            //{
            //    Console.WriteLine(mesh.Vertices[j]);
            //    NewVertices[j] = mesh.Vertices[j];
            //}



            foreach (Mesh mesh2 in meshsub)
            {
                //for (var index = 0; index < facecount; index++)
                //{
                //    foreach (var faces in mesh.Faces)
                //    {
                //        meshnew.Faces[index] = mesh.Faces[index];
                //    }
                //}
                int i = 0;
                foreach (var face in mesh.Faces)
                {
                    var P0 = face.A;
                    var P1 = face.B;
                    var P2 = face.C;
//                     Console.WriteLine("Faces");
//                     Console.WriteLine(P0);
//                     Console.WriteLine(P1);
//                     Console.WriteLine(P2);

                    NewVertices[i] = getfourthvert(P0, P1, P2, meshsub);
                    NewVertices[i + 1] = getfourthvert(P1, P2, P0, meshsub);
                    NewVertices[i + 2] = getfourthvert(P2, P0, P1, meshsub);
                    i = i + 3;





                    for (var index = verticescount; index < newvercount; index++)
                    {
                        meshnew.Vertices[index] = NewVertices[index];
                    }
                    / *         for(var index = facecount; index < newfacecount; index++)
                             {
                                 var a = face.A;
                                 var b = (int)indicesArray[index * 3 + 1].Value;
                                 var c = (int)indicesArray[index * 3 + 2].Value;
                                 mesh.Faces[index] = new Face { A = a, B = b, C = c };
                             }* /


                    meshes.Add(meshnew);
                }


                int n = 6;
                double num = (3.0f + 2.0f * Mathf.Cos(2.0f * Mathf.PI / n));
                double beta = 1.0 / n * (5.0 / 8.0 - num * num / 64.0);

            }
        }
        return meshes.ToArray();
    }


    private static int getnumfaces(Mesh[] meshsub)
    {
        int count = 0;
        foreach (Mesh mesh in meshsub)
        {
            foreach (var face in mesh.Faces)
                count = count + 1;
        }
        return count;

    }

    private static int getnumvertices(Mesh[] meshsub)
    {
        int count = 0;
        foreach (Mesh mesh in meshsub)
        {
            foreach (var vert in mesh.Vertices)
                count = count + 1;
        }
        return count;
    }

    private static Vector3 getfourthvert(int X0, int X1, int X2, Mesh[] meshsub)
    {
        int X3;
        Vector3 V4 = new Vector3(0, 0, 0);
        foreach (Mesh mesh in meshsub)
        {

            foreach (var face2 in mesh.Faces)
            {
                var V0 = mesh.Vertices[X0];
                var V1 = mesh.Vertices[X1];
                var V2 = mesh.Vertices[X2];
                var V3 = mesh.Vertices[0];

                if ((X0 == face2.A) && (X1 == face2.B))
                {
                    var temp = face2.C;

                    if (temp != X2)
                    {
                        X3 = temp;
                        V3 = mesh.Vertices[X3];
                        V4 = (3 * V0 + 3 * V1 + V2 + V3) / 8;
                    }

                }

            }

        }

        Console.WriteLine(V4);
        return V4;
    }*/



}
