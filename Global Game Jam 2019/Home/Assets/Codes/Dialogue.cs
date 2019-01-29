using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public GameObject dialoguePanel;

    public GameObject mouthSprite;

    public string textInput;
    public float textSpeed;

    public Text textOutput;

    public List<char> letters = new List<char> ();
    public int mouthGap;

    public void Reset ()
    {
        dialoguePanel = GameObject.FindGameObjectWithTag ("UI_Dialogue");
    }

    public void _DialogueTrigger (string input)
    {
        dialoguePanel.SetActive (true);
        StartCoroutine (DialogueTrigger (input));
    }

    private void Start ()
    {
        mouthSprite.SetActive (false);
    }

    IEnumerator DialogueTrigger (string input)
    {
        letters.Clear (); // Reset letters list
        textOutput.text = "";
        mouthSprite.SetActive (mouthSprite.activeSelf ? false : true); // Open mouth first

        textInput = input;
        foreach (char letter in input)
            letters.Add (letter);

        int letterCount = 0;
        while (textOutput.text != input)
        {
            if (letterCount >= mouthGap)
            {
                mouthSprite.SetActive (mouthSprite.activeSelf ? false : true);
                letterCount = 0;
            }

            textOutput.text = textOutput.text + letters [0];
            letters.RemoveAt (0);
            letterCount++;
            yield return new WaitForSeconds (textSpeed);
        }
    }

    public void SkipText ()
    {
        if (textOutput.text == textInput)
            dialoguePanel.SetActive (false);

        letters.Clear ();
        textOutput.text = textInput;
    }

    private void Update ()
    {
        if (letters.Count == 0)
            mouthSprite.SetActive (false);
    }
}