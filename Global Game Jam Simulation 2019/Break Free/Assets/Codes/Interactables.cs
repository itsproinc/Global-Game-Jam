using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactables : MonoBehaviour
{
    public Player player;

    private void Start ()
    {
        player = GameObject.Find ("Player").GetComponent<Player> ();
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        // Ladder
        if (gameObject.tag == "Ladder")
        {
            if (other.tag == "Player")
                player.onLadder = true;
        }

        // Hiding door
        if (gameObject.tag == "Hiding Door")
        {
            if (other.tag == "Player")
                player.onHidingDoor = true;
        }

        // Cell
        if (gameObject.tag == "Cell")
        {
            if (other.tag == "Player")
                player.onCellDoor = true;
        }
    }

    private void OnTriggerExit2D (Collider2D other)
    {
        player.toolTip.text = "";
        player.interactButton.interactable = false;

        // Ladder
        if (gameObject.tag == "Ladder")
        {
            if (other.tag == "Player")
                player.onLadder = false;
        }

        // Hiding door
        if (gameObject.tag == "Hiding Door")
        {
            if (other.tag == "Player")
                player.onHidingDoor = false;
        }

        // Cell
        if (gameObject.tag == "Cell")
        {
            if (other.tag == "Player")
                player.onCellDoor = false;
        }
    }
}