using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    public Coordinates xz;
    public GameObject p;
    public float speed;
    public static Coordinates current;
    //Enumerations  for States of Enemy
    enum State { Roam, Dead, Flee, Chase }
    RandomIA randomPath;
    AstarIA astarPath;
    FleeIA fleePath;
    State currentState = State.Roam;
    Color[] colAr = new Color[4];
    int colorIndex;

    private void Awake()
    {
        //Assign colours to be used by the material of gameobjects
        colAr[0] = Color.blue;
        colAr[1] = Color.red;
        colAr[2] = Color.yellow;
        colAr[3] = Color.magenta;
        colorIndex = Random.Range(0, 4);
        randomPath = this.gameObject.GetComponent<RandomIA>();
        astarPath = this.gameObject.GetComponent<AstarIA>();
        fleePath = this.gameObject.GetComponent<FleeIA>();
        PathAgent.speed = speed;
    }


    void Update()
    {
        FSM();
    }

    public void FSM()
    {
        //assign player gameobject.
        p = GameObject.Find("Player");
        





        switch (currentState)
        {
            // Dead state will destroy the gameobject this script (Enemy.cs) is attatched to.
            case State.Dead:
                Dungeon.Multiplier++;
                Dungeon.Score += 10*Dungeon.Multiplier;
                Destroy(this.gameObject);
                break;
            // Flee state will calculate a path to get away from the player.
            case State.Flee:
                //Assign all fleeing Enemys gameobjects's material to the white colour to represent the change in state. 
                this.gameObject.GetComponentInChildren<Renderer>().material.color = Color.white;
                //If the gameobject is fleeing and is caught by the player then the state will change to dead.
                if (Vector3.Distance(p.transform.position, this.gameObject.transform.position) <= 2.8)
                {
                    currentState = State.Dead;

                }
                //If player is not powered up anymore change state to Roam.
                if (Player.Powered == false)
                {
                    currentState = State.Roam;
                }
                //If the game obejct was previous using the RandomIA script then remove it because Enemy is no longer chasing player.
                if (randomPath.enabled == true)
                {
                    randomPath.enabled = false;
                }
                //If the game obejct was previous using the AstarIA script then remove it because Enemy is no longer chasing player.
                if (astarPath.enabled == true)
                {
                    astarPath.enabled = false;
                }
                if (fleePath.enabled==false)
                {
                    fleePath.enabled = true;
                }
                break;
            //Roam state will calculate random paths around the level until it sees the player (comes within distance of the player).
            case State.Roam:
                //Assign an appropriate colour to the game object this script is attatched to (Enemy.cs). 
                if (this.gameObject.GetComponentInChildren<Renderer>().material.color != colAr[colorIndex])
                {
                    this.gameObject.GetComponentInChildren<Renderer>().material.color = colAr[colorIndex];
                }
                //If the game obejct was previous using the AstarIA script then remove it because Enemy is no longer chasing player.
                if (astarPath.enabled == true)
                {
                    astarPath.enabled = false;
                }
                //If the game object is not currently using the RandomIA script as a random path finding method then apply it as a component to this gameobject.
                if (randomPath.enabled==false)
                {
                    randomPath.enabled = true;
                }
                //If gameobject sees the player then change state to chase.
                if (Vector3.Distance(p.transform.position, this.gameObject.transform.position) <= 15)
                {
                    currentState = State.Chase;
                }
                //If the player is touching the enemy then destroy the player.
                if (Vector3.Distance(p.transform.position, this.gameObject.transform.position) <= 2.8)
                {
                    Dungeon.Score = 0;
                    Dungeon.Multiplier = 0;
                    print("Final Score" + Dungeon.Score);
                    Destroy(p);
                    Dungeon.complete = true;
                }
                if (fleePath.enabled==true)
                {
                    fleePath.enabled = false;
                }
                //If the player is powered up then change state to Flee.
                if (Player.Powered == true)
                {
                    currentState = State.Flee;
                }

                break;
            case State.Chase:
              
                //If the game obejct was previous using the RandomIA script then remove it because Enemy is no longer chasing player.
                if (randomPath.enabled==true)
                {
                    randomPath.enabled = false;
                }
                //If the game object does not have the component of the AstarIA script then apply it to calculate path to the player.
                if (astarPath.enabled==false)
                {
                    astarPath.enabled =true;
                }
                //Destroy player if the Enemy (this.gameobject) catches player.
                if (Vector3.Distance(p.transform.position, this.gameObject.transform.position) <= 2.8)
                {
                    Dungeon.Score = 0;
                    Dungeon.Multiplier = 0;
                    Destroy(p);
                    Dungeon.complete = true;
                }

                //change state back to roam if the player is no longer in sight.
                if (Vector3.Distance(p.transform.position, this.gameObject.transform.position) >= 25)
                {
                    currentState = State.Roam;
                }
                //change state to flee if player is powered-up.
                if (Player.Powered == true)
                {
                    currentState = State.Flee;
                }
                if (fleePath.enabled == true)
                {
                    fleePath.enabled = false;
                }
                break;

        }
        Vector3ToCoordinates();

    }
  


    //Convet vector3 vector position to Coordinates
    public Coordinates Vector3ToCoordinates()
    {
        Coordinates output = new Coordinates((int)this.gameObject.transform.position.x + 11, (int)this.gameObject.transform.position.z + 16);
        current = output;
        return output;
    }

}
