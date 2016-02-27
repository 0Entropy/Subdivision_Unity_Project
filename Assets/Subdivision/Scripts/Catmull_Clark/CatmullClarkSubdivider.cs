using System;
using System.Collections.Generic;
using System.Linq;


namespace Subdivision.Core
{
    public class CatmullClarkSubdivider : ISubdivider
    {
        public Shape Subdivide(Shape shape)
        {
            // http://rosettacode.org/wiki/Catmull%E2%80%93Clark_subdivision_surface 
            Shape subdivided = new Shape();

            //for each face, a face point is created which is the average of all the points of the face.
            CreateFacePoints(shape);

            //for each edge, an edge point is created which is the average between the center of 
            //  the edge and the center of the segment made with the face points of the two adjacent faces.
            CreateEdgePoints(shape);

            //for each vertex point, its coordinates are updated from (new_coords):
            //    the old coordinates (old_coords),
            //    the average of the face points of the faces the point belongs to (avg_face_points),
            //    the average of the centers of edges the point belongs to (avg_mid_edges),
            //    how many faces a point belongs to (n), then use this formula: 
            //m1 = (n - 3) / n
            //m2 = 1 / n
            //m3 = 2 / n
            //new_coords = (m1 * old_coords)
            //           + (m2 * avg_face_points)
            //           + (m3 * avg_mid_edges)
            CreateVertexPoints(shape);

            //for a triangle face (a,b,c): 
            //   (a, edge_pointab, face_pointabc, edge_pointca)
            //   (b, edge_pointbc, face_pointabc, edge_pointab)
            //   (c, edge_pointca, face_pointabc, edge_pointbc)

            //for a quad face (a,b,c,d): 
            //   (a, edge_pointab, face_pointabcd, edge_pointda)
            //   (b, edge_pointbc, face_pointabcd, edge_pointab)
            //   (c, edge_pointcd, face_pointabcd, edge_pointbc)
            //   (d, edge_pointda, face_pointabcd, edge_pointcd)
            CreateFaces(shape, subdivided);

            return subdivided;
        }

        private void CreateFacePoints(Shape shape)
        {
            //for each face, a face point is created which is the average of all the points of the face.
            foreach (Face face in shape.Faces)
            {
                List<Point> points = face.AllPoints;
                face.FacePoint = new Point(Average(points));
                //UnityEngine.Debug.Log(face.FacePoint.Position);
            }
        }

        private void CreateEdgePoints(Shape shape)
        {
            //for each edge, an edge point is created which is the average between the center of 
            //  the edge and the center of the segment made with the face points of the two adjacent faces.
            List<Edge> edges = shape.AllEdges;
            foreach (Edge edge in edges)
            {
                if (edge.IsOnBorderOfHole)
                {
                    Vector3 position =
                        Average(
                            edge.Points[0],
                            edge.Points[1]);

                    edge.EdgePoint = new Point(position);
                }
                else
                {
                    Vector3 position =
                        Average(
                            edge.Points[0],
                            edge.Points[1],
                            edge.Faces[0].FacePoint,
                            edge.Faces[1].FacePoint);

                    edge.EdgePoint = new Point(position);
                }
            }
        }

        private void CreateVertexPoints(Shape shape)
        {
            //for each vertex point, its coordinates are updated from (new_coords):
            //    the old coordinates (old_coords),
            //    the average of the centers of edges the point belongs to (avg_mid_edges),
            //    the average of the face points of the faces the point belongs to (avg_face_points),
            //    how many faces a point belongs to (n), then use this formula: 
            //m1 = (n - 3) / n
            //m2 = 1 / n
            //m3 = 2 / n
            //new_coords = (m1 * old_coords)
            //           + (m2 * avg_face_points)
            //           + (m3 * avg_mid_edges) 
            List<Point> allPoints = shape.AllPoints;
            List<Edge> allEdges = shape.AllEdges;

            foreach (Point oldPoint in allPoints)
            {
                if (oldPoint.IsOnBorderOfHole)
                {
                    oldPoint.Successor = CreateVertexPointForBorderPoint(oldPoint);
                }
                else
                {
                    oldPoint.Successor = CreateVertexPoint(allEdges, oldPoint);
                }
            }
        }

        private Point CreateVertexPoint(List<Edge> allEdges, Point oldPoint)
        {
            //    the average of the face points of the faces the point belongs to (avg_face_points),
            //    how many faces a point belongs to (n), then use this formula: 

            //    the average of the centers of edges the point belongs to (avg_mid_edges),
            Vector3 avgMidEdges = Vector3.Average(oldPoint.Edges.Select(e => e.Middle));

            //    the average of the face points of the faces the point belongs to (avg_face_points),
            List<Face> pointFaces = oldPoint.AllFaces;
            Vector3 avgFacePoints = Average(pointFaces.Select(pf => pf.FacePoint));

            int faceCount = pointFaces.Count;

            double m1 = (faceCount - 3f) / faceCount;
            double m2 = 1f / faceCount;
            double m3 = 2f / faceCount;

            Vector3 position = m1 * oldPoint.Position + m2 * avgFacePoints + m3 * avgMidEdges;
         
            return new Point(position);
        }

