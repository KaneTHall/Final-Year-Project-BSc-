using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
    public Coordinates xz;
    public static int interval = -1;
    public static int changeDirection = 1;
    public static int roomSize = 1000;
    //return edge of cell
  

    public Direction nextDirection
    {
        get
        {
            //interval assigned to number of edges
                
       
            interval++;
            if(interval%(roomSize-1)==0)
            {
                changeDirection++;
            }
            if(changeDirection==4)
            {
                changeDirection = 0;
            }
            return (Direction)changeDirection; 
  
            throw new System.InvalidOperationException("Tiles has no uninitialized directions left.");
        }
    }

}
