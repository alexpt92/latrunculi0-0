using System.Collections;
using System.Collections.Generic;
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
