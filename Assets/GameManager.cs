using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{

    public int sizeX;
    public int sizeY;
    public GameObject wPrefab;
    public GameObject bPrefab;

    public Board mBoard;
    public PieceManager mPieceManager;
    public AIManager mAIManager;
    public string mAIType;

    private bool gameRunning = false;
    // private int maxRounds = 10;
  //  private int currentRound = 1;
    private int maxPlayers = 2;
    protected int currentPlayer;
    BoardAI mAI;
    // string winner = null;


    private bool moveAgain;

    // Start is called before the first frame update
    void Start()
    {
        //Create Board

        Debug.Log("iwas");
        mBoard.Create();

        //Create Pieces

        mPieceManager.Setup(mBoard, "iwas");
        // mAIManager.Setup(mAIType);
        Camera.main.transform.position = new Vector3(1, 1, 116);
        Camera.main.orthographicSize = 10 * 100;
        Camera.main.gameObject.SetActive(true);
        
     //   Camera.main.rect = new Rect(0, 0, 900, 900);


        GameObject.FindGameObjectWithTag("BoardCanvas").transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 125));
        GameObject.FindGameObjectWithTag("Pieces").transform.position = GameObject.FindGameObjectWithTag("BoardCanvas").transform.position;

        //Camera.main.orthographicSize = 9 * 40;
    }

    private void Update()
    {
        if (currentPlayer == 2)
        {
            Move m = new Move();
           // BoardAI.Minimax(mBoard, currentPlayer, 2, 0, ref m);
           // currentPlayer = 1;
        }
        if (mBoard.GetCurrentPlayer() == 2)
        {
            //currentPlayer = 2;
        }
    }

    public void nextPlayer()
    {
        if (currentPlayer == null)
            currentPlayer = 1;
        else if (currentPlayer == 1)
        {
            currentPlayer = 2;
            Move m = new Move();
            float iwas;
            List<Move> moves = new List<Move>();
            for (int i = 0; i < mPieceManager.getBPieces().ToArray().Length - 1; i++)
            {
                mPieceManager.getBPieces()[i].getPossibleActions();
                moves.AddRange(mPieceManager.getBPieces()[i].moves);
            }
            //iwas = BoardAI.Minimax(mBoard, currentPlayer, 2, 1, mPieceManager.getBPieces()[2], ref m);

            //!!!!!!!!!!!!!!TODO
            // m.mPiece.PlaceByAI(mBoard.mAllCells[m.x, m.y].transform.localPosition);
            Random rnd = new Random();
           // int r = rnd.Next(moves.Count);
            int r = Random.Range(0, moves.Count);
            m = moves.ToArray()[r];

            
            
            m.mPiece.PlaceByAI(m.mPiece.getTargetCell());//.transform.localPosition);// mBoard.mAllCells[m.x, m.y].transform.localPosition);
            //m.mPiece.
            // mPieceManager.getBPieces()[2].Place(mBoard.mAllCells[4,4]);
            mPieceManager.SwitchSides(Color.black);

        }
        else
            currentPlayer = 1;
    }

    public int getCurrentPlayer ()
    {
        return currentPlayer;
    }

}
