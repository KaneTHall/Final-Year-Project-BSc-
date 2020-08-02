using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public interface Graph
{

    bool addNode(int a);          // true if node added
    bool addEdge(int a, int b, float cost);   // true if edge added

    List<int> nodes();
    float cost(int a, int b);
    List<int> neighbours(int a);

}
