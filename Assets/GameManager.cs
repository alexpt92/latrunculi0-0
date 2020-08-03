using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int sizeX;
    public int sizeY;
    public GameObject wPrefab;
    public GameObject bPrefab;

    public Board mBoard;
    public PieceManager mPieceManager;

    private bool gameRunning = false;
    // private int maxRounds = 10;
  //  private int currentRound = 1;
    //private int maxPlayers = 2;
    //private int currentPlayer = 1;
   // string winner = null;


    private bool moveAgain;

    // Start is called before the first frame update
    void Start()
    {
        //Create Board
        mBoard.Create();


        //Create Pieces

        mPieceManager.Setup(mBoard);
        Camera.main.transform.position = new Vector3(1, 1, 1);
        Camera.main.orthographicSize = 9 * 100;
        Camera.main.gameObject.SetActive(true);


        GameObject.FindGameObjectWithTag("BoardCanvas").transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 125));
        GameObject.FindGameObjectWithTag("Pieces").transform.position = GameObject.FindGameObjectWithTag("BoardCanvas").transform.position;

        //Camera.main.orthographicSize = 9 * 40;


    }


}
