using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class UnityVectorUtils
{
    public static UnityEngine.Vector3 Average(IEnumerable<Vector3> vectors)
    {
        return
            new UnityEngine.Vector3(
                vectors.Average(v => v.x),
                vectors.Average(v => v.y),
                vectors.Average(v => v.z));
    }
}
