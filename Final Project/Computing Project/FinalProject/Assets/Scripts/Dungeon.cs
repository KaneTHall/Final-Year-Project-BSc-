using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Dungeon : MonoBehaviour
{

    //a variable to create a delay and will be used for generating the grid.
    public float stepDelay;
    public Tile tilePrefab;
    private Tile[,] tiles;
    public Coordinates Vector;
    public static Dictionary<Coordinates,Tile> FullDung = new Dictionary<Coordinates,Tile>();
    public int NumberOfRooms;
    public static Coordinates CD = new Coordinates(0, 0);
    public static int tileCount = 0;
    public static int RoomNo=0;
    public static int RoomArea;
    //reference wall instance
    public Wall wPrefab;
    public Wall wPrefab2;
    public Wall wPrefab3;
    public Wall wPrefab4;
    private List<Tile> allTiles = new List<Tile>();
    private List<Tile> newList = new List<Tile>();
    public static int interval=0;
    public Player playerPrefab;
    public Enemy enemy1Prefab;
    public Enemy enemy2Prefab;
    public Enemy enemy3Prefab;
    public Enemy enemy4Prefab;
    public static Coordinates playerSpawn = new Coordinates(2, 2);
    public static List<GameObject> freeTiles = new List<GameObject>();
    public static List<Wall> wallLit = new List<Wall>();
    public static Wall[,] trees;
    public Goal goalPrefab;
    public static int Level = 1;
    public static bool complete = false;
    public static int numberOfEnemies;
    private List<Coordinates> plotMap = new List<Coordinates>();
    public static float threshold;
    public static float frequency;
    public PowerUp powerPrefab;
    public static bool VR;
    public static int Score=0;
    public static int Multiplier=0;
    public int spawnG=0;

    public IEnumerator Generate()
    {
        //instantitate a generation delay which is a delay for each tile generated.
        WaitForSeconds generationDelay = new WaitForSeconds(stepDelay);
        //call cleangame function to clear all gameobjects
        cleanGame();
        //Room area = area of the search space
        RoomArea = Vector.x * Vector.z ;
        //plot the map with the coordinates for each tile.
        for(int i=CD.x;i<Vector.x+CD.x;i++)
        {
            for(int j=CD.z;j<Vector.z+CD.z;j++)
            {
                Coordinates newXZ = new Coordinates(i, j);
                plotMap.Add(newXZ);
            }
        }
        //add new tile to the frontier 
        freeTiles.Add(CreateTile(CD).gameObject);
        FullDung.Remove(new Coordinates(CD.x, CD.z));
        //while coordinates still exist in the plotMap list call GenerateLevel function to render tiles per interval
        while (plotMap.Count>0)
        {
            yield return generationDelay;
            GenerateLevel(freeTiles);
            //when all tiles are generated
            if (freeTiles.Count-1 == RoomArea)
            {
                   //If level generated is too populated then return to Menu
                   if(wallLit.Count<15 || wallLit.Count>(freeTiles.Count/2))
                    {
                       SceneManager.LoadScene("Menu");
                    }
                   //Spawn Player in
                    Coordinates playerSpawn = new Coordinates(CD.x+5, CD.z+5);
                    SpawnPlayerIn(playerSpawn);
                    //If VR is False Destroy VR Object
                    if (VR == false)
                    {
                        Destroy(GameObject.Find("Player/VRSimulatorCameraRig"));
                    }
                    //Else Destroy the players head object consisting of the player arms, head and camera
                    else
                    {
                        Destroy(GameObject.Find("Player/Head"));
                    }
                    //Spawn enemyNo number of enemies at random locations in the level
                    for (int enemyNo = 1; enemyNo <= numberOfEnemies; enemyNo++)
                    {
                        Coordinates enemySpawn = new Coordinates(Random.Range(CD.x, Vector.x), Random.Range(CD.z, Vector.z));
                        SpawnEnemyIn(enemySpawn, enemyNo);
                    }
                    //Spawn a goal to return to
                    Coordinates goalSpawn = new Coordinates((Vector.x + CD.x) - playerSpawn.x, (Vector.z + CD.z) - playerSpawn.z);
                    SpawnGoalIn(goalSpawn);
                    break;
                
            }
        }
    }


    private void Update()
    {
        //spawn a new power-up object if Power-up object has been collected / cannot be found.
        if(GameObject.Find("Power-Up")==null)
        {
            Coordinates PowerSpawn = new Coordinates(Random.Range(0, Vector.x), Random.Range(0, Vector.z));
            SpawnPowerUps(PowerSpawn);
        }
    }



    private void GenerateLevel(List<GameObject> tiles)
    {
            //select tile at end of list
            int Index = tiles.Count - 1;
            //current tile is assigned to tile at end of list
            GameObject current = tiles[Index];

        //Create a Column of tiles
        for (int i = 0; i < Vector.z; i++)
        {
            //if plotMap contains coordinates
            if (plotMap.Count != 0)
            {
                //assign c to coordinates at end of plot map list
                Coordinates c = plotMap[plotMap.Count - 1];
                //remove last element in plot map list
                plotMap.RemoveAt(plotMap.Count - 1);

                //if coordinates are within the search space (Vector.x and Vector.z max height and width of the search space)
                if (coordinatesCheck(c))
                {
                        //assign next tile to a new game obejct and create tile at current coordinates 
                        GameObject nextTile = CreateTile(c).gameObject;
                        //add nextTile to list of gameobject tiles to be used as Waypoint later.
                        tiles.Add(nextTile);
                }
            }
        }
    }

    //Function to render tile at coordinates  of input c
    private Tile CreateTile(Coordinates c)
    {
        Tile t = Instantiate(tilePrefab) as Tile;
        t.xz = c;
       //assign a name to the tile
        t.name = "Tile:[" + c.x + "," + c.z + "]";
        t.transform.parent = transform;
        //set the new tile at the corresponding position
        Vector3 point = new Vector3(c.x - Vector.x * 0.5f + 0.5f, 0f, c.z - Vector.z * 0.5f + 0.5f);
        //render prefab at the location of the point Vecor
        t.transform.localPosition = point;
        //Count tiles
        tileCount++;
        //Add cell coordinates and tile to dictionary
        FullDung.Add(c, t);
        //Add current tile 
        //freeTiles.Add(t.gameObject);
        //Call The Noise function
        Nfunction method = Noise.noiseDimensions[1];
        //The noise method takes the declared point and frequency as an input then returns a value between -1 and 1 if that value is greater than the user
        //determined threshold then build a wall
        if (method(point, (frequency)) > threshold)
          {
            //build wall at location c
              buildWall(c);
          }
        //return as Tile component
        return t;
    }

    //Destory tile object at coordinates c
    private void DestroyTile(Coordinates c)
    {
        //Destroy tile gameobject
        Destroy(Gettile(c).gameObject);
        //add the tile to all tiles list
        allTiles.Add(Gettile(c));
        //remove it from available tiles.
        freeTiles.Remove(Gettile(c).gameObject);
        
    }
    //function to spawn the player in at input coordinates c 
    private Player SpawnPlayerIn(Coordinates c)
    {
        //instantiate the player model prefab
        Player player = Instantiate(playerPrefab) as Player;
        //assign player coordinates to input coordinates
        player.xz = c;
        //Assign the name of the player game object
        player.name = "Player";
        player.transform.parent = transform;
        //set the vector 3 position of the player.
        player.transform.localPosition = new Vector3(c.x - Vector.x * 0.5f + 0.5f, 10f, c.z - Vector.z * 0.5f + 0.5f);
        return player;
    }

    //SpawnENemy function this function will create an IA at coordinates c
    private Enemy SpawnEnemyIn(Coordinates c,int enemyNo)
    {
        //generate number to determine which type of IA to spawn
        int spawnEnemy = Random.Range(0, 2);
        Enemy enemy=null;
        if (spawnEnemy == 0)
        {
             enemy = Instantiate(enemy1Prefab) as Enemy;
        }
        if (spawnEnemy == 1)
        {
             enemy = Instantiate(enemy2Prefab) as Enemy;
        }
        //assign agent coordinates to the input cooordinates
        enemy.xz = c;
        //Assign name to agent
        enemy.name = "Enemy"+enemyNo;
        enemy.transform.parent = transform;
        //set the location of the IA to spawn
        enemy.transform.localPosition = new Vector3(c.x - Vector.x * 0.5f + 0.5f, 1f, c.z - Vector.z * 0.5f + 0.5f);
        return enemy;
    }
    //Spawn goal in at input coordinates c
    private Goal SpawnGoalIn(Coordinates c)
    {
        Goal goal = Instantiate(goalPrefab) as Goal;
        //increment level +1 each time a new goal is spawned.
        Level++;
        goal.xz = c;
        //Assign name to goal
        goal.name = "Goal";
        goal.transform.parent = transform;
        //set the new goal at the corresponding position
        goal.transform.localPosition = new Vector3(c.x - Vector.x * 0.5f + 0.5f, 1f, c.z - Vector.z * 0.5f + 0.5f);
        return goal;
    }

    //function that spawns a power-up at coordinates c
    private PowerUp SpawnPowerUps(Coordinates c)
    {
        PowerUp pow = Instantiate(powerPrefab) as PowerUp;
        pow.xz = c;
        //assign name to power-up
        pow.name = "Power-Up";
        pow.transform.parent = transform;
        //set the vector 3 position of the power-up
        pow.transform.localPosition = new Vector3(c.x - Vector.x * 0.5f + 0.5f, 1f, c.z - Vector.z * 0.5f + 0.5f);
        return pow;
    }

    //Construct new obstacle
    private Wall buildWall(Coordinates c)
    {
        //Randomly generate a number to determine which prefab to render to have variety in what obstacles are spawned
        int randomObstacleNo = Random.Range(0, 4);
        Wall w = null;
        if (randomObstacleNo == 0)
        {
            w = Instantiate(wPrefab) as Wall;
        }
        if (randomObstacleNo == 1)
        {
            w = Instantiate(wPrefab2) as Wall;
        }
        if (randomObstacleNo == 2)
        {
            w = Instantiate(wPrefab3) as Wall;
        }
        if (randomObstacleNo == 3)
        {
            w = Instantiate(wPrefab4) as Wall;
        }

        w.xz = c;
        //assign name of obstacle and the coordinates location
        w.name = "Obstacle:[" + c.x + "," + c.z + "]";
        w.transform.parent = transform;
        
        //set obstacle position
        w.transform.localPosition = new Vector3(c.x - Vector.x * 0.5f + 0.5f, 0f, c.z - Vector.z * 0.5f + 0.5f);
        //some objects are not perfecly alligned with tiles so adjust certain prefabs when rendered to centere them in the middle of a tile.
        if(randomObstacleNo==0)
        {
            w.transform.Translate(1f, 0, 0.75f);
        }

        if(randomObstacleNo==1)
        {
            w.transform.Translate(0.7f, 0, 0.8f);
        }
        // remove the tile that occupies the cell the obstacle was placed on from the free tiles list.
        freeTiles.Remove(Gettile(c).gameObject);
        wallLit.Add(w);
        //return as wall component
        return w;
    }

    //method to check if coordinates are in the generated search space;
    public bool coordinatesCheck(Coordinates c)
    {
        return c.x >= CD.x && c.x < Vector.x+CD.x && c.z >= CD.z && c.z < Vector.z+CD.z;
    }

    //return the tile located at coordinates c
    public static Tile Gettile(Coordinates c)
    {
        if (FullDung.ContainsKey(c))
        {
            return FullDung[c];
        }
        else
        {
            return null;
        }
    }

    //return obstacle at coordinates location c
    public static Wall GetWall(Coordinates c)
    {
        return trees[c.x, c.z];
    }


    //function to clean game and wipe all gameobjects when a new level needs creating or player dies.
    public void cleanGame()
    {
        freeTiles.Clear();
        allTiles.Clear();
        newList.Clear();
        FullDung.Clear();
        wallLit.Clear();
        GameObject e1 = GameObject.Find("Enemy1");
        GameObject e2 = GameObject.Find("Enemy2");
        GameObject e3 = GameObject.Find("Enemy3");
        GameObject e4 = GameObject.Find("Enemy4");
        GameObject p = GameObject.Find("Player");
        GameObject g = GameObject.Find("Goal");
        Destroy(e1);
        Destroy(p);
        Destroy(g);
        if(e2!=null)
        {
            Destroy(e2);
        }
        if (e3 != null)
        {
            Destroy(e3);
        }
        if (e4 != null)
        {
            Destroy(e4);
        }
        Tile.interval = -1;
        Tile.changeDirection = 1;
        Tile.roomSize = 1000;

    }


}
