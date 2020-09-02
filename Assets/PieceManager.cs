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
    protected Dictionary<Piece, List<Move>> mWMoveLists = null;
    protected Dictionary<Piece, List<Move>> mBMoveLists = null;

   // private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

    public List<Piece> getBPieces()
    {
        return mBPieces;
    }

    public Board getBoard()
    {
        return board;
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


    public Dictionary<Piece, List<Move>> getPossibleActions(string State)
    {
        Dictionary<Piece, List<Move>> tempDic = new Dictionary<Piece, List<Move>>();
        NTree<Move> test = new NTree<Move>(null);
    //    TreeVisitor<Move> iwas = new TreeVisitor<Move>();
        //test.AddChild(mWPieces[0].getPossibleActions());

        for (int i = 0; i<mWPieces.ToArray().Length-1; i++)
        {
            tempDic.Add(mWPieces[i], mWPieces[i].getPossibleActions());
        }
        return null;
    }


    private List<Piece> CreatePieces(Color teamColor, Color32 spriteColor, Board board)
    {
        List<Piece> newPieces = new List<Piece>();

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

    public void SwitchSides(Color color)
    {
        Piece[] wPieces = mWPieces.ToArray();
        Piece[] bPieces = mBPieces.ToArray();
        int wDeathCounter = 0;
        int bDeathCounter = 0;

        for (int i = 0; i < wPieces.Length - 1; i++)
        {
            if (wPieces[i].isDead())
            {
                wDeathCounter++;
                if (wDeathCounter == wPieces.Length - 1)
                {
                    ResetPieces();
                    SwitchSides(Color.black);
                }
            }
            if (bPieces[i].isDead())
            {
                bDeathCounter++;
                if (bDeathCounter == bPieces.Length - 1)
                {
                    ResetPieces();
                    SwitchSides(Color.black);
                }
            }

        }

        /*if (moveAgain)
        {
            if (!(color == Color.black))
            {
                color = Color.white;
            }

        }*/

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

        }
        else
        {
            SetInteractive(mWPieces, !isBlackTurn);
            SetInteractive(mBPieces, isBlackTurn);
        }
        if (!moveAgain)
            NextPlayer();
    }

    private void NextPlayer()
    {
        GameObject referenceObject;
        GameManager referenceScript;
        referenceObject = GameObject.FindGameObjectWithTag("GameManager");
        referenceScript = referenceObject.GetComponent<GameManager>();
        if (moveAgain == false)
            referenceScript.nextPlayer();
        Debug.Log(referenceScript.getCurrentPlayer());

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


}
