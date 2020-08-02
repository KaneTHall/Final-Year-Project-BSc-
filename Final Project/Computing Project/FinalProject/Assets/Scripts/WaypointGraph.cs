using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Implementation from Games AI Coursework
public class WaypointGraph
{

    public Graph navGraph;
    public static List<GameObject> waypoints;
    public static int[,] arrayPoints= new int[Dungeon.freeTiles.Count,Dungeon.freeTiles.Count];
    public static Dictionary<GameObject, int> TileNode = new Dictionary<GameObject, int>();
    public GameObject this[int i]
    {
        get { return waypoints[i]; }
        set { waypoints[i] = value; }
    }

    public WaypointGraph(GameObject waypointSet)
    {
        //assign waypoints to new list
        waypoints = new List<GameObject>();
        //assign navGraph to new AdjacencyL
        navGraph = new AdjacencyListGraph();

        findWaypoints(waypointSet);
        buildGraph();
    }

    private void findWaypoints(GameObject waypointSett)
    {
        GameObject player = GameObject.Find("Player");
       // waypoints.Add(player);
       //use Dungeon.freeTiles a list of all game object tiles and add them as waypoints to the waypoint list
        if (Dungeon.freeTiles.Count != null)
        {
            for(int i=0;i < Dungeon.freeTiles.Count;i++)
            {
               
                waypoints.Add(Dungeon.freeTiles[i]);
                

            }
            Debug.Log(Dungeon.freeTiles.Count+Dungeon.wallLit.Count);
            // Debug.Log("Found " + waypoints.Count + " waypoints.");
            
        }
        else
        {
            //Debug.Log("No waypoints found.");

        }
    }

    private void buildGraph()
    {

        int n = waypoints.Count;

        navGraph = new AdjacencyListGraph();
        //add all nodes to to the navgraph
        for (int i = 0; i < n; i++)
        {
            navGraph.addNode(i);

        }

        
        for (int i = 0; i < n; i++)
        {
            for (int j = 1; j < n; j++)
            {
                //calculate distance cost between nodes
                Vector3 wPos1 = waypoints[i].transform.position;
                Vector3 wPos2 = waypoints[j].transform.position;
                float cost = Vector3.Distance(wPos1, wPos2);
                if (cost <= 1.5)
                {
                    navGraph.addEdge(i, j, cost);
                    //Debug.Log(cost);
                }
          
               // waypoints.Find(arrayPoints[i, j]);
          
            }
        }
    }

    //find nearest node from current vector position
    public int? returnNearest(Vector3 here)
    {
        int Start = Dungeon.freeTiles.IndexOf(Dungeon.Gettile(Enemy.current).gameObject);
        Debug.Log("Node:" + Start);
        return Start;

    }


}