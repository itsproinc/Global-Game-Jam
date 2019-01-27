using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgradeables : MonoBehaviour
{
    public Home home;

    public Dialogue dialogue;
    public GameObject dialoguePanel;
    public GameObject upgradePanel;

    public GameObject [] property = new GameObject [8];
    public GameObject [] upgradeablesObject = new GameObject [8];
    public bool [] ownedObjects = new bool [8];
    /*  [0] = Jam dinding
        [1] = Kalendar
        [2] = Tanaman kiri
        [3] = Tanaman tengah
        [4] = Tanaman kanan
        [5] = Lampu
        [6] = Lukisan tembok
        [7] = Tong sampah
     */

    public Transform tooltip;
    public Text toolTipText;
    public string [] tooltipTexts = new string [8];
    public int [] objectPrices = new int [8];

    public int UpgradeablesIndex;

    public Text moneyText;
    public int curMoney;

    public float attackDamage;
    public float attackSpeed;
    public float mental;

    private void Reset ()
    {
        upgradePanel = GameObject.FindGameObjectWithTag ("UI_UpgradePanel");
        dialoguePanel = GameObject.FindGameObjectWithTag ("UI_Dialogue");
        dialogue = dialoguePanel.GetComponent<Dialogue> ();
    }

    private void Start ()
    {
        //PlayerPrefs.DeleteAll ();
        tooltipTexts [0] = "Attack Speed x1.5";
        tooltipTexts [1] = "Attack damage x1.5";
        tooltipTexts [2] = "Mental x1.5";
        tooltipTexts [3] = "Attack Speed x2";
        tooltipTexts [4] = "Attack damage x2";
        tooltipTexts [5] = "Mental x2";
        tooltipTexts [6] = "Attack Speed x2.5";
        tooltipTexts [7] = "Attack damage x2.5";

        objectPrices [0] = 500;
        objectPrices [1] = 1000;
        objectPrices [2] = 1500;
        objectPrices [3] = 4000;
        objectPrices [4] = 2000;
        objectPrices [5] = 5000;
        objectPrices [6] = 10000;
        objectPrices [7] = 3000;

        dialoguePanel.SetActive (false);

        Refresh ();
    }

    // Start is called before the first frame update
    public void Refresh ()
    {
        upgradePanel.SetActive (true);

        curMoney = PlayerPrefs.GetInt ("money");
        attackDamage = PlayerPrefs.GetFloat ("attackDamage");
        attackSpeed = PlayerPrefs.GetFloat ("attackSpeed");
        mental = PlayerPrefs.GetFloat ("mental");

        moneyText.text = curMoney.ToString ();

        for (int i = 0; i < ownedObjects.Length; i++)
        {
            ownedObjects [i] = (PlayerPrefs.GetInt ("OwnedObject" + i) == 1) ? true : false;

            if (ownedObjects [i] == true)
            {
                upgradeablesObject [i].SetActive (false);
                property [i].SetActive (true);
            }
        }

        bool once = false;
        if (!once)
        {
            once = true;
            upgradePanel.SetActive (false);
        }
    }

    public void OnHover (int itemIndex)
    {
        UpgradeablesIndex = itemIndex;

        RectTransform upgrade = upgradeablesObject [itemIndex].GetComponent<RectTransform> ();
        Vector2 pos = new Vector2 (upgradeablesObject [itemIndex].transform.localPosition.x + upgrade.sizeDelta.x + 50, upgradeablesObject [itemIndex].transform.localPosition.y);
        tooltip.localPosition = pos;

        if (curMoney >= objectPrices [itemIndex])
            toolTipText.text = tooltipTexts [itemIndex] + "\n\n" + "Klik untuk beli";
        else
            toolTipText.text = tooltipTexts [itemIndex] + "\n\n" + "Uang tidak mencukupi untuk beli!";
    }

    public void OnExit ()
    {
        tooltip.localPosition = new Vector2 (1920, 1080);
    }

    public void Buy ()
    {
        if (curMoney >= objectPrices [UpgradeablesIndex])
        {
            dialogue._DialogueTrigger ("Wahhh.. Sayang makin cinta aku sama kamu");
            ownedObjects [UpgradeablesIndex] = true;
            curMoney -= objectPrices [UpgradeablesIndex];

            switch (UpgradeablesIndex)
            {
                case 0:
                    attackDamage = 1.5f;
                    break;
                case 1:
                    attackSpeed = 1.5f;
                    break;
                case 2:
                    mental = 1.5f;
                    break;
                case 3:
                    attackDamage = 2;
                    break;
                case 4:
                    attackSpeed = 2;
                    break;
                case 5:
                    mental = 2;
                    break;
                case 6:
                    attackDamage = 2.5f;
                    break;
                case 7:
                    attackSpeed = 2.5f;
                    break;
            }

            home.buyButtonText.text = "Shop";
            tooltip.localPosition = new Vector2 (1920, 1080);
            Save ();
        }
        else
            dialogue._DialogueTrigger ("Kamu perlu cari uang lebih banyak lagi sayang...");
    }

    public void Save ()
    {
        for (int i = 0; i < ownedObjects.Length; i++)
        {
            PlayerPrefs.SetInt ("OwnedObject" + i, ownedObjects [i] ? 1 : 0);
        }

        PlayerPrefs.SetInt ("money", curMoney);
        PlayerPrefs.SetFloat ("attackDamage", attackDamage);
        PlayerPrefs.SetFloat ("attackSpeed", attackSpeed);
        PlayerPrefs.SetFloat ("mental", mental);

        Refresh ();
    }
}