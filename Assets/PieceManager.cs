using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;


public class PieceManager : MonoBehaviour
{
    public GameObject mPiecePrefab;
    public bool moveAgain = false;
    public bool isAIActive = true;


    private List<Piece> mWPieces = null;
    private List<Piece> mBPieces = null;
    private List<Piece> mAllPieces = null;
    private Board board;


   // private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

    public List<Piece> getBPieces()
    {
        return mBPieces;
    }

    public List<Piece> getWPieces()
    {
        return mWPieces;
    }

    public Board getBoard()
    {
        return board;
    }
    public string[,] getBoardDraught2()
    {

        return board.getDraughtAsStrings();
    }
    public BoardDraught getBoardDraught()
    {
        
        return board.getDraught();
    }

    public void setBoard(Board newBoard)
    {
        board = newBoard;
    }

    public void Setup(Board board2, string aiType)
    {
        this.board = board2;

        isAIActive = aiType != null ? true : false;

        //Create White Pieces
        mWPieces = CreatePieces(Color.white, new Color32(100, 100, 100, 255), board);        
        //Create Black Pieces
        mBPieces = CreatePieces(Color.black, new Color32(0, 0, 0, 255), board);



        PlacePieces(0, mWPieces, board);

        PlacePieces(board.sizeY - 1, mBPieces, board);

        //White starts
        //Switch Sides
        SwitchSides(Color.black);

    }


    private List<Piece> CreatePieces(Color teamColor, Color32 spriteColor, Board board)
    {
        List<Piece> newPieces = new List<Piece>();
        float height = Screen.height;
        float width = Screen.width;
        float midHeight = height / 2;
        float midWidth = width / 2;
        float sizePieceY = height / board.sizeY;
        float sizePieceX = width / board.sizeX;
        for (int i = 0; i < board.sizeX; i++)
        {
            //new Object
            GameObject newPieceObject = Instantiate(mPiecePrefab);
            newPieceObject.transform.SetParent(this.transform);
            string currentColor;
            if (teamColor == Color.white)
                currentColor = "White";
            else
                currentColor = "Black";
            newPieceObject.name = "Piece" + currentColor + i;

            //Scale&Position
            newPieceObject.transform.localScale = new Vector3(1, 1, 1);
            newPieceObject.transform.localRotation = Quaternion.identity;
            RectTransform rectTransform = newPieceObject.GetComponent<RectTransform>();
          //  float height = Screen.height / (board.sizeY);

            rectTransform.sizeDelta = new Vector2(sizePieceY+(sizePieceX -sizePieceY), sizePieceY);
            rectTransform.anchoredPosition = new Vector2((i * sizePieceX), (i * sizePieceY));

            //Store piece
            Piece newPiece = (Piece)newPieceObject.AddComponent(typeof(SimplePiece));
            newPieces.Add(newPiece);

            //Setup Piece
            newPiece.Setup(teamColor, spriteColor, this, new Vector3Int(board.sizeX, board.sizeY, 1));
        }
        return newPieces;

    }
    
    private void PlacePieces(int initRow, List<Piece> pieces, Board board)
    {
        for (int i = 0; i<board.sizeX; i++)
        {
            pieces[i].Place(board.mAllCells[i, initRow]);
            board.simpleAllCells[i, initRow] = pieces[i].name;

        }
    }

    private void SetInteractive(List<Piece> allPieces, bool value)
    {
        foreach (Piece piece in allPieces)
            piece.enabled = value;
    }

 

    private void DisableAllPieces()
    {

        SetInteractive(mWPieces, false);
        SetInteractive(mBPieces, false);

    }

    private void EnableAllPieces()
    {
        foreach (Piece piece in mAllPieces)
            piece.enabled = true;
    }
    
    private bool GameOver(Color color)
    {
        //string[,] boarddraught = board.getDraughtAsStrings();
        //GameObject.AddComponent<BoardDraught>();        
       // BoardDraught b = board; 
       // b.Create();
       // b.simpleAllCells = board.getDraughtAsStrings();
        //b.simpleAllCells = boarddraught;
        //= new BoardDraught(boarddraught, 1, board.sizeX, board.sizeY);
        //BoardDraught b2 = new BoardDraught(boarddraught, 2, board.sizeX, board.sizeY);
        board.simpleAllCells = board.getDraughtAsStrings();
        //int player = board.GetCurrentPlayer();
        return (board.IsGameOver());
    }

