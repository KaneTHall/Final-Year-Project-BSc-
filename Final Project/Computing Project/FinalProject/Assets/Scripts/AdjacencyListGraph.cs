using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AdjacencyListGraph : Graph {

    Dictionary<int, Dictionary<int, float>> d = new Dictionary<int, Dictionary<int, float>>();
    public AdjacencyListGraph()
    {

    }

    bool Graph.addNode(int a)
    {
        if (!d.ContainsKey(a))
        {
            Dictionary<int, float> neighCost = new Dictionary<int, float>();
            d.Add(a, neighCost);
            return true;
        }
        else
        {
            return false;
        }
    }

    bool Graph.addEdge(int a, int b, float cost)
    {
        if (d.ContainsKey(a) & d.ContainsKey(b))
        {

            d[a].Add(b, cost);
            return true;
        }
        else
        {
            return false;
        }
    }

    List<int> Graph.nodes()
    {

        List<int> nodes = new List<int>(this.d.Keys);

        return nodes;
    }

    List<int> Graph.neighbours(int a)
    {
        if (d.ContainsKey(a))
        {
            List<int> neighbours = new List<int>(d[a].Keys);
            return neighbours;
        }
        else
        {
            return null;
        }
    }


    float Graph.cost(int a, int b)
    {
        if (d.ContainsKey(a))
        {
            float cost = d[a][b];
            return cost;
        }
        else
        {
            return 0;
        }

    }
}
