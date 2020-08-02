using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Player : MonoBehaviour {

    public Coordinates xz;
    public GameObject sight;
    public GameObject head;
    public float horizontalMovement = 2;
    public float verticalMovement = 2;
    //Enumerations  for States of Enemy
    enum State { Alive,Dead,PoweredUp}
    State currentState = State.Alive;
    static bool[,] visited = new bool[500, 500];
    public static List<int> playerPath = new List<int>();
    public static Coordinates current;
    public static bool Powered;
    Direction currentDirection;
    void Update()
    {

        //Move Up
        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            Coordinates Up = new Coordinates(0, 1);
            xz += Up;
           // print("X= "+ xz.x+ "Z= "+ xz.z);
           // player.transform.Translate(0, 0, Up.z);

            currentDirection = Direction.North;
        }
        //Move Right
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            Coordinates Right = new Coordinates(1, 0);
            xz += Right;
         //   print("X= " + xz.x + "Z= " + xz.z);
          //  player.transform.Translate(Right.x, 0, 0);
            currentDirection = Direction.East;
        }
        //Move Down
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            Coordinates Down = new Coordinates(0, -1);
            xz += Down;
        //    print("X= " + xz.x + "Z= " + xz.z);
           // player.transform.Translate(0, 0, Down.z);
            currentDirection = Direction.South;
        }
        //Move Left
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            Coordinates Left = new Coordinates(-1, 0);
            xz += Left;
          //  print("X= " + xz.x + "Z= " + xz.z);
           // player.transform.Translate(Left.x, 0, 0);
            currentDirection = Direction.West;
        }
        //print("Function Test: " + "X= " + Vector3ToCoordinates().x + "Z= " + Vector3ToCoordinates().z);
        // direction enumerators dictating what  direction the player moves in
        switch (currentDirection)
        {
            case Direction.North:
                this.gameObject.transform.Translate(0, 0, 0.1f);
                break;
            case Direction.East:
                this.gameObject.transform.Translate(0.1f, 0, 0);
                break;
            case Direction.South:
                this.gameObject.transform.Translate(0, 0, -0.1f);
                break;
            case Direction.West:
                this.gameObject.transform.Translate(-0.1f, 0, 0);
                break;
        }


        playerFSM();
        //print(Dungeon.Score);
       //Assign mouse speed 
        float h= horizontalMovement * Input.GetAxis("Mouse X");
        float v = verticalMovement * Input.GetAxis("Mouse Y");
        head = GameObject.Find("Player/Head");
        //Set the sight rotation to match mouse movement
        if (head != null)
        {
            head.transform.Rotate(0, Input.GetAxis("Mouse X") * 8, 0);
        }
  
        visitingNodes();
    }

    public void playerFSM() 
    {
        foundPowerUp();
        switch (currentState)
        {   //Dead state destroy the player object and reset the game back to level 1
            case State.Dead:
                Destroy(this.gameObject);
                Dungeon.complete = true;
                break;
            //Powered up state set powered to true with is used as a static variable in the Enemy.cs script
            case State.PoweredUp:
                Powered = true;
                //Invoke the setAliveState() method to trigger in x seconds which is used as a time limit for how long 
                Invoke("setAliveState", 10);
               // print("Power"); 
                break;
            //Alive state which sets powered to false which controls certain behaviours of the Enemy gameobjects
            case State.Alive:
                Powered = false;
                break;

        }


    }


    //Keeps a log of most up to date 
    public void visitingNodes()
    {
        playerPath.Insert(0,Dungeon.freeTiles.IndexOf(Dungeon.Gettile(Vector3ToCoordinates()).gameObject));
        if(playerPath.Count>30)
        {
            playerPath.RemoveAt(playerPath.Count - 1);
        }
        
    }
    //Convet vector3 vector position to Coordinates
    public Coordinates Vector3ToCoordinates()
    {
     
        Coordinates output = new Coordinates((int)this.gameObject.transform.position.x+11, (int)this.gameObject.transform.position.z+16);
        current = output;
        return output;
    }

    public void cleanWaypoints()
    {
        Dungeon.freeTiles.Clear();
    }


    public bool foundPowerUp() 
    {
        GameObject u;
        //If power-up has been destroyed (collected by player) 
        if (GameObject.Find("Power-Up") != null)
        {
            u = GameObject.Find("Power-Up");
            if (Vector3.Distance(this.gameObject.transform.position, u.transform.position) <= 2.8)
            {
                //change state of player to powered up and destroy the power-up gameobject
                currentState = State.PoweredUp;
                Destroy(u);
                return true;
            }
        }
        return false;

    }


    //set state to alive.
    public void setAliveState()
    {
        currentState = State.Alive;
    }

}

