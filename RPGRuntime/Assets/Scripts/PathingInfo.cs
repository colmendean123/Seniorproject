using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathingInfo
{
    public (int, int) position { get; }
    public int distance { get; }

    public PathingInfo((int, int) position,int distance)
    {
        this.position = position;
        this.distance = distance;
    }
}
