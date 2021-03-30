using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathingInfo
{
    public bool throughobjects;
    public int length;
    public bool success;
    private List<(int, int)> coordinates;

    public PathingInfo()
    {
        throughobjects = false;
        length = 0;
        success = false;
        coordinates = new List<(int, int)>();
    }

    public PathingInfo(int x, int y, PathingInfo prev)
    {
        throughobjects = prev.throughobjects;
        length = prev.length;
        success = prev.success;
        coordinates = prev.coordinates;
        coordinates.Add((x, y));
    }

    public (int, int) GetCoordinates(int index)
    {
        return coordinates[index];
    }

    public int GetX(int index)
    {
        return coordinates[index].Item1;
    }

    public int GetY(int index)
    {
        return coordinates[index].Item2;
    }

    public void AddCoordinates(int x, int y)
    {
        var add = (x, y);
        coordinates.Add(add);
        ++length;
    }
}
