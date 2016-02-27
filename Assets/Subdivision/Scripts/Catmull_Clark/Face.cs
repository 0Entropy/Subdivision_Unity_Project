using System;
using System.Collections.Generic;
using System.Linq;

namespace Subdivision.Core
{
    public class Face
    {
        private readonly Lazy<Vector3> _lazyNormal;
        private readonly Lazy<List<Point>> _lazyAllPoints;

        public Face(params Edge[] edges)
        {
            Edges = new List<Edge>(edges);
            Edges.ForEach(e => e.AddFace(this));
            _lazyNormal = new Lazy<Vector3>(ComputeNormal);
            _lazyAllPoints = new Lazy<List<Point>>(GetAllPointsInWindingOrder);
        }

        public List<Edge> Edges { get; set; }
        public List<Point> AllPoints { get { return _lazyAllPoints.Value; } }
        public Vector3 Normal { get { return _lazyNormal.Value; } }

        public Point FacePoint { get; set; }

        public bool IsMatchFor(Face face)
        {
            return face.Edges.All(e => Edges.Contains(e));
        }

        private List<Point> GetAllPointsInWindingOrder()
        {
            //return Edges.SelectMany(e => e.Points).Distinct().ToList();
            List<Point> points = new List<Point>();
            // The edges were added in order, but we don't know if the points within the edges are in order. 
            // Therefore we look at the previous edge to figoure out which point has allready been visited

            Edge previous = Edges.First();
            foreach (Edge current in Edges.Skip(1).Take(Edges.Count - 2))
            {
                if (points.Count == 0)
                {
                    Point firstPoint = previous.PointOnlyInThis(current);
                    points.Add(firstPoint);
                    Point secondPoint = previous.PointInBoth(current);
                    points.Add(secondPoint);
                }

                Point nextPoint = current.PointOnlyInThis(previous);
                points.Add(nextPoint);

                previous = current;
            }

            return points;
        }

        private Vector3 ComputeNormal()
        {
            List<Point> points = GetAllPointsInWindingOrder();
            Vector3 v1 = points[0].Position;
            Vector3 v2 = points[1].Position;
            Vector3 v3 = points[2].Position;
            return (v2 - v1).CrossProduct(v3 - v1).Normalized();
        }
    }
}