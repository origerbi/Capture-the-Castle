using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WelcomeScene : MonoBehaviour
{
    // Fields
    public static int victoryFlag;

    public LayerMask enemyLayers;
    public Toggle attackRadioButton;
    public GameObject victoryMessage, defeatMessage;

    // Methods
    public void Start()
    {
        if (victoryFlag != 0)
        {
            victoryMessage.SetActive(victoryFlag == 1);
            defeatMessage.SetActive(victoryFlag == -1);
        }
        victoryFlag = 0;
    }

    public void OnPlayButtonClick()
    {
        // Passes arguments required for game scene
        Spawner.playerTeam = attackRadioButton.isOn ? Knight.ROLE.ATTACK : Knight.ROLE.DEFENSE;
        Spawner.enemyLayers = enemyLayers;

        SceneManager.LoadScene(1);
    }
}