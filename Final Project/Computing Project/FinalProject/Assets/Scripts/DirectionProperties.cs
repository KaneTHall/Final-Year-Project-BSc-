using UnityEngine;
using System.Collections;

public static class DirectionProperties
{
    public const int numOfDirections = 4;

    //array of directions
    private static Direction[] opposite =
    {
        Direction.South,
        Direction.West,
        Direction.North,
        Direction.East
    };
    //array of rotations
    private static Quaternion[] rotate =
    {
        Quaternion.identity,
        Quaternion.Euler(0f, 90f, 0f),
        Quaternion.Euler(0f, 180f, 0f),
        Quaternion.Euler(0f, 270f, 0f)
    };


    //get opposite direction to input
    public static Direction getOpposite(this Direction d)
    {
        return opposite[(int)d];
    }
    //get rotation
    public static Quaternion getRotation(this Direction d)
    {
        return rotate[(int)d];
    }

    //the numerical direction of each direction
    private static Coordinates[] directionalVectors =
    {
        new Coordinates(0,1),
        new Coordinates(1,0),
        new Coordinates(0,-1),
        new Coordinates(-1,0)
    };
    //return random direction from:North,East,South,West
    public static Direction RandomDirection
    {
        get
        {
            return (Direction)Random.Range(0, numOfDirections);
        }
    }



    //method to convert directional vector to its coordinates
    public static Coordinates toCoordinates(this Direction d)
    {
        return directionalVectors[(int)d];
    }




}
