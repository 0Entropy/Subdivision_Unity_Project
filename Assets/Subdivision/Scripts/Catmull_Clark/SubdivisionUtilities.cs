using System;
using System.Collections.Generic;
using System.Linq;

namespace Subdivision.Core
{
    public static class SubdivisionUtilities
    {
        public static Edge[] GetOrCreateEdges(List<Edge> existingEdges, params Point[] points)
        {
            List<Edge> edges = new List<Edge>();

            Point first = points.First();
            Point previous = first;
            foreach (Point point in points.Skip(1))
            {
                Edge edge = GetOrCreateEdge(existingEdges, previous, point);
                edges.Add(edge);
                previous = point;
            }

            edges.Add(GetOrCreateEdge(existingEdges, previous, first));

            return edges.ToArray();
        }

        public static Edge GetOrCreateEdge(List<Edge> existingEdges, Point p1, Point p2)
        {
            Edge edge = existingEdges.SingleOrDefault(e => e.IsMatchFor(p1, p2));

            if (edge == null)
            {
                edge = new Edge(p1, p2);
                existingEdges.Add(edge);
            }
            return edge;
        }

        public static Face CreateFaceF(List<Edge> edges, params Point[] points)
        {
            return new Face(GetOrCreateEdges(edges, points));
        }

        public static Face CreateFaceR(List<Edge> edges, params Point[] points)
        {
            return new Face(GetOrCreateEdges(edges, points.Reverse().ToArray()));
        }

        public static void VerifyThatThereAreNoEdgeDuplicates(List<Edge> edges)
        {
            // Debug code
            //foreach (Edge edge in edges)
            //{
            //    Point p1 = edge.Points[0];
            //    Point p2 = edge.Points[1];
            //    int c1 = edges.Count(e => e.Points[0].Position.Equals(p1.Position) && e.Points[1].Position.Equals(p2.Position));
            //    int c2 = edges.Count(e => e.Points[0].Position.Equals(p2.Position) && e.Points[1].Position.Equals(p1.Position));

            //    if (c1 + c2 != 1)
            //    {
            //        throw new InvalidOperationException("There are edge duplicates!");
            //    }
            //}
        }
    }
}