using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;



public enum CellState
{
    None,
    Friendly,
    Enemy,
    Free,
    OutOfBounds
}


public class Board : MonoBehaviour
{

    public GameObject mCellPrefab;
    public int sizeX;
    public int sizeY;
    [HideInInspector]
    public Cell[,] mAllCells;// = new Cell[sizeX, sizeY];
    protected int player;

    public virtual Board MakeMove(Move m)
    {

        Move move = m;
        int nextPlayer;
        if (player == 1)
            nextPlayer = 2;
        else
            nextPlayer = 1;
        
        //Copy Board and make move#

        //Piece[,] copy = new Cell[sizeX, sizeY]; 
        //Array.Copy(board, 0, copy, 0, board.Length);
        Board newBoard = new Board();

        //TODO:!
        //Array.Copy(mAllCells, 0, copy, 0, mAllCells.Length);
      //  newBoard.mAllCells = copy;
        // copy[move.y, move.x] = move; 

        //Board b = new Board(copy, nextPlayer); 
        //return b;
        return newBoard;
      //  return newBoard();
    }

    public bool IsGameOver()
    {
        return false;
    }

    public virtual float Evaluate(int player)
    {
        Color color = Color.white;
        if (player == 1)
            color = Color.black;
        return Evaluate(color);

        //return Mathf.NegativeInfinity;
    }


    public virtual float Evaluate(Color color)
    {

        //TODO!!!!!!!!!!!!!!


        /*  float eval = 1f;
          float pointSimple = 1f;
          float pointSuccess = 5f;
          int rows = sizeX;
          int cols = sizeY;

          if (color == Color.black)
              for (int j = 0; j < cols; j++)
              {
              //    Move[] moves = GetMoves();
              }
          return Mathf.NegativeInfinity;*/
        return Mathf.NegativeInfinity; 


       /* float eval = 1f;
        float pointSimple = 1f;
        float pointSuccess = 5f;
        int rows = sizeY;
        int cols = sizeX;

        int i;
        int j;

        for (i = 0; i < rows; i++)
        {
            for (j = 0; j < cols; j++)
            {
                Piece p = board[i, j];
                if (p == null)
                    continue;
                if (p.mColor != color)
                    continue;
                p.GetMovesMan(ref board);
                Move[] moves = p.GetMoveList().ToArray();
                foreach (Move mv in moves)
                {
                    MoveDraughts m = (MoveDraughts)mv;
                    if (m.success)
                        eval += pointSuccess;
                    else
                        eval += pointSimple;
                }
            }
        }
        return eval;*/

    }

    public virtual float Evaluate()
    {
        return Mathf.NegativeInfinity;
    }


    public int GetCurrentPlayer()
    {
        return player;
    }

    public virtual Move[] GetMoves(Piece piece)
    {

        List<Move> moves = new List<Move>();
        int[] moveX = new int[] { -sizeX, sizeX};
        int moveY = -1;

        if (player == 2)
        {
            
        }
        //return piece.getPossibleActions().ToArray();  

        return new Move[0];
    }

   /* public virtual Board MakeMove(Move m)
    {
        List<Move> moves = new List<Move>(); int i; int j;
    }*/

public void Create()
    {
        mAllCells = new Cell[this.sizeX, this.sizeY];
        for (int y = 0; y < this.sizeY; y++)
        {
            for (int x = 0; x < this.sizeX; x++)
            {
                GameObject newCell = Instantiate(mCellPrefab, transform);


                //Position
                RectTransform rectTransform = newCell.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2((x * 80), (y * 80));

                //Setup

                mAllCells[x, y] = newCell.GetComponent<Cell>();
                mAllCells[x, y].Setup(new Vector2Int(x, y), this);
            }
        }
}

    public CellState ValidateCell(int targetX, int targetY, Piece checkingPiece)
    {

        //Bounds check
        if (targetX < 0 || targetX > (sizeX-1))
            return CellState.OutOfBounds;

        if (targetY < 0 || targetY > (sizeY - 1))
            return CellState.OutOfBounds;

        // Get cell
        Cell targetCell = mAllCells[targetX, targetY];

        // If the cell has a piece
        if (targetCell.mCurrentPiece != null)
        {
            //if friendly
            if (checkingPiece.mColor == targetCell.mCurrentPiece.mColor)
                return CellState.Friendly;

            if (checkingPiece.mColor != targetCell.mCurrentPiece.mColor)
                return CellState.Enemy;
        }

        return CellState.Free;


    }

}