        private Point CreateVertexPointForBorderPoint(Point oldPoint)
        {
            //for the vertex points that are on the border of a hole, the new coordinates are calculated as follows:
            // in all the edges the point belongs to, only take in account the middles of the edges that are on the border of the hole
            // calculate the average between these points (on the hole boundary) and the old coordinates (also on the hole boundary). 
            List<Vector3> positions = oldPoint.Edges.Where(e => e.IsOnBorderOfHole).Select(e => e.Middle).ToList();
            positions.Add(oldPoint.Position);

            return new Point(Vector3.Average(positions));
        }

        private void CreateFaces(Shape shape, Shape subdivided)
        {
            List<Face> faces = shape.Faces;
            List<Edge> existingEdges = new List<Edge>();
            foreach (Face face in faces)
            {
                if (face.AllPoints.Count() == 3)
                {
                    CreateTriangleFace(existingEdges, subdivided, face);
                }
                else if (face.AllPoints.Count() == 4)
                {
                    CreateQuadFace(existingEdges, subdivided, face);
                }
                else
                {
                    throw new InvalidOperationException(string.Format("Unhandled facetype (point count={0})!", face.AllPoints.Count()));
                }
            }
            SubdivisionUtilities.VerifyThatThereAreNoEdgeDuplicates(existingEdges);
        }

        private void CreateTriangleFace(List<Edge> existingEdges, Shape subdivided, Face face)
        {
            List<Point> points = face.AllPoints;
            Point a = points[0].Successor;
            Point b = points[1].Successor;
            Point c = points[2].Successor;

            //for a triangle face (a,b,c): 
            //   (a, edge_pointab, face_pointabc, edge_pointca)
            //   (b, edge_pointbc, face_pointabc, edge_pointab)
            //   (c, edge_pointca, face_pointabc, edge_pointbc)
            Point facePoint = face.FacePoint;

            subdivided.Faces.Add(SubdivisionUtilities.CreateFaceF(existingEdges, a, face.Edges[0].EdgePoint, face.Edges[2].EdgePoint));
            subdivided.Faces.Add(SubdivisionUtilities.CreateFaceF(existingEdges, b, face.Edges[1].EdgePoint, face.Edges[0].EdgePoint));
            subdivided.Faces.Add(SubdivisionUtilities.CreateFaceF(existingEdges, c, face.Edges[2].EdgePoint, face.Edges[1].EdgePoint));
            subdivided.Faces.Add(SubdivisionUtilities.CreateFaceF(existingEdges, face.Edges[0].EdgePoint, face.Edges[1].EdgePoint, face.Edges[2].EdgePoint));

            SubdivisionUtilities.VerifyThatThereAreNoEdgeDuplicates(existingEdges);
        }

        private void CreateQuadFace(List<Edge> existingEdges, Shape subdivided, Face face)
        {
            //                  0 1 2 -> 3 
            //for a quad face (a,b,c,d): 
            //   (a, edge_pointab, face_pointabcd, edge_pointda)
            //   (b, edge_pointbc, face_pointabcd, edge_pointab)
            //   (c, edge_pointcd, face_pointabcd, edge_pointbc)
            //   (d, edge_pointda, face_pointabcd, edge_pointcd)
            List<Point> points = face.AllPoints;
            Point a = points[0].Successor;
            Point b = points[1].Successor;
            Point c = points[2].Successor;
            Point d = points[3].Successor;

            Point facePoint = face.FacePoint;

            subdivided.Faces.Add(SubdivisionUtilities.CreateFaceF(existingEdges, a, face.Edges[0].EdgePoint, facePoint, face.Edges[3].EdgePoint));
            subdivided.Faces.Add(SubdivisionUtilities.CreateFaceF(existingEdges, b, face.Edges[1].EdgePoint, facePoint, face.Edges[0].EdgePoint));
            subdivided.Faces.Add(SubdivisionUtilities.CreateFaceF(existingEdges, c, face.Edges[2].EdgePoint, facePoint, face.Edges[1].EdgePoint));
            subdivided.Faces.Add(SubdivisionUtilities.CreateFaceF(existingEdges, d, face.Edges[3].EdgePoint, facePoint, face.Edges[2].EdgePoint));

            SubdivisionUtilities.VerifyThatThereAreNoEdgeDuplicates(existingEdges);
        }

        private Vector3 Average(IEnumerable<Point> points)
        {
            return Vector3.Average(points.Select(p => p.Position));
        }

        private Vector3 Average(params Point[] points)
        {
            return Vector3.Average(points.Select(p => p.Position));
        }
    }
}