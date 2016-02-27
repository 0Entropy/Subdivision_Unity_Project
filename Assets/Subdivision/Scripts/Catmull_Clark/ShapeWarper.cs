using System;
using System.Collections.Generic;

namespace Subdivision.Core
{
    /// <summary>
    /// Takes all vertices in a shape and warps them according to a given function. 
    /// </summary>
    public class ShapeWarper
    {
        /* http://www.michael-hansmeyer.com/projects/platonic_solids_info2.html?screenSize=1&color=0#undefined
  The processes at the core of this project are the Catmull-Clark and Doo-Sabin algorithms. 
  * They were both conceived of in the late 1970's with the aim of generating smooth 
  * surfaces from coarses polygonal meshes.

     The processes can be understood by considering their two parts: topological rules and weighting rules. 
  * The topological rules specify how to obtain the combinatorics of the refined mesh from the 
  * combinatorics of the input mesh by generating new vertices, edges and faces. The weighting 
  * rules specify how to calculate the positions of these new vertices based on interpolation 
  * between vertices of the input mesh.

   By introducing parameters to allow for variations in these weighting rules, non-rounded forms with 
  * highly diverse attributes can be produced. Whereas the traditional weighting rules 
  * specify the positions of new vertices strictly as interpolations of previous-generation 
  * vertices, these rules are amended to allow for extrusion along face, edge and vertex 
  * normals. It is primarily through these two changes to the established schemes the 
  * complex geometries in this project become possible. */

        public void Warp(Shape shape, double size)
        {
            List<Point> points = shape.AllPoints;

            double sizeDivisor = 1 / size;

            // Whereas the traditional weighting rules specify the positions of new vertices strictly as interpolations 
            // of previous-generation vertices, these rules are amended to allow for extrusion along face, edge and vertex 
            // normals.

            foreach (Point point in points)
            {
                point.Position -= 2 * point.Normal; // Warp(point.Position * sizeDivisor) * size;

                foreach (Face face in point.AllFaces)
                {
                    point.Position += 2 * face.Normal;
                }
            }
        }

        private Vector3 Warp(Vector3 position)
        {
            return position;
            //return position * 0.8 + (Swirl(position) * 2 + Horseshoe(position)) * 0.2;
        }

        private Vector3 Horseshoe(Vector3 current)
        {
            double radius = Math.Sqrt((current.X * current.X) + (current.Y * current.Y));
            double angle = Math.Atan2(current.Y, current.X);
            return new Vector3(radius * Math.Cos(2 * angle), radius * Math.Sin(2 * angle), current.Z);
        }

        private Vector3 Swirl(Vector3 position)
        {
            const double swirlFactor = 3.0;

            double radius = Math.Sqrt((position.X * position.X) + (position.Z * position.Z));
            double angle = Math.Atan2(position.Z, position.X);
            double inner = angle - Math.Cos(radius * swirlFactor);

            return new Vector3(radius * Math.Cos(inner), position.Y, radius * Math.Sin(inner));
        }
    }
}