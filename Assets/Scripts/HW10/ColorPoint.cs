using System;
using UnityEngine;

[Serializable]
public class ColorPoint
{
    public Vector3 Position;
    public Color Color;

}

[Serializable]
public class ColorPoints
{
    public ColorPoint[] Points;
}
