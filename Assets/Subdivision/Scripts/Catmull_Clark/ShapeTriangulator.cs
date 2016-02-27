using System.Collections.Generic;

namespace Subdivision.Core
{
    public abstract class ShapeTriangulator
    {
        public virtual void Triangulate(Shape shape)
        {
            foreach (Face face in shape.Faces)
            {
                List<Point> points = face.AllPoints;

                List<Vector3> positions = new List<Vector3>(points.Count);
                List<Vector3> normals = new List<Vector3>(points.Count);

                foreach (Point point in points)
                {
                    positions.Add(point.Position);
                    normals.Add(point.Normal);
                }

                AddFace(face, positions, normals);
            }
        }

        protected abstract void AddFace(Face face, List<Vector3> positions, List<Vector3> normals);
    }
}