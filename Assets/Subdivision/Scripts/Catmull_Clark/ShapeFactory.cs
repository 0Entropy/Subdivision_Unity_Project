using System.Collections.Generic;

namespace Subdivision.Core
{
    public class ShapeFactory
    {
        public Shape BuildBox(double x, double y, double z)
        {
            Shape shape = new Shape();

            Point[,,] points = CreateBoxPoints(x, y, z);
            List<Edge> edges = new List<Edge>();

            // Using OGRE which uses counter clockwise winding / anti clickwise winding. Our faces are thus CCW.
            // http://www.ogre3d.org/docs/api/html/classOgre_1_1ManualObject.html
            // This is important for determining frontfacing and backfacing faces. 
            // Get it wrong and faces become invisible and the manifold is broken.
            shape.AddFace(SubdivisionUtilities.CreateFaceF(edges, points[0, 0, 0], points[1, 0, 0], points[1, 0, 1], points[0, 0, 1]));
            shape.AddFace(SubdivisionUtilities.CreateFaceR(edges, points[0, 1, 0], points[1, 1, 0], points[1, 1, 1], points[0, 1, 1]));

            shape.AddFace(SubdivisionUtilities.CreateFaceF(edges, points[0, 0, 0], points[0, 1, 0], points[1, 1, 0], points[1, 0, 0]));
            shape.AddFace(SubdivisionUtilities.CreateFaceR(edges, points[0, 0, 1], points[0, 1, 1], points[1, 1, 1], points[1, 0, 1]));

            shape.AddFace(SubdivisionUtilities.CreateFaceR(edges, points[0, 0, 0], points[0, 1, 0], points[0, 1, 1], points[0, 0, 1]));
            shape.AddFace(SubdivisionUtilities.CreateFaceR(edges, points[1, 0, 0], points[1, 0, 1], points[1, 1, 1], points[1, 1, 0]));
            return shape;
        }

        private static Point[,,] CreateBoxPoints(double x, double y, double z)
        {
            Point[,,] points = new Point[2, 2, 2];

            for (int ax = 0; ax <= 1; ax++)
            {
                for (int ay = 0; ay <= 1; ay++)
                {
                    for (int az = 0; az <= 1; az++)
                    {
                        double fx = ax == 0 ? -x * 0.5f : +x * 0.5f;
                        double fy = ay == 0 ? -y * 0.5f : +y * 0.5f;
                        double fz = az == 0 ? -z * 0.5f : +z * 0.5f;
                        points[ax, ay, az] = new Point(fx, fy, fz);
                    }
                }
            }
            return points;
        }
    }
}