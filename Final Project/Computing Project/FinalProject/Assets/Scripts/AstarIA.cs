using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Priority_Queue;
using System.Linq;
using System.Collections;
using System;


public class AstarIA : PathAgent
{
    public static float heuristicCost(int a, int b)
    {
        //set gameobjects assigned to waypoints in the waypoints array.
        GameObject wayPoint1 = waypoints[a];
        GameObject wayPoint2 = waypoints[b];
        //create vector3 for two positions of the 2 waypoints
        Vector3 pos1 = wayPoint1.transform.position;
        Vector3 pos2 = wayPoint2.transform.position;
        // calculate the distance to be used as a heuristic
        float dx = (float)Math.Abs(pos1.x - pos2.x);
        float dy = (float)Math.Abs(pos1.y - pos2.y);
        return (float)Math.Sqrt(dx * dx + dy * dy);
    }

    public override Pathfinder createPathfinder()
    {
        //create a heuristic that will be calculated from two positions
        Heuristic cost = new Heuristic(heuristicCost);
        //return new AstarPathfinder with the heuristic 'cost'
        return new AStarPathfinder(cost);
    }


}

public delegate float Heuristic(int a, int b);

public class AStarPathfinder : Pathfinder
{
    protected Heuristic guessCost;

    public AStarPathfinder(Heuristic h)
    {
        guessCost = h;
    }

    public override List<int> findPath(int start, int goal)
    {
        //create Simple priority queue and Dictionary
        SimplePriorityQueue<int> frontier = new SimplePriorityQueue<int>();
        Dictionary<int, int> visitedFrom = new Dictionary<int, int>();
        Dictionary<float, float> costSoFar = new Dictionary<float, float>();
        //Create a stack to pop nodes back in to the list.
        Stack<int> nextDestination = new Stack<int>();
        List<int> l1 = new List<int>();
        List<int> l2 = new List<int>();
        List<int> path = new List<int>();
        //add starting node with cost of 0 to frontier
        frontier.Enqueue(start, 0);
        Debug.Log(start);
        //set visited from [start] to -1
        visitedFrom[start] = -1;
        //cost so far = 0;
        costSoFar[start] = 0;
        
        while (frontier.Count > 0)
        {
            //get the node at the head of the queue / lowest priority
            int current = frontier.Dequeue();
            //if head of queue = goal then simplePriority queue complete. 
            if (current == goal)
            {
                break;
            }
            //create list neighbours of current node
            List<int> neighbours = navGraph.neighbours(current);
            foreach (int next in neighbours)
            {
                //calculate the cost accumalted so far.
                float nextCost = costSoFar[current] + navGraph.cost(current, next);
                //If no neighbours cost have been compared or check if new lower cost path has been found
                if (!costSoFar.ContainsKey(next) || nextCost < costSoFar[next])
                {
                    //   Debug.Log("Next " + next + " Current: " + current);
                    //add next node to priority queue with a cost of next cost + the heuristic guess cost
                    frontier.Enqueue(next, nextCost + guessCost(next, goal));
                    visitedFrom[next] = current;
                    //cost so far is added to the dictionary
                    costSoFar[next] = nextCost;
                    l1.Add(next);
                    l2.Add(visitedFrom[next]);
                }
            }
        }
        //push target goal to stack
        nextDestination.Push(goal);
        for (int destination = goal; destination != start; destination = visitedFrom[destination])

        {
            //keep pushing all the nodes visited in a dictionary
            nextDestination.Push(visitedFrom[destination]);
        }
        for (int i = nextDestination.Count; i > 0; i--)
        {
            //while stack is greater than 0 keep popping stack
          
            path.Add(nextDestination.Pop());
        }
        //return the path
        return path;
    }

}


