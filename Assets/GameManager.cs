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
  //  private int currentRound = 1; ?
    private int maxPlayers = 2;
    protected int currentPlayer;
    BoardAI mAI;
    string winner = null;


    private bool moveAgain;

    // Start is called before the first frame update
    void Start()
    {
        //Create Board

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
           // BoardAI.Minimax(mBoard, currentPlayer, 2, 0, ref m);
        }
        if (mBoard.GetCurrentPlayer() == 2)
        {

        }
    }

    public void nextPlayer()
    {
        if (GameOver() != null)
        {
            Debug.Log(GameOver() + "wins.");
            mPieceManager.ResetPieces();
            nextPlayer();
        }
        else
        {
            if (currentPlayer == null)
                currentPlayer = 1;
            else if (currentPlayer == 1)
            {
                currentPlayer = 2;
                Move m = new Move();
                float iwas;
                List<Move> moves = new List<Move>();
                mPieceManager.setBoard(mBoard);


                for (int i = 0; i < mPieceManager.getBPieces().ToArray().Length; i++)
                {
                    List<Piece> list = mPieceManager.getBPieces();
                    mPieceManager.getBPieces()[i].getPossibleActions();
                    bool checkDead = list[i].isDead();
                    if (!mPieceManager.getBPieces()[i].isDead())
                        moves.AddRange(mPieceManager.getBPieces()[i].moves);
                }

                //!!!!!!!!!!!!!!TODO

                // m.mPiece.PlaceByAI(mBoard.mAllCells[m.x, m.y].transform.localPosition);
                Random rnd = new Random();
                int r = Random.Range(0, moves.Count);
                m = moves.ToArray()[r];

                Debug.Log("Next Move: " + m.mPiece.name);
                Debug.Log("Next Move: CurrentCell: " + m.mPiece.getCurrentCell().name);
                Debug.Log("Next Move: TargetCell: " + m.mPiece.getTargetCell().name);

                List<Piece> blist = mPieceManager.getBPieces();
                for (int i = 0; i < blist.ToArray().Length; i++)
                {
                    if (blist.ToArray()[i].name == m.mPiece.name)
                    {
                        blist.ToArray()[i].PlaceByAI(m.mPiece.getTargetCell());
                        break;
                    }
                }
            }
            else
                currentPlayer = 1;
        }
    }
    public int getCurrentPlayer ()
    {
        return currentPlayer;
    }

    public void MoveAgain(int currentPlayer)
    {
        if (currentPlayer == 2)
            this.currentPlayer = 1;
        else if (currentPlayer == 1)
            this.currentPlayer = 2;
    }

    public string GameOver()
    {
        if (mPieceManager.getBPieces().ToArray().Length < 2)
            return "Player 1";
        else if (mPieceManager.getWPieces().ToArray().Length < 2)
            return "Player 2";
        else return null;
    }

}
