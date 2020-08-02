using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Implementation from Games AI Coursework
public class RandomIA : PathAgent
{
    public override Pathfinder createPathfinder()
    {
        return new RandomPathfinder();
    }

}

public class RandomPathfinder : Pathfinder
{
    public override List<int> findPath(int start, int goal)
    {
        //path of nodes
        List<int> path = new List<int>();
        //add starting node
        path.Add(start);
        //initialise random object rnd
        System.Random rnd = new System.Random();
        while (path.Count < 2)
        {
            //assign next node to a random number from 0 to number of nodes in the search space
            int nextNode = rnd.Next(0, Dungeon.freeTiles.Count);
            //add next node
            path.Add(nextNode);
        }

    
        return path;
    }
}