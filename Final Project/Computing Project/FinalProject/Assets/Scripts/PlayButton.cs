using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
//script for play button
public class PlayButton : MonoBehaviour {

    public Button btnPlay;
    public Text freq;
    public Text threshold;
    public Toggle VR;
    public Text score;
    public Text noEnemies;
    void Start()
    {
        Button btn = btnPlay.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        //update score
        score.text += Dungeon.Score;
    }

    //on play button click assign field values to static variables
    void TaskOnClick()
    {
        Dungeon.frequency = float.Parse(freq.text);
        Dungeon.threshold = float.Parse(threshold.text);
        Dungeon.VR = VR.isOn;
        Dungeon.numberOfEnemies = int.Parse(noEnemies.text);
        SceneManager.LoadScene("FinalProject");
    }


}
