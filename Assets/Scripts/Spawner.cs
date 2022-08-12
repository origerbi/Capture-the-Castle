using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour // ALL WORKS DO NOT TOUCH!!!
{
    // Constants
    private Vector3 PLAYERATTACKSPAWN = new Vector3(-103f, 39.3f, 576f);
    private Vector3 PLAYERDEFENSESPAWN = new Vector3(-168.5f, 34.4f, 447.2f);
    private Vector3 ATTACKERSPAWN = new Vector3(-114.65f, 37.8f, 579.5f);
    private Vector3 DEFENDERSPAWN = new Vector3(-168.5f, 34.4f, 447.2f);

    // Fields
    public static Knight.ROLE playerTeam;
    public static LayerMask enemyLayers;

    public GameObject attackTeam, defenseTeam;
    public Player player;

    // Methods
    // Start is called before the first frame update
    public void Start()
    {
        player.team = playerTeam;
        player.transform.parent = playerTeam == Knight.ROLE.ATTACK ? attackTeam.transform : defenseTeam.transform;
        player.transform.position = playerTeam == Knight.ROLE.ATTACK ? PLAYERATTACKSPAWN : PLAYERDEFENSESPAWN;

        FamiliarizeTeams();
    }

    public void FamiliarizeTeams()
    {
        List<Knight> attackers = new List<Knight>(attackTeam.transform.GetComponentsInChildren<Knight>()),
            defenders = new List<Knight>(defenseTeam.transform.GetComponentsInChildren<Knight>()),
        allKnights = new List<Knight>();

        // Combines all knights on field into one list
        allKnights.AddRange(attackers);
        allKnights.AddRange(defenders);

        foreach (Knight knight in allKnights)
        {
            // Tells each knight who are allies and enemies
            knight.allies = knight.team == Knight.ROLE.ATTACK ? attackers : defenders;
            knight.enemies = knight.team == Knight.ROLE.ATTACK ? defenders : attackers;

            // Adds enemy knights to enemy's layer (for player's sword)
            if (knight.team != player.team)
                knight.gameObject.layer = LayerMask.NameToLayer("Enemies");
        }
    }
}