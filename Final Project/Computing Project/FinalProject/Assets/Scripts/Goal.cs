using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

    public Coordinates xz;

    void Update()
    {
        foundGoal();
   
    }

    //check if player is near goal
    public static bool foundGoal()
    {
        GameObject p = GameObject.Find("Player");
        GameObject g = GameObject.Find("Goal");
        if (Vector3.Distance(p.transform.position, g.transform.position) <= 2.8)
        {
            Destroy(g);
            Dungeon.complete = true;
            return true;
        }
        else
        {
            return false;
        }
        
    }




}

