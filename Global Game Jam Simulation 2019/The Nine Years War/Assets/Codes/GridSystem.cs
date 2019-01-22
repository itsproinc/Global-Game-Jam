using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GridSystem : MonoBehaviour
{
    public timer Timer;

    public Button startButton;
    public int [] placeDone;

    public GameObject grid;

    public int xSize, ySize;
    public int gridSize;
    public GameObject curGrid;

    public List<GameObject> playerGrid = new List<GameObject> ();
    public List<GameObject> emptyPlayerGrid = new List<GameObject> ();
    public List<GameObject> enemyGrid = new List<GameObject> ();

    public List<Vector2> playerGridPos = new List<Vector2> ();
    public List<Vector2> enemyGridPos = new List<Vector2> ();

    public Transform pGrid;
    public Transform eGrid;
    public GameObject shipMenu;

    public GameObject [] ships;
    public GameObject [] eShips;
    public int [] eShipsHP;
    public int [] pShipsHP;
    public GameObject currentShip;
    public int shipIndex;
    public bool playerShipIndex;

    public int shipRotation;
    public int shipLength;
    public int prevRotation;
    public int prevShip;

    public bool isHovering;
    public GameObject ghost;
    public List<GameObject> ghostGrid = new List<GameObject> ();

    public GameObject prevHighlight;

    public float Offset;
    public bool canPlace;
    public int randomIndex;

    public bool generatingEnemy;
    public int eIndex;

    public bool attackPhase;
    public int attackChance;
    public Sprite cross;
    public Sprite normal;
    public bool enemyTurn;

    public float playerHP;
    public float enemyHP;
    public GameObject pHP;
    public GameObject eHP;

    public AudioSource audioSource;
    public AudioClip sfxCannon;
    public AudioClip sfxExplosion;
    public AudioClip sfxSplash;
    public AudioClip sfxBoom;

    public AudioSource bgmAudioSource;
    public AudioClip bgm01;
    public AudioClip bgm02;

    public Text turnArrow;
    public GameObject gameOver;

    public int s_playTime;
    public int m_playTime;
    public int win;
    public int totalGame;
    public bool saved;

    public float timer;

    public void Reset ()
    {
        xSize = 11;
        ySize = 12;
        gridSize = 75;

        audioSource = GetComponent<AudioSource> ();
        bgmAudioSource = GameObject.Find ("BGM").GetComponent<AudioSource> ();
        startButton = GameObject.Find ("btn_start").GetComponent<Button> ();

        pGrid = GameObject.Find ("Player Grid").GetComponent<Transform> ();
        eGrid = GameObject.Find ("Enemy Grid").GetComponent<Transform> ();
        pHP = GameObject.Find ("health");
        eHP = GameObject.Find ("health_lawan");
        shipMenu = GameObject.Find ("Ship Menu");

        turnArrow = GameObject.Find ("TurnArrow").GetComponent<Text> ();
        gameOver = GameObject.Find ("bg_over");
    }

    public void Start ()
    {
        saved = false;
        Timer = GetComponent<timer> ();

        placeDone = new int [ships.Length];
        eShipsHP = new int [eShips.Length];
        eShipsHP [0] = 4;
        eShipsHP [1] = 3;
        eShipsHP [2] = 2;

        pShipsHP = new int [eShips.Length];
        pShipsHP [0] = 4;
        pShipsHP [1] = 3;
        pShipsHP [2] = 2;

        pGrid.gameObject.SetActive (true);
        eGrid.gameObject.SetActive (true);
        shipMenu.SetActive (false);
        generatingEnemy = false;

        attackPhase = false;
        attackChance = 3;

        playerHP = 9;
        enemyHP = 9;
        prevShip = -1;

        GenerateGrid ();
        ShipPlacementMode ();
        Music ();
        enemyTurn = false;
    }

    public void Music ()
    {
        bgmAudioSource.PlayOneShot (bgm01);
    }

    public void GenerateGrid ()
    {
        // Generate player's grid
        int index = 0;
        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                GameObject _grid = Instantiate (grid, new Vector3 ((gridSize * x) + 75, 900 - (75 * y), 0), Quaternion.identity, pGrid);
                _grid.name = "Player" + index;

                EventTrigger.Entry eventtype = new EventTrigger.Entry ();
                eventtype.eventID = EventTriggerType.PointerEnter;
                eventtype.callback.AddListener ((eventData) => { Hover (_grid, true); });

                EventTrigger.Entry eventtype2 = new EventTrigger.Entry ();
                eventtype2.eventID = EventTriggerType.PointerExit;
                eventtype2.callback.AddListener ((eventData) => { EndHover (); });

                _grid.GetComponent<EventTrigger> ().triggers.Add (eventtype);
                _grid.GetComponent<EventTrigger> ().triggers.Add (eventtype2);

                playerGrid.Add (_grid);
                emptyPlayerGrid.Add (_grid);
                playerGridPos.Add (new Vector2 (x, y));
                index++;
            }
        }

        // Generate enemy's grid
        index = 0;
        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                GameObject _grid = Instantiate (grid, new Vector3 ((gridSize * x) + 1095, 900 - (75 * y), 0), Quaternion.identity, eGrid);
                _grid.name = "Enemy" + index;

                EventTrigger.Entry eventtype = new EventTrigger.Entry ();
                eventtype.eventID = EventTriggerType.PointerEnter;
                eventtype.callback.AddListener ((eventData) => { HoverHighlight (_grid); });
                _grid.GetComponent<EventTrigger> ().triggers.Add (eventtype);

                _grid.GetComponent<Button> ().onClick.AddListener (() => Attack (_grid));

                enemyGrid.Add (_grid);
                enemyGridPos.Add (new Vector2 (x, y));
                index++;
            }
        }
    }

    public void ShipPlacementMode ()
    {
        eGrid.gameObject.SetActive (false);
        shipMenu.SetActive (true);
    }

    public void PlaceShip (int sIndex)
    {
        shipIndex = sIndex;
        currentShip = ships [shipIndex];
        if (shipIndex == prevShip)
            shipRotation = prevRotation;
        else
            shipRotation = 0;

        ShipInfo ();

        // Reset
        if (sIndex == 0)
        {
            foreach (GameObject playerShip in playerGrid)
            {
                if (playerShip.name == "Ship01")
                    playerShip.name = "Player" + playerGrid.IndexOf (playerShip);

                placeDone [0] = 0;
            }
        }
        else if (sIndex == 1)
        {
            foreach (GameObject playerShip in playerGrid)
            {
                if (playerShip.name == "Ship02")
                    playerShip.name = "Player" + playerGrid.IndexOf (playerShip);

                placeDone [1] = 0;
            }
        }
        else if (sIndex == 2)
        {
            foreach (GameObject playerShip in playerGrid)
            {
                if (playerShip.name == "Ship03")
                    playerShip.name = "Player" + playerGrid.IndexOf (playerShip);

                placeDone [2] = 0;
            }
        }
    }

    public void ShipInfo ()
    {
        if (shipIndex == 0)
        {
            shipLength = 4;
            Offset = 0;
            prevShip = 0;
        }
        else if (shipIndex == 1)
        {
            shipLength = 3;
            Offset = -37.5f;
            prevShip = 1;
        }
        else if (shipIndex == 2)
        {
            shipLength = 2;
            Offset = -75f;
            prevShip = 2;
        }
    }

    public void Update ()
    {
        // BGM
        bool playOnce = false;
        if (!bgmAudioSource.isPlaying && !playOnce)
        {
            playOnce = true;
            bgmAudioSource.clip = bgm02;
            bgmAudioSource.loop = true;
            bgmAudioSource.Play ();
        }

        // Start button
        if (placeDone [0] == 1 && placeDone [1] == 1 && placeDone [2] == 1)
        {
            startButton.enabled = true;
            startButton.GetComponent<Image> ().color = Color.white;
        }
        else
        {
            startButton.enabled = false;
            startButton.GetComponent<Image> ().color = new Color32 (255, 255, 255, 100);
        }

        if (currentShip != null && isHovering)
        {
            if (Input.GetKeyDown (KeyCode.R))
            {
                ghost.transform.eulerAngles = new Vector3 (ghost.transform.eulerAngles.x, ghost.transform.eulerAngles.y, ghost.transform.eulerAngles.z - 90);

                if (shipRotation == 0) // => 90 degree
                {
                    ghost.transform.position = new Vector3 (ghost.transform.position.x - 112.5f - Offset, ghost.transform.position.y - 112.5f - Offset, ghost.transform.position.z);
                    shipRotation = 90;
                }
                else if (shipRotation == 90) // => 180 degree
                {
                    ghost.transform.position = new Vector3 (ghost.transform.position.x - 112.5f - Offset, ghost.transform.position.y + 112.5f + Offset, ghost.transform.position.z);
                    shipRotation = 180;
                }
                else if (shipRotation == 180) // => 270 degree
                {
                    ghost.transform.position = new Vector3 (ghost.transform.position.x + 112.5f + Offset, ghost.transform.position.y + 112.5f + Offset, ghost.transform.position.z);
                    shipRotation = 270;
                }
                else if (shipRotation == 270) // => 0 degree
                {
                    ghost.transform.position = new Vector3 (ghost.transform.position.x + 112.5f + Offset, ghost.transform.position.y - 112.5f - Offset, ghost.transform.position.z);
                    shipRotation = 0;
                }

                prevRotation = shipRotation;
            }

            CheckGrid (curGrid);
        }

        /*
        if (Input.GetKeyDown (KeyCode.P))
        {
            EnemyShip ();
        }
         */

        // Play time
        if (timer <= 60)
        {
            timer += Time.deltaTime;
            s_playTime = Mathf.RoundToInt (timer);
        }
        else
        {
            timer = 0;
            m_playTime++;
        }
    }

    public void Hover (GameObject _curGrid, bool isPlayer)
    {
        if (!generatingEnemy)
        {
            curGrid = _curGrid;
            isHovering = true;
            if (currentShip != null)
            {
                ghost = currentShip;

                int index = 0;
                index = playerGrid.IndexOf (curGrid);

                if (shipRotation == 0)
                    ghost.transform.position = new Vector2 (playerGrid [index].transform.position.x + 112.5f + Offset, playerGrid [index].transform.position.y);

                if (shipRotation == 90)
                    ghost.transform.position = new Vector2 (playerGrid [index].transform.position.x, playerGrid [index].transform.position.y - 112.5f - Offset);

                if (shipRotation == 180)
                    ghost.transform.position = new Vector2 (playerGrid [index].transform.position.x - 112.5f - Offset, playerGrid [index].transform.position.y);

                if (shipRotation == 270)
                    ghost.transform.position = new Vector2 (playerGrid [index].transform.position.x, playerGrid [index].transform.position.y +
                        112.5f + Offset);
                ghost.GetComponent<Image> ().color = Color.white;

                CheckGrid (curGrid);
            }
        }
    }

    public void EndHover ()
    {
        isHovering = false;
    }

    public void CheckGrid (GameObject curGrid)
    {
        int index = playerGrid.IndexOf (curGrid);
        ghostGrid.Clear ();
        try
        {
            if (shipRotation == 0) // Horizontal
            {
                for (int i = 0; i < shipLength; i++)
                {
                    ghostGrid.Add (playerGrid [index + i]);
                    if (playerGrid [index + i].name.Contains ("Ship") || (index + i) >= xSize * (playerGridPos [index].y + 1))
                    {
                        ghost.GetComponent<Image> ().color = Color.red;
                        canPlace = false;
                        return;
                    }
                    else
                    {
                        ghost.GetComponent<Image> ().color = Color.white;
                        canPlace = true;
                    }
                }
            }
            else if (shipRotation == 90) // Bottom
            {
                for (int i = 0; i < shipLength; i++)
                {
                    ghostGrid.Add (playerGrid [index + (xSize * i)]);
                    if (playerGrid [index + (xSize * i)].name.Contains ("Ship"))
                    {
                        ghost.GetComponent<Image> ().color = Color.red;
                        canPlace = false;
                        return;
                    }
                    else
                    {
                        ghost.GetComponent<Image> ().color = Color.white;
                        canPlace = true;
                    }
                }
            }
            else if (shipRotation == 180) // Left
            {
                for (int i = 0; i < shipLength; i++)
                {
                    ghostGrid.Add (playerGrid [index - i]);
                    if (playerGrid [index - i].name.Contains ("Ship") || (index - i) < (playerGridPos [index].y * xSize))
                    {
                        ghost.GetComponent<Image> ().color = Color.red;
                        canPlace = false;
                        return;
                    }
                    else
                    {
                        ghost.GetComponent<Image> ().color = Color.white;
                        canPlace = true;
                    }
                }
            }
            else if (shipRotation == 270) // Up
            {
                for (int i = 0; i < shipLength; i++)
                {
                    ghostGrid.Add (playerGrid [index - (xSize * i)]);
                    if (playerGrid [index - (xSize * i)].name.Contains ("Ship"))
                    {
                        ghost.GetComponent<Image> ().color = Color.red;
                        canPlace = false;
                        return;
                    }
                    else
                    {
                        ghost.GetComponent<Image> ().color = Color.white;
                        canPlace = true;
                    }
                }
            }
        }
        catch
        {
            ghost.GetComponent<Image> ().color = Color.red;
            canPlace = false;
        }
    }

    public void PlaceinGrid ()
    {
        if (canPlace && !attackPhase)
        {
            currentShip = null;

            int index;
            for (int i = 0; i < ghostGrid.Count; i++)
            {
                index = playerGrid.IndexOf (ghostGrid [i]);

                if (shipIndex == 0)
                {
                    playerGrid [index].name = "Ship01";
                    placeDone [0] = 1;
                }
                else if (shipIndex == 1)
                {
                    playerGrid [index].name = "Ship02";
                    placeDone [1] = 1;
                }
                else if (shipIndex == 2)
                {
                    playerGrid [index].name = "Ship03";
                    placeDone [2] = 1;
                }
            }
        }
    }

    public void EnemyShip ()
    {
        generatingEnemy = true;
        eIndex = 0;
        turnArrow.text = ">";
        while (eIndex < eShips.Length)
        {
            Debug.Log (eIndex);
            randomIndex = UnityEngine.Random.Range (0, enemyGrid.Count);
            int randomRotation = UnityEngine.Random.Range (1, 4);
            int randomRotDegree = 90 * randomRotation;

            shipIndex = eIndex;
            ShipInfo ();

            currentShip = eShips [eIndex];
            shipRotation = randomRotDegree;
            ghost = currentShip;
            ghost.GetComponent<Image> ().color = new Color32 (0, 0, 0, 0);

            ghost.transform.eulerAngles = new Vector3 (ghost.transform.eulerAngles.x, ghost.transform.eulerAngles.y, 0); // Reset rotation
            ghost.transform.eulerAngles = new Vector3 (ghost.transform.eulerAngles.x, ghost.transform.eulerAngles.y, ghost.transform.eulerAngles.z - shipRotation);

            if (shipRotation == 0)
                ghost.transform.position = new Vector2 (enemyGrid [randomIndex].transform.position.x + 112.5f + Offset, enemyGrid [randomIndex].transform.position.y);

            if (shipRotation == 90)
                ghost.transform.position = new Vector2 (enemyGrid [randomIndex].transform.position.x, enemyGrid [randomIndex].transform.position.y - 112.5f - Offset);

            if (shipRotation == 180)
                ghost.transform.position = new Vector2 (enemyGrid [randomIndex].transform.position.x - 112.5f - Offset, enemyGrid [randomIndex].transform.position.y);

            if (shipRotation == 270)
                ghost.transform.position = new Vector2 (enemyGrid [randomIndex].transform.position.x, enemyGrid [randomIndex].transform.position.y + 112.5f + Offset);

            // Check enemy grid
            try
            {
                ghostGrid.Clear ();
                canPlace = true;
                if (shipRotation == 0) // Horizontal
                {
                    for (int j = 0; j < shipLength; j++)
                    {
                        ghostGrid.Add (enemyGrid [randomIndex + j]);
                        if (enemyGrid [randomIndex + j].name.Contains ("Ship") || (randomIndex + j) >= xSize * (enemyGridPos [randomIndex].y + 1))
                        {
                            canPlace = false;
                            break;
                        }
                    }
                }
                else if (shipRotation == 90) // Bottom
                {
                    for (int j = 0; j < shipLength; j++)
                    {
                        ghostGrid.Add (enemyGrid [randomIndex + (xSize * j)]);
                        if (enemyGrid [randomIndex + (xSize * j)].name.Contains ("Ship") || randomIndex + (xSize * j) > enemyGrid.Count)
                        {
                            canPlace = false;
                            break;
                        }
                    }
                }
                else if (shipRotation == 180) // Left
                {
                    for (int j = 0; j < shipLength; j++)
                    {
                        ghostGrid.Add (enemyGrid [randomIndex - j]);
                        if (enemyGrid [randomIndex - j].name.Contains ("Ship") || (randomIndex - j) < (enemyGridPos [randomIndex].y * xSize))
                        {
                            canPlace = false;
                            break;
                        }
                    }
                }
                else if (shipRotation == 270) // Up
                {
                    for (int j = 0; j < shipLength; j++)
                    {
                        ghostGrid.Add (enemyGrid [randomIndex - (xSize * j)]);
                        if (enemyGrid [randomIndex - (xSize * j)].name.Contains ("Ship") || randomIndex - (xSize * j) < 0)
                        {
                            canPlace = false;
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                canPlace = false;
                Debug.Log ("Failed, ship collided");
            }

            if (canPlace)
            {
                // Reset
                if (shipIndex == 0)
                {
                    foreach (GameObject enemyShip in enemyGrid)
                    {
                        if (enemyShip.name == "Ship01")
                            enemyShip.name = "Enemy" + enemyGrid.IndexOf (enemyShip);
                    }
                }
                else if (shipIndex == 1)
                {
                    foreach (GameObject enemyShip in enemyGrid)
                    {
                        if (enemyShip.name == "Ship02")
                            enemyShip.name = "Enemy" + enemyGrid.IndexOf (enemyShip);
                    }
                }
                else if (shipIndex == 2)
                {
                    foreach (GameObject enemyShip in enemyGrid)
                    {
                        if (enemyShip.name == "Ship03")
                            enemyShip.name = "Enemy" + enemyGrid.IndexOf (enemyShip);
                    }
                }

                // Set ship to array
                currentShip = null;
                int index;
                for (int k = 0; k < ghostGrid.Count; k++)
                {
                    index = enemyGrid.IndexOf (ghostGrid [k]);

                    if (shipIndex == 0)
                        enemyGrid [index].name = "Ship01";
                    else if (shipIndex == 1)
                        enemyGrid [index].name = "Ship02";
                    else if (shipIndex == 2)
                        enemyGrid [index].name = "Ship03";
                }

                eIndex++;
            }
            else
            {
                Debug.Log ("Failed, ship collided");
            }
        }

        generatingEnemy = false;
        attackPhase = true;
        Timer.gameStarted = true;
    }

    public void HoverHighlight (GameObject grid)
    {
        if (!enemyTurn)
        {
            if (prevHighlight != null && attackPhase)
            {
                prevHighlight.GetComponent<Image> ().color = new Color32 (255, 255, 255, 50);
            }

            if (grid.name != "Empty")
            {
                if (grid.name != "Destroy")
                {
                    grid.GetComponent<Image> ().color = Color.red;
                    prevHighlight = grid;
                }
                else
                    prevHighlight = null;
            }
            else
                prevHighlight = null;
        }
    }

    public void Attack (GameObject grid)
    {
        if (attackChance > 0)
        {
            if (grid.name != "Destroy")
            {
                if (grid.name != "Empty")
                {
                    attackChance--;
                    audioSource.PlayOneShot (sfxCannon);
                    if (grid.name.Contains ("Ship"))
                    {
                        audioSource.PlayOneShot (sfxExplosion);
                        if (grid.name == "Ship01")
                        {
                            int hp = eShipsHP [0];
                            hp--;
                            int [] _hp = eShipsHP;
                            eShipsHP = new int [eShips.Length];
                            eShipsHP [0] = hp;
                            eShipsHP [1] = _hp [1];
                            eShipsHP [2] = _hp [2];
                        }
                        else if (grid.name == "Ship02")
                        {
                            int hp = eShipsHP [1];
                            hp--;
                            int [] _hp = eShipsHP;
                            eShipsHP = new int [eShips.Length];
                            eShipsHP [0] = _hp [0];
                            eShipsHP [1] = hp;
                            eShipsHP [2] = _hp [2];
                        }
                        if (grid.name == "Ship03")
                        {
                            int hp = eShipsHP [2];
                            hp--;
                            int [] _hp = eShipsHP;
                            eShipsHP = new int [eShips.Length];
                            eShipsHP [0] = _hp [0];
                            eShipsHP [1] = _hp [1];
                            eShipsHP [2] = hp;
                        }

                        grid.GetComponent<Image> ().color = Color.red;
                        prevHighlight = null;
                        grid.name = "Destroy";

                        enemyHP--;
                        UpdateHPBar ();

                        if (eShipsHP [0] == 0)
                        {
                            eShips [0].GetComponent<Image> ().color = new Color32 (100, 100, 100, 100);
                            audioSource.PlayOneShot (sfxBoom);
                        }

                        if (eShipsHP [1] == 0)
                        {
                            eShips [1].GetComponent<Image> ().color = new Color32 (100, 100, 100, 100);
                            audioSource.PlayOneShot (sfxBoom);
                        }

                        if (eShipsHP [2] == 0)
                        {
                            eShips [2].GetComponent<Image> ().color = new Color32 (100, 100, 100, 100);
                            audioSource.PlayOneShot (sfxBoom);
                        }
                    }
                    else
                    {
                        grid.GetComponent<Image> ().sprite = cross;
                        grid.GetComponent<Image> ().color = Color.red;
                        prevHighlight = null;
                        grid.name = "Empty";
                        audioSource.PlayOneShot (sfxSplash);

                    }

                    if (attackChance == 0)
                    {
                        Timer.reset = true;
                        Timer.timer1 = 0;
                        StartCoroutine (EnemyAttack ());
                    }
                }
            }
        }
    }

    public IEnumerator EnemyAttack ()
    {
        attackChance = 0;
        turnArrow.text = "<";
        int enemyChance = 5;
        enemyTurn = true;
        while (enemyChance > 0 && enemyHP > 0)
        {
            yield return new WaitForSeconds (0.5f);
            randomIndex = UnityEngine.Random.Range (0, emptyPlayerGrid.Count);
            GameObject grid = emptyPlayerGrid [randomIndex];
            if (grid.name != "Empty")
            {
                if (grid.name != "Destroy")
                {
                    audioSource.PlayOneShot (sfxCannon);

                    if (grid.name.Contains ("Ship"))
                    {
                        if (grid.name == "Ship01")
                        {
                            int hp = pShipsHP [0];
                            hp--;
                            int [] _hp = pShipsHP;
                            pShipsHP = new int [eShips.Length];
                            pShipsHP [0] = hp;
                            pShipsHP [1] = _hp [1];
                            pShipsHP [2] = _hp [2];
                        }
                        else if (grid.name == "Ship02")
                        {
                            int hp = pShipsHP [1];
                            hp--;
                            int [] _hp = pShipsHP;
                            pShipsHP = new int [eShips.Length];
                            pShipsHP [0] = _hp [0];
                            pShipsHP [1] = hp;
                            pShipsHP [2] = _hp [2];
                        }
                        if (grid.name == "Ship03")
                        {
                            int hp = pShipsHP [2];
                            hp--;
                            int [] _hp = pShipsHP;
                            pShipsHP = new int [eShips.Length];
                            pShipsHP [0] = _hp [0];
                            pShipsHP [1] = _hp [1];
                            pShipsHP [2] = hp;
                        }

                        audioSource.PlayOneShot (sfxExplosion);
                        grid.GetComponent<Image> ().color = Color.red;
                        prevHighlight = null;
                        grid.name = "Destroy";

                        playerHP--;
                        UpdateHPBar ();

                        if (pShipsHP [0] == 0)
                        {
                            ships [0].GetComponent<Image> ().color = new Color32 (100, 100, 100, 100);
                            audioSource.PlayOneShot (sfxBoom);
                        }

                        if (pShipsHP [1] == 0)
                        {
                            ships [1].GetComponent<Image> ().color = new Color32 (100, 100, 100, 100);
                            audioSource.PlayOneShot (sfxBoom);
                        }

                        if (pShipsHP [2] == 0)
                        {
                            ships [2].GetComponent<Image> ().color = new Color32 (100, 100, 100, 100);
                            audioSource.PlayOneShot (sfxBoom);
                        }
                    }
                    else
                    {
                        grid.GetComponent<Image> ().sprite = cross;
                        grid.GetComponent<Image> ().color = Color.red;
                        prevHighlight = null;
                        emptyPlayerGrid [emptyPlayerGrid.IndexOf (grid)].name = "Clear";
                        List<GameObject> _emptyPlayerGrid = new List<GameObject> ();
                        foreach (GameObject _epg in emptyPlayerGrid)
                        {
                            if (_epg.name != "Clear")
                            {
                                if (_epg.name != "Destroy")
                                    _emptyPlayerGrid.Add (_epg);
                            }
                        }

                        emptyPlayerGrid.Clear ();
                        foreach (GameObject _epg in _emptyPlayerGrid)
                        {
                            emptyPlayerGrid.Add (_epg);
                        }

                        audioSource.PlayOneShot (sfxSplash);
                    }

                    enemyChance--;

                    if (enemyChance == 0)
                    {
                        turnArrow.text = ">";
                        enemyTurn = false;
                        attackChance = 3;
                        Timer.timer1 = 10f;
                        Timer.reset = false;
                    }
                }
            }
        }
    }

    public void UpdateHPBar ()
    {
        float _playerHP = playerHP / 9;
        float _enemyHP = enemyHP / 9;

        Vector3 newPHP = new Vector3 (_playerHP, pHP.transform.localScale.y, pHP.transform.localScale.z);
        Vector3 newEHP = new Vector3 (_enemyHP, eHP.transform.localScale.y, eHP.transform.localScale.z);

        pHP.transform.localScale = Vector3.Lerp (pHP.transform.localScale, newPHP, 10f);
        eHP.transform.localScale = Vector3.Lerp (pHP.transform.localScale, newEHP, 10f);

        if (_playerHP <= 0 || _enemyHP <= 0)
        {
            gameOver.SetActive (true);
            Time.timeScale = 0;

            // Win / Lose
            if (_enemyHP == 0 && _playerHP != 0)
                win++;

            if (!saved)
            {
                saved = true;
                totalGame++;
                SaveProfile ();
                timer = 10;
                s_playTime = 0;
                m_playTime = 0;
            }
        }
    }

    public void pause ()
    {
        bgmAudioSource.mute = bgmAudioSource.mute ? false : true;
    }

    public void TimesUp ()
    {
        if (Timer.gameStarted)
        {
            StartCoroutine (EnemyAttack ());
        }
    }

    public void RestartGame ()
    {
        if (!saved)
        {
            saved = true;
            totalGame++;
            SaveProfile ();
        }
        Scene scene = SceneManager.GetActiveScene ();
        SceneManager.LoadScene (scene.name);
    }

    public void BackToMenu ()
    {
        if (!saved)
        {
            saved = true;
            totalGame++;
            SaveProfile ();
        }
        SceneManager.LoadScene (0);
    }

    public void SaveProfile ()
    {
        if (PlayerPrefs.HasKey ("s_playTime"))
        {
            int prevSPlayTime = PlayerPrefs.GetInt ("s_playTime");
            int totalPlayTime = prevSPlayTime + s_playTime;
            if (totalPlayTime >= 60)
            {
                m_playTime++;
                totalPlayTime -= 60;
            }

            PlayerPrefs.SetInt ("s_playTime", s_playTime);
        }
        else
            PlayerPrefs.SetInt ("s_playTime", s_playTime);

        if (PlayerPrefs.HasKey ("m_playTime"))
        {
            int prevMPlayTime = PlayerPrefs.GetInt ("m_playTime");
            PlayerPrefs.SetInt ("m_playTime", prevMPlayTime + m_playTime);
        }
        else
            PlayerPrefs.SetInt ("m_playTime", m_playTime);

        if (PlayerPrefs.HasKey ("win"))
        {
            int prevWin = PlayerPrefs.GetInt ("win");
            PlayerPrefs.SetInt ("win", prevWin + win);
        }
        else
            PlayerPrefs.SetInt ("win", win);

        if (PlayerPrefs.HasKey ("totalGame"))
        {
            int prevTotalGame = PlayerPrefs.GetInt ("totalGame");
            PlayerPrefs.SetInt ("totalGame", prevTotalGame + totalGame);
        }
        else
            PlayerPrefs.SetInt ("totalGame", totalGame);

    }
}