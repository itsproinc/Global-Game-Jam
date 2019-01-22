using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour
{
    GridSystem gridSystem;

    public int timer2;
    public float timer1;
    public Text tulisan;

    public bool gameStarted;
    public bool reset;

    private void Start ()
    {
        gridSystem = GetComponent<GridSystem> ();
    }

    void Update ()
    {
        if (gameStarted)
        {
            tulisan.text = timer2.ToString ("00");
            if (timer1 > 0)
            {
                timer1 -= Time.deltaTime;
            }

            if (timer1 <= 0)
            {
                if (!reset)
                {
                    reset = true;
                    TimesUp ();
                }
            }
            timer2 = Mathf.RoundToInt (timer1);
        }
    }

    private void TimesUp ()
    {
        gridSystem.TimesUp ();
    }
}