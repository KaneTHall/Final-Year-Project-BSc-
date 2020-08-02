using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
//Implementation from Games AI Coursework
public abstract class PathAgent : MonoBehaviour
{

    // Set from inspector
    public GameObject waypointSet;
    public GameObject IA;
    // Waypoints
    public static WaypointGraph waypoints;
    protected int? current;

    protected List<int> path;
    protected Pathfinder pathfinder;

    public static float speed;
    protected static float NEARBY = 0.2f;
    protected static System.Random rnd = new System.Random();
    public abstract Pathfinder createPathfinder();





    void Start()
    {
        IA = GameObject.Find("Enemy1");
        waypoints = new WaypointGraph(waypointSet);
        waypointSet = GameObject.Find("Dungeon(Clone)");
        path = new List<int>();
        pathfinder = createPathfinder();
        pathfinder.navGraph = waypoints.navGraph;
    }

    void Update()
    {

        if (path.Count == 0)
        {
            // We don't know where to go next
            generateNewPath();

        }
        else
        {
            // Get the next waypoint position
            GameObject next = waypoints[path[0]];
            Vector3 there = next.transform.position;
            Vector3 here = transform.position;

            // Are we there yet?
            float distance = Vector3.Distance(here, there);
            if (distance < NEARBY)
            {
                // We're here
                current = path[0];
                path.RemoveAt(0);
              //  Debug.Log("Arrived at waypoint " + current);
            }
        }
    }

   
    //state that adds a new node to path
    IEnumerator newNode()
    {
        Debug.Log("check");
        Random r = new Random();
        int newr = Random.Range(0, 5);
        path.Add(newr);
        yield return 0;
    }



    void FixedUpdate()
    {
        if (path.Count > 0)
        {
            GameObject next = waypoints[path[0]];
            Vector3 position = next.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, position, speed);
        }
    }

    protected void generateNewPath()
    {

        if (current != null)
        {
            // We know where are
            List<int> nodes = waypoints.navGraph.nodes();

            if (nodes.Count > 0)
            {
                // Pick a random node to aim for
                int target = nodes[playerTarget()];
              //  Debug.Log("New target: " + target);
                // Find a path from here to there
                path = pathfinder.findPath(current.Value, target);
                // Debug.Log( playerTarget());
                // Debug.Log();

            }
            else
            {
                // There are zero nodes
               Debug.Log("No waypoints - can't select new target");
            }

        }
        else
        {
            // We don't know where we are

            // Find the nearest waypoint
            int? target = waypoints.returnNearest(transform.position);

            if (target != null)
            {
                // Go to target

                path.Clear();
                path.Add(target.Value);
              //  Debug.Log("Heading for nearest waypoint: " + target);
            }
            else
            {
                ///Couldn't find a waypoint
             //  Debug.Log("Can't find nearby waypoint to target" + "target is: " + target);
            }

        }
    }

    public static string writePath(List<int> path)
    {
        var s = new StringBuilder();
        bool first = true;
        foreach (int t in path)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                s.Append(", ");
            }
            s.Append(t);
        }
        return s.ToString();
    }

    //assign target to the current node the player is occupying
    public static int playerTarget()
    {
        int Target = Dungeon.freeTiles.IndexOf(Dungeon.Gettile(Player.current).gameObject);

        return Target;
    }


    public static int playerStart()
    {
        int Start = Dungeon.freeTiles.IndexOf(Dungeon.Gettile(Enemy.current).gameObject);

        return Start;
    }
}