using System;
using System.Collections.Generic;

namespace Subdivision.Core
{
    public class Edge
    {
        public Edge(Point p1, Point p2)
        {
            if (p1 == p2)
            {
                throw new InvalidOperationException("Edge must be between two different points!");
            }
            Points = new List<Point> { p1, p2 };
            Faces = new List<Face>();
            p1.Edges.Add(this);
            p2.Edges.Add(this);
        }

        public List<Point> Points { get; set; }
        public List<Face> Faces { get; set; }
        public Point EdgePoint { get; set; }
        public Vector3 Middle { get { return (Points[0].Position + Points[1].Position) * 0.5f; } }               

        public bool IsOnBorderOfHole
        {
            get
            {
                //  an edge is the border of a hole if it belongs to only one face,
                return Faces.Count == 1;
            }
        }

        public bool IsMatchFor(Point p1, Point p2)
        {
            Point a1 = Points[0];
            Point a2 = Points[1];
            return
                (a1.Position.Equals( p1.Position) 
                && a2.Position.Equals(p2.Position)) ||
                (a1.Position.Equals(p2.Position)
                && a2.Position.Equals(p1.Position));
        }

        public bool IsMatchFor(Edge edge)
        {
            return IsMatchFor(edge.Points[0], edge.Points[1]);
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}", Points[0], Points[1]);
        }

        public void AddFace(Face face)
        {
            if (Faces.Contains(face))
            {
                throw new InvalidOperationException("Edge allready contains face!");
            }
            Faces.Add(face);
        }

        public Point PointInBoth(Edge other)
        {
            Point p1 = Points[0];
            if (other.Points.Contains(p1))
            {
                return p1;
            }
            else
            {
                return Points[1];
            }
        }

        public Point PointOnlyInThis(Edge other)
        {
            Point p1 = Points[0];
            if (!other.Points.Contains(p1))
            {
                return p1;
            }
            else
            {
                return Points[1];
            }
        }
    }
}