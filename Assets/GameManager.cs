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
    public int maxDepth;

    private bool gameRunning;// = false;
                             // private int maxRounds = 10;
                             //  private int currentRound = 1; ?
    private int maxPlayers;// = 2;
    protected int currentPlayer = 0;
    string winner;

    private bool moveAgain;

    // Start is called before the first frame update
    void Start()
    {
        //Create Board
        // currentPlayer = 1;
        Camera.main.transform.position = new Vector3(1, 1, 116);//mBoard.mAllCells[0, 0].mBoardPosition.x, mBoard.mAllCells[0, 0].mBoardPosition.y, 0);//1, 116);
        Camera.main.orthographicSize = 10 * 100;
        Camera.main.gameObject.SetActive(true);
        mBoard.Create();

        //Create Pieces

        mPieceManager.Setup(mBoard, "iwas");
        Camera.main.transform.position = new Vector3(mBoard.mAllCells[0, 0].mBoardPosition.x , mBoard.mAllCells[0, 0].mBoardPosition.y, 0);//1, 116);
        Camera.main.orthographicSize = 10 * 100;
        Camera.main.gameObject.SetActive(true);
        
     //   Camera.main.rect = new Rect(0, 0, 900, 900);
     //   GameObject.FindGameObjectWithTag("BoardCanvas").transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 125));
        GameObject.FindGameObjectWithTag("Pieces").transform.position = GameObject.FindGameObjectWithTag("BoardCanvas").transform.position;

        //Camera.main.orthographicSize = 9 * 40;
    }

    void Update()
    {
      /*  if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }*/
    }

    public void MovePiece()
    {

        List<Move> moves = new List<Move>();
        mPieceManager.setBoard(mBoard);
        string[,] boarddraught = mBoard.getDraughtAsStrings();
        BoardDraught b = new BoardDraught(boarddraught, currentPlayer, mBoard.sizeX, mBoard.sizeY);
        float test;
        Move currentMove = new Move();

        test = AIManager.Minimax(b, currentPlayer, maxDepth, 0, ref currentMove);

        //  Debug.Log("Moves Again: " + m.mPiece.name + " CurrentCell: " + m.mPiece.getCurrentCell().name + " TargetCell: " + mBoard.mAllCells[m.x, m.y].name);//.mPiece.getTargetCell().name);
       // StartCoroutine(ExecuteMoveAfterDelay(1));
        List<Piece> blist = mPieceManager.getBPieces();
        for (int i = 0; i < blist.ToArray().Length; i++)
        {
            if (blist.ToArray()[i].name == currentMove.mPieceName)
            {
                blist.ToArray()[i].CheckPath(currentMove);
                blist.ToArray()[i].ShowCells();
                //StartCoroutine(ExecuteHighlightAfterDelay(1, i, currentMove));

                StartCoroutine(ExecuteMoveAfterDelay((float)1.5, i, currentMove));

                break;
            }
        }
    }

    IEnumerator ExecuteHighlightAfterDelay(float time, int idx, Move currentMove)
    {
        yield return new WaitForSeconds(time);
        List<Piece> blist = mPieceManager.getBPieces();

        blist.ToArray()[idx].ShowCells();


    }


    public void MoveAgain ()
    {
        if (GameOver() != null)
        {
            Debug.Log(GameOver() + "wins.");
        }
        if (currentPlayer == 2)
        {
           // StartCoroutine(ExecuteMoveAfterDelay(1));
           MovePiece();
    }
}

    IEnumerator ExecuteMoveAfterDelay(float time, int idx, Move currentMove)
    {
        //Print the time of when the function is first called.
        //Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(time);
        // MovePiece();
        List<Piece> blist = mPieceManager.getBPieces();
        blist.ToArray()[idx].PlaceByAI(mBoard.mAllCells[currentMove.x, currentMove.y]);
        blist.ToArray()[idx].ClearCells();

        //After we have waited 5 seconds print the time again.
        //Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
    public void nextPlayer()
    {
        if (GameOver() != null)
        {
            Debug.Log(GameOver() + "wins.");
            //mBoard.NextPlayer();
            //nextPlayer();
        }
        else
        {
            if (currentPlayer == 0)
            {
                currentPlayer = 1;
            }
            else if (currentPlayer == 1)
            {
                currentPlayer = 2;
                //StartCoroutine(ExecuteMoveAfterDelay(1));
                MovePiece();
                /* List<Move> moves = new List<Move>();
                mPieceManager.setBoard(mBoard);

                    string[,] boarddraught = mBoard.getDraughtAsStrings();

                    BoardDraught b = new BoardDraught(boarddraught, currentPlayer, mBoard.sizeX, mBoard.sizeY);
                    float test;
                Move currentMove = null;
                test = AIManager.Minimax(b, currentPlayer, maxDepth, 0, ref currentMove);

                //  Debug.Log("Next Move: " + m.mPieceName + " CurrentCell: " + m.mPiece.getCurrentCell().name + " TargetCell: " + mBoard.mAllCells[m.x, m.y].name);//.mPiece.getTargetCell().name);

                List<Piece> blist = mPieceManager.getBPieces();
                    for (int i = 0; i < blist.ToArray().Length; i++)
                    {
                        if (blist.ToArray()[i].name == currentMove.mPieceName)
                        {
                            blist.ToArray()[i].PlaceByAI(mBoard.mAllCells[currentMove.x, currentMove.y]);
                            break;
                        }
                    }*/
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
       // string[,] boarddraught = mBoard.getDraughtAsStrings();
       // BoardDraught b = new BoardDraught(boarddraught, currentPlayer, mBoard.sizeX, mBoard.sizeY);

        if (mPieceManager.getBPieces().ToArray().Length < 2 || mBoard.IsGameOver())
            return "Player 1";
        else if (mPieceManager.getWPieces().ToArray().Length < 2 || mBoard.IsGameOver())
            return "Player 2";
        else return null;
    }
   /* private void CopySpecialComponents(GameObject _sourceGO, GameObject _targetGO)
    {
        foreach (var component in _sourceGO.GetComponents<Component>())
        {
            var componentType = component.GetType();
            if (componentType != typeof(Transform) &&
                componentType != typeof(MeshFilter) &&
                componentType != typeof(MeshRenderer)
                )
            {
                Debug.Log("Found a component of type " + component.GetType());
                UnityEditorInternal.ComponentUtility.CopyComponent(component);
                UnityEditorInternal.ComponentUtility.PasteComponentAsNew(_targetGO);
                Debug.Log("Copied " + component.GetType() + " from " + _sourceGO.name + " to " + _targetGO.name);
            }
        }
    }*/
}
