using System;
using System.Collections.Generic;
using System.Linq;

namespace Subdivision.Core
{
    public class Shape
    {
        private readonly Lazy<List<Edge>> _lazyAllEdges;
        private readonly Lazy<List<Point>> _lazyAllPoints;

        public Shape()
        {
            Faces = new List<Face>();

            _lazyAllEdges = new Lazy<List<Edge>>(GetAllEdges);
            _lazyAllPoints = new Lazy<List<Point>>(GetAllPoints);
        }

        public List<Face> Faces { get; set; }
        public List<Edge> AllEdges { get { return _lazyAllEdges.Value; } }
        public List<Point> AllPoints { get { return _lazyAllPoints.Value; } }

        public Face AddFace(Face face)
        {   
            if (Faces.Any(f => f.IsMatchFor(face)))
            {
                throw new InvalidOperationException("There is allready such a face in the shape!");
            }
            Faces.Add(face);
            return face;
        }

        public void RemoveFace(Face face)
        {
            Faces.Remove(face);
            face.Edges.ForEach(e => e.Faces.Remove(face));
        }

        private List<Edge> GetAllEdges()
        {
            return Faces.SelectMany(f => f.Edges).Distinct().ToList();
        }

        private List<Point> GetAllPoints()
        {
            return Faces.SelectMany(f => f.AllPoints).Distinct().ToList();
        }
    }
}