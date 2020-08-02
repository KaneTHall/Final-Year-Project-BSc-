using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Implementation from Games AI Coursework
public abstract class Pathfinder
{
    public Graph navGraph;
    public abstract List<int> findPath(int a, int b);
}