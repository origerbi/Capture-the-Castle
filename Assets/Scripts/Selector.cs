using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    // Fields
    private string selectableTag = "Selectable";
    private NPC selection;
    private Material lastSelectionMaterial;

    public Player player;
    public GameObject commandMessage, commandsUI;
    public Material enemyMaterial, allymaterial;

    // Methods
    // Update is called once per frame
    public void Update()
    {
        NPC npc;
        SkinnedMeshRenderer renderer;
        RaycastHit hit;

        if (selection != null)
        {
            renderer = selection.transform.GetComponentInChildren<SkinnedMeshRenderer>();
            SetMaterial(lastSelectionMaterial);
            commandMessage.SetActive(false);

            selection = null;
            lastSelectionMaterial = null;
        }

        // Validates any transform is hit by raycast
        if (!Physics.Raycast(player.playerCamera.transform.position, player.playerCamera.transform.forward, out hit))
            return;

        // Validates a knight is the hit transform
        npc = hit.transform.GetComponent<NPC>();
        if (npc == null)
            return;

        if (npc.CompareTag(selectableTag))
        {
            // Validates the knight has a renderer
            selection = npc;
            renderer = selection.transform.GetComponentInChildren<SkinnedMeshRenderer>();
            if (renderer == null)
                return;

            if (selection.team == player.team)
            {
                SetMaterial(allymaterial); // Ally material
                // 
                commandMessage.SetActive(true);

                if (Input.anyKeyDown)
                    HandleKeyboardInput();
            }
            else
            {
                SetMaterial(enemyMaterial); // enemy material
                //
                commandsUI.SetActive(false);
                commandMessage.SetActive(false);
            }
        }
    }

    public void SetMaterial(Material material)
    {
        SkinnedMeshRenderer renderer = selection.transform.GetComponentInChildren<SkinnedMeshRenderer>();
        lastSelectionMaterial = renderer.material;
        renderer.material = material;
    }

    public void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl)) // Command ally
        {
            selection.Halt();
            commandsUI.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.C)) // charge
        {
            selection.Charge();
            commandsUI.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.H)) // halt
        {
            commandsUI.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.F)) // flee
        {
            selection.Flee();
            commandsUI.SetActive(false);
        }

        commandMessage.SetActive(false);
    }
}
