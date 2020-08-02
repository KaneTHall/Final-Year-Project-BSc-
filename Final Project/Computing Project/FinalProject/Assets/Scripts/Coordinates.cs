using UnityEngine;
using System.Collections;
[System.Serializable]
//Struct to manipulate two integers as one
public struct Coordinates
{
    //define int variables for x and z coordinates.
    public int x;
    public int z;

    public Coordinates(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    //operator to add coordinates
    public static Coordinates operator +(Coordinates V1, Coordinates V2)
    {
        V1.x += V2.x;
        V1.z += V2.z;
        return V1;
    }
}
