using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

public class FleeIA : PathAgent {

   
        public override Pathfinder createPathfinder()
        {
            return new FleePathfinder();
        }

    

    public class FleePathfinder : Pathfinder
    {
        public override List<int> findPath(int start, int goal)
        {
            //path of nodes
            List<int> path = new List<int>();
            int [] corner = new int[4];
            corner[0] = 0;
            corner[1] = 49;
            corner[2] = Dungeon.RoomArea - 1;
            corner[3] = corner[2] - 50;
            start = Random.Range(0,corner.Length);
            //add starting node
            path.Add(start);

            //initialise random object rnd
            System.Random rnd = new System.Random();
            while (path.Count < 25)
            {
                //assign next node to a random number from 0 to number of nodes in the search space
                int nextNode = rnd.Next(0, Dungeon.freeTiles.Count);
                if (!Player.playerPath.Contains(nextNode))
                {
                    //add next node
                    path.Add(nextNode);
                }
            }


            return path;
        }
    }
}
