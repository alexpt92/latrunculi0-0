using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceColor
{
    WHITE, BLACK
};

public enum PieceType
{
    MAN,
    KING
};


public class PieceDraught : MonoBehaviour
{

    public int x;
    public int y;
    public PieceColor color;
    public PieceType type;

    public void Setup(int x, int y, PieceColor color,
        PieceType type = PieceType.MAN)
    {
        this.x = x;
        this.y = y;
        this.color = color;
        this.type = type;
    }


   /* public void Move(MoveDraughts move, ref Piece [,] board)
    {
        board[move.y, move.x] = this;
        board[y, x] = null; 
        x = move.x; 
        y = move.y;    

        if (move.success)
        {
            Destroy(board[move.removeY, move.removeX]);
            board[move.removeY, move.removeX] = null;
        }

       /* if (type == PieceType.KING)
            return;*/
       //prepared for king-version
   /* }

    private bool IsMoveInBounds(int x, int y, ref Piece[,] board)
    {
        int rows = board.GetLength(0);
        int cols = board.GetLength(1);
        if (x < 0 || x >= cols || y < 0 || y >= rows)
            return false;
        return true;
    }


    public Move[] GetMoves(ref Piece[,] board)
    {
        List<Move> moves = new List<Move>();
        if (type == PieceType.KING) 
        {
            // moves = GetMovesKing(ref board);

        }
        else
            moves = GetMovesMan(ref board);
        return moves.ToArray();
    }

    private List<Move> GetMovesMan(ref Piece[,] board)
    {
        List<Move> moves = new List<Move>();
        return new List<Move>();
    }


    public void CreateCellPath(int xDirection, int yDirection, int movement)
    {
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;

        for (int i = 1; i <= movement; i++)
        {
            currentX += xDirection;
            currentY += yDirection;

            CellState cellState = CellState.None;
            cellState = mCurrentCell.mBoard.ValidateCell(currentX, currentY, this);

            //If enemy, break;
            if (cellState == CellState.Enemy)
            {
                break;
            }

            //get CellState
            if (cellState != CellState.Free)
                break;

            //ADD to Highlighted List
            mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);




        }



    }

    protected virtual void CheckPathing()
    {
        //Horizontal
        CreateCellPath(1, 0, mMovement.x);
        CreateCellPath(-1, 0, mMovement.x);

        //Vertical
        CreateCellPath(0, 1, mMovement.y);
        CreateCellPath(0, -1, mMovement.y);

        /*  //Upper diagonal
           CreateCellPath(1, 1, mMovement.z);
           CreateCellPath(-1, 1, mMovement.z);

           //Lower diagonal
           CreateCellPath(-1, -1, mMovement.z);
           CreateCellPath(1, -1, mMovement.z);
    }*/


}
