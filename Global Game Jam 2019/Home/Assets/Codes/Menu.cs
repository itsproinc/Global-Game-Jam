using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public AudioSource audioSource;

    public GameObject menu;
    public Transform character;
    public Animator animator;

    public float speed;

    /*  [0] = Jam dinding
        [1] = Kalendar
        [2] = Tanaman kiri
        [3] = Tanaman tengah
        [4] = Tanaman kanan
        [5] = Lampu
        [6] = Lukisan tembok
        [7] = Tong sampah
     */

    public GameObject [] property = new GameObject [8];
    public bool animating;

    private void Start ()
    {
        animator = character.GetComponent<Animator> ();

        for (int i = 0; i < property.Length; i++)
        {
            bool propertyBool = (PlayerPrefs.GetInt ("OwnedObject" + i) == 1) ? true : false;

            if (propertyBool == true)
            {
                property [i].SetActive (true);
            }
        }
    }

    private void Update ()
    {
        animator.SetFloat ("speed", speed);
        if (animating)
        {
            if (speed > 0.3f)
            {
                speed -= Time.fixedDeltaTime * 0.6f;
                audioSource.volume = speed;
            }

            if (character.localPosition.x >= 575)
                SceneManager.LoadScene (1);
        }
    }

    public void PlayGame ()
    {
        menu.SetActive (false);

        // Move wife
        speed = 1;
        animating = true;
        animator.SetTrigger ("Move01");
    }
}