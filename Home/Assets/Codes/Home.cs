using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Home : MonoBehaviour
{
    public Transform character;
    public AudioSource audioSource;

    public Dialogue dialogue;
    public GameObject dialoguePanel;

    public GameObject upgradePanel;

    public GameObject buttons;
    public Animator animator;
    public float speed;

    public Text buyButtonText;

    public bool animating;

    public void UpgradePanel ()
    {
        upgradePanel.SetActive (upgradePanel.activeSelf ? false : true);
        buyButtonText.text = upgradePanel.activeSelf ? "Back" : "Shop";
    }

    private void Reset ()
    {
        character = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> ();

        upgradePanel = GameObject.FindGameObjectWithTag ("UI_UpgradePanel");
        dialoguePanel = GameObject.FindGameObjectWithTag ("UI_Dialogue");
        dialogue = dialoguePanel.GetComponent<Dialogue> ();
        animator = GameObject.FindGameObjectWithTag ("Player").GetComponent<Animator> ();
        buttons = GameObject.FindGameObjectWithTag ("Buttons");
    }

    public void AtHome ()
    {
        dialoguePanel.SetActive (true);
        dialogue._DialogueTrigger ("Welcome back home...");
    }

    public void BackMainMenu ()
    {
        buttons.SetActive (false);
        upgradePanel.SetActive (false);

        // Move wife
        speed = 1;
        animating = true;
        animator.SetTrigger ("Move11");
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

            if (character.localPosition.x <= -410f)
                SceneManager.LoadScene (0);
        }
    }
}