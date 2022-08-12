using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Fields
    public static int enemiesCount;
    public Spawner spawner;
    public Player player;

    // Methods
    public void Start() // Start is called before the first frame update
    {
        enemiesCount = 5; // No matter the team a player spawns in
    }

    public void Update() // Update is called once per frame
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);

        if ((player.isDead) || (enemiesCount <= 0))
            GameOver();
    }

    public void GameOver()
    {
        WelcomeScene.victoryFlag = enemiesCount <= 0 ? 1 : -1;

        SceneManager.LoadScene(0);
    }
}