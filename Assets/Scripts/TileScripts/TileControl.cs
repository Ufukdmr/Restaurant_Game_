using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileControl
{
    public Vector3Int Position { get; set; }

    public int I { get; set; }

    public TileControl(Vector3Int position, int i)
    {
        this.Position = position;
        this.I = i;
    }

    public override bool Equals(object obj)
    {
        return obj is TileControl control &&
               Position.Equals(control.Position) &&
               I == control.I;
    }

    public override int GetHashCode()
    {
        int hashCode = -1297133538;
        hashCode = hashCode * -1521134295 + Position.GetHashCode();
        hashCode = hashCode * -1521134295 + I.GetHashCode();
        return hashCode;
    }
}
