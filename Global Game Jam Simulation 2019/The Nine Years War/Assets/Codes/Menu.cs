using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Text playTimeText;
    public Text winText;
    public Text winRateText;
    public Text matchText;

    public InputField username;

    void start ()
    {
        Time.timeScale = 1;
    }
    public void loadGame (string gantiScene)
    {
        SceneManager.LoadScene (gantiScene);
    }

    public void berhenti ()
    {
        Time.timeScale = 0;
    }

    public void mulai_lagi ()
    {
        Time.timeScale = 1;
    }

    public void LoadProfile ()
    {
        username.text = PlayerPrefs.GetString ("username");

        int secondPlayTime = PlayerPrefs.GetInt ("s_playTime");
        int minutePlayTime = PlayerPrefs.GetInt ("m_playTime");

        winText.text = PlayerPrefs.GetInt ("win").ToString ();

        playTimeText.text = minutePlayTime.ToString ("00") + " : " + secondPlayTime.ToString ("00");
        matchText.text = PlayerPrefs.GetInt ("totalGame").ToString ();

        float win = PlayerPrefs.GetInt ("win");
        float totalGame = PlayerPrefs.GetInt ("totalGame");
        string WR = ((win / totalGame) * 100).ToString ("0.0");
        winRateText.text = "Win-Rate : " + WR + "%";
    }

    public void SaveUsername ()
    {
        PlayerPrefs.SetString ("username", username.text);
    }

    public void ExitGame ()
    {
        Application.Quit ();
    }
}