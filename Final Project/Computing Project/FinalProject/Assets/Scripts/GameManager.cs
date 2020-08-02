using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public Dungeon DPrefab;
    //private to hold the instance (we do this to instantiate a maze in begin game and destroy it in restart game before we begin a new game)
    private Dungeon DInstance;

    // Use this for initialization
    void Start () {
        //Begingame method called at the start.
        print("Test");
        BeginGame();
    }
	
	// Update is called once per frame
	void Update () {
        //If space key is pressed restart game/ regenerate maze
        if (Input.GetKeyDown(KeyCode.Space) || Dungeon.complete==true)
        {
            Dungeon.complete = false;
            SceneManager.LoadScene("Menu");
            RestartGame();
        }



    }

    private void BeginGame()
    {
        //Instantiate maze when we begin game
        DInstance = Instantiate(DPrefab) as Dungeon;
        StartCoroutine(DInstance.Generate());
    }

    private void RestartGame()
    {
        StopAllCoroutines();
        //destroy maze and begin  game again.
        Destroy(DInstance.gameObject);
        BeginGame();
    }


}
