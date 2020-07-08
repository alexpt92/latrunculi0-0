using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public boardScript board;

    public int x { get; set; }//Brettgröße X & Y
    public int y { get; set; }
    public float xCoord;
    public float yCoord;
    public int sizeX;
    public int sizeY;
    public pathFinder pathFinder;
    public Sprite o_Sprite;
    string lastSelectedButtonName = null;

    public Sprite m_Sprite;

    private bool gameRunning = false;
   // private int maxRounds = 10;
    private int currentRound = 1;
    private int maxPlayers = 2;
    private int currentPlayer = 1;
    private bool eliminatedInPrevTurn = false;
    string winner = null;
    //private SoundManagerScript soundManager;
   // private int maxBalls = 2;
  // private int currentBall = 1;

    void Start()
    {
        board = new boardScript(sizeX, sizeY, xCoord, yCoord, m_Sprite, o_Sprite);
        pathFinder = new pathFinder();
        gameRunning = true;
        ParticleSystem particleSystemWin = GameObject.Find("ParticlesWin").GetComponent<ParticleSystem>();
       // soundManager = new SoundManagerScript();
        //particleSystemWin.Play();
        particleSystemWin.Stop();

    }

    // Update is called once per frame
    void Update()
    {
        if (gameRunning)
        {
            if (currentPlayer == 1)
            {
                Button[] bButtons = board.bList.ToArray();
                for (int i = 0; i < bButtons.Length; i++)
                {
                    bButtons[i].interactable = false;
                }
                Button[] wButtons = board.wList.ToArray();
                for (int i = 0; i < wButtons.Length; i++)
                {
                    wButtons[i].interactable = true;

                }
            }

            if (currentPlayer == 2)
            {
                Button[] bButtons = board.bList.ToArray();
                for (int i = 0; i < bButtons.Length; i++)
                {
                    bButtons[i].interactable = true;
                }
                Button[] wButtons = board.wList.ToArray();
                for (int i = 0; i < wButtons.Length; i++)
                {
                    wButtons[i].interactable = false;

                }
            }
        }
    }

    public void clicked()
    {
        if (gameRunning)
        {
            string selectedName = EventSystem.current.currentSelectedGameObject.name;
            if (selectedName.Contains("Button"))
            {
                Button actButton = GameObject.Find(selectedName).GetComponent<Button>();
                board.LocateField(selectedName);
                lastSelectedButtonName = selectedName;
                // board.HighlightFields();
            }

            else if (lastSelectedButtonName != null)
            {
                //String[] seperator = { "x" };
                //String[] fieldPos = selectedName.Split(seperator);
                Image clickedField = GameObject.Find(selectedName).GetComponent<Image>();
                board.moveButton(lastSelectedButtonName, clickedField);
                //soundManager.
                //soundManager.
                SoundManagerScript.PlaySound("move");
                board.checkAttack(lastSelectedButtonName);
                lastSelectedButtonName = null;
                eliminatedInPrevTurn = board.moveAgain;
                checkForWin();

                if (winner != null)
                {
                    if (winner == "white")
                    {
                        Debug.Log("Player1 wins");
                        ParticleSystem particleSystemWin = GameObject.Find("ParticlesWin").GetComponent<ParticleSystem>();
                        particleSystemWin.Play();
                    }
                    else if (winner == "black")
                    {
                        Debug.Log("Player2 wins");
                        ParticleSystem particleSystemWin = GameObject.Find("ParticlesWin").GetComponent<ParticleSystem>();
                        particleSystemWin.Play();

                    }
                }
                NextPlayer();
            }
        }
    }


    void checkForWin()
    {
        Button[] bArray = board.bList.ToArray();
        Button[] wArray = board.wList.ToArray();

        if (bArray.Length < 2)
        {
            winner = "white";
            board.disablePieces();
            gameRunning = false;

        }
        else if (wArray.Length < 2)
        {
            winner = "black";
            board.disablePieces();
            gameRunning = false;

        }
    }
    void NextPlayer()
    {
        if (eliminatedInPrevTurn) return;
        if (currentPlayer == 1)
        {
            currentPlayer = 2;
        }
        else
        {
            //NextRound();
            currentPlayer = 1;
        }
    }

    void NextRound()
    {
        currentRound++;
    }
}