    public void SwitchSides(Color color)
    {
        Piece[] wPieces = mWPieces.ToArray();
        Piece[] bPieces = mBPieces.ToArray();
        int wDeathCounter = 0;
        int bDeathCounter = 0;
        if (GameOver(color))
        {
            if (color == Color.black)
            {       if (moveAgain)
                    Debug.Log("White wins. No Moves left.");
            else
                Debug.Log("Black wins. No Moves left");
           
            }
            else
            {
                    if (moveAgain)
                    Debug.Log("Black wins. No Moves left");
            else
                Debug.Log("White wins. No Moves left.");

            }
            ResetPieces();
        }
        else {
            for (int i = 0; i < wPieces.Length; i++)
            {
                if (wPieces[i].isDead())
                {
                    wDeathCounter++;
                    if (wDeathCounter == wPieces.Length - 1)
                    {
                        Debug.Log("Black wins.");

                        ResetPieces();
                        //  SwitchSides(Color.black);
                    }
                }
                if (bPieces[i].isDead())
                {
                    bDeathCounter++;
                    if (bDeathCounter == bPieces.Length - 1)
                    {
                        Debug.Log("White wins.");

                        ResetPieces();
                        // SwitchSides(Color.black);
                    }
                }
            }
        }

        bool isBlackTurn = color == Color.white ? true : false;
        if (isAIActive && isBlackTurn)
        {
            GameObject referenceObject;
            GameManager referenceScript;
            referenceObject = GameObject.FindGameObjectWithTag("GameManager");
            referenceScript = referenceObject.GetComponent<GameManager>();
            if (referenceScript.getCurrentPlayer() == 1)
            {
                DisableAllPieces();
            }
            else if (referenceScript.getCurrentPlayer() == 2)
            {
               // referenceScript.MoveAgain(2);
            }

        }
        else
        {
            SetInteractive(mWPieces, !isBlackTurn);
            SetInteractive(mBPieces, isBlackTurn);
        }
        if (!moveAgain)
        {
            NextPlayer();
            getBoard().NextPlayer();
        }
        else if (moveAgain)
        {
            moveAgain = false;
            GameObject referenceObject;
            GameManager referenceScript;
            referenceObject = GameObject.FindGameObjectWithTag("GameManager");
            referenceScript = referenceObject.GetComponent<GameManager>();
            referenceScript.MoveAgain();
        }
    }

    private void NextPlayer()
    {
        GameObject referenceObject;
        GameManager referenceScript;
        referenceObject = GameObject.FindGameObjectWithTag("GameManager");
        referenceScript = referenceObject.GetComponent<GameManager>();
        //if (moveAgain == false)
            
        referenceScript.nextPlayer();
        //Debug.Log(referenceScript.getCurrentPlayer());

    }

    public void ResetPieces()
    {
        //Reset White
        foreach (Piece piece in mWPieces)

        piece.Reset();

        foreach(Piece piece in mBPieces)
        {
            piece.Reset();
        }
        moveAgain = false;
    }

    public Cell GetCellByPos(Vector3 pos)
    {
        for (int i = 0; i < board.sizeX; i++)
        {
            for (int j = 0; j < board.sizeY; j++)
            {
                if (board.mAllCells[i,j].transform.localPosition.x == pos.x && board.mAllCells[i, j].transform.localPosition.x == pos.y)
                {
                    return board.mAllCells[i, j];
                }
            }
        }
        return new Cell();
    }

    public void removePiece (Piece piece)
    {
        if (piece.mColor == Color.white)
        {
            for (int i = 0; i < mWPieces.ToArray().Length; i++)
            {
                if (mWPieces.ToArray()[i].name == piece.name)
                {
                    mWPieces.Remove(piece);
                }
            }
        }
        if (piece.mColor == Color.black)
        {
            for (int i = 0; i < mBPieces.ToArray().Length; i++)
            {
                if (mBPieces.ToArray()[i].name == piece.name)
                {
                    mBPieces.Remove(piece);
                }
            }
        }
    }
}
