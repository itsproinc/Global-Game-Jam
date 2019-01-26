using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public GameObject upgradePanel;
    public GameObject dialoguePanel;

    public string textInput;
    public float textSpeed;

    public Image dialogueImage;
    public Sprite [] dialogueImages;

    public Text textOutput;

    public List<char> letters = new List<char> ();

    public void Reset ()
    {
        dialoguePanel = GameObject.FindGameObjectWithTag ("UI_Dialogue");
        upgradePanel = GameObject.Find ("UI_UpgradePanel");
    }

    public void _DialogueTrigger (string input, int dialogueCharacter)
    {
        StartCoroutine (DialogueTrigger (input, 0));
    }

    IEnumerator DialogueTrigger (string input, int dialogueCharacter)
    {
        textInput = input;
        letters.Clear (); // Reset letters list
        foreach (char letter in input)
        {
            letters.Add (letter);
        }

        while (textOutput.text != input)
        {
            textOutput.text = textOutput.text + letters [0];
            letters.RemoveAt (0);
            yield return new WaitForSeconds (textSpeed);
        }
    }

    public void SkipText ()
    {
        letters.Clear ();
        textOutput.text = textInput;

        upgradePanel.SetActive (true);
        dialoguePanel.SetActive (false);
    }
}