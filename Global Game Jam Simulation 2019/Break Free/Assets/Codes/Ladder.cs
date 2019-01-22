using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public CubeMovement cubeMovement;

    private void Reset ()
    {
        cubeMovement = GameObject.Find ("Movement Joystick Area").GetComponent<CubeMovement> ();
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Player")
        {

        }
    }
}