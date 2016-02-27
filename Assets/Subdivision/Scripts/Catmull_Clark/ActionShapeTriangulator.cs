using System;
using System.Collections.Generic;

namespace Subdivision.Core
{
    public class ActionShapeTriangulator : ShapeTriangulator
    {
        private readonly Action<List<Vector3>, List<Vector3>> _addTriangle;

        public ActionShapeTriangulator(Action<List<Vector3>, List<Vector3>> addTriangle)
        {
            _addTriangle = addTriangle;
        }

        protected override void AddFace(Face face, List<Vector3> positions, List<Vector3> normals)
        {
            _addTriangle(positions, normals);
        }
    }
}