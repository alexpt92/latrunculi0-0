using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    public GameObject mPiecePrefab;
    public bool moveAgain;

    private List<Piece> mWPieces = null;
    private List<Piece> mBPieces = null;

    public void Setup(Board board)
    {
        //Create White Pieces
        mWPieces = CreatePieces(Color.white, new Color32(80, 124, 159, 255), board);        
        //Create Black Pieces
        mBPieces = CreatePieces(Color.black, new Color32(80, 124, 0, 255), board);

        PlacePieces(0, mWPieces, board);

        PlacePieces(board.sizeY - 1, mBPieces, board);

        //White starts
        //Switch Sides
        SwitchSides(Color.black);

    }

    private List<Piece> CreatePieces(Color teamColor, Color32 spriteColor, Board board)
    {
        List<Piece> newPieces = new List<Piece>();

        for (int i = 0; i < board.sizeX; i++)
        {
            //new Object
            GameObject newPieceObject = Instantiate(mPiecePrefab);
            newPieceObject.transform.SetParent(this.transform);

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

        if (moveAgain)
        {
            if (!(color == Color.black))
            {
                color = Color.white;
            }

        }

        bool isBlackTurn = color == Color.white ? true : false;

        SetInteractive(mWPieces, !isBlackTurn);
        SetInteractive(mBPieces, isBlackTurn);
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


}
