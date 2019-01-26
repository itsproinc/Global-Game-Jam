using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{
    public Dialogue dialogue;
    public GameObject dialoguePanel;

    public GameObject homeDoor;
    public GameObject character;

    public int curHomeTier;

    public int homeTier;
    public GameObject upgradePanel;

    private void Reset ()
    {
        dialoguePanel = GameObject.FindGameObjectWithTag ("UI_Dialogue");
        dialogue = dialoguePanel.GetComponent<Dialogue> ();
        upgradePanel = GameObject.FindGameObjectWithTag ("UI_UpgradePanel");
    }

    private void Start ()
    {
        // Spawn player at door
        character.transform.position = homeDoor.transform.position;
    }

    public void AtHome ()
    {
        dialoguePanel.SetActive (true);
        dialogue._DialogueTrigger ("Welcome back home...", 0);
    }
}