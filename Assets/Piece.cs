﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Piece : EventTrigger
{

    [HideInInspector]
    public Color mColor = Color.clear;
    public Sprite n_Sprite;
    public Sprite o_Sprite;

    private int sizeX;
    private int sizeY;

    protected Cell mOriginalCell = null;
    protected Cell mCurrentCell = null;

    protected RectTransform mRectTransform = null;
    protected PieceManager mPieceManager;

    protected Cell mTargetCell = null;
    protected List<Move> mMoves = new List<Move>();
  //  private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


    protected Vector3Int mMovement;// = new Vector3Int(sizeX, sizeY, 1);
    protected List<Cell> mHighlightedCells = new List<Cell>();
    public List<Move> moves = new List<Move>();
   // protected bool isMinMax = false;





    public virtual void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager, Vector3Int movement)
    {
        mPieceManager = newPieceManager;

        mColor = newTeamColor;
        GetComponent<Image>().color = newSpriteColor;
        mRectTransform = GetComponent<RectTransform>();
        mMovement = movement;
    }

    public Move[] GetMoveArray ()
    {
        return this.mPieceManager.getBoard().GetMoves(this);
    }

    public void GetMovesMan(ref Piece[,] board)
    {
        //List<Move> moves = new List<Move>();
        //isMinMax = true;
        CheckPathing();

        //isMinMax = false;
       // return moves;
    }


    public List<Move> getPossibleActions()
    {
        if (isDead())
            return null;
        else
        {
            CheckPathing();
            //List<string>
            for (int i = 0; i < mHighlightedCells.ToArray().Length - 1; i++)
            {
                mTargetCell = mHighlightedCells.ToArray()[i];
                FindMoves();
                //mMoves.Add(new Move(mHighlightedCells[0].mBoardPosition, 1, this));
            }
            //this.Move();

            return mMoves;
        }
    }


    public void Place(Cell newCell)
    {



        //Cell
        mCurrentCell = newCell;
        mOriginalCell = newCell;
        mCurrentCell.mCurrentPiece = this;

        //Object
        transform.localPosition = mCurrentCell.transform.localPosition;
        
        gameObject.SetActive(true);
    }

    public void PlaceByAI(Cell pos)
    {
        Color color = mColor;
        Cell[] attackedCells = checkNeighbor();
        if (attackedCells != null)
        {
            for (int i = 0; i <= attackedCells.Length - 1; i++)
            {
                if (attackedCells[i] != null)
                {
                    attackedCells[i].RemovePiece();
                }
            }
        }
        if (attackedCells != null)
        {
            if (color == Color.white)
                color = Color.black;
            else
                color = Color.white;
        }
        //Cell

        Debug.Log("original Position: " + transform.localPosition + " this.Piece: " + this + "OriginalCell.mCurrentPiece: " + mCurrentCell.mCurrentPiece + " newPos: " + pos.transform.localPosition);
        transform.localPosition = pos.transform.localPosition;
        mCurrentCell.mCurrentPiece = null;
        mCurrentCell = pos;// mPieceManager.GetCellByPos(pos);
        mCurrentCell.mCurrentPiece = this;
        Debug.Log("original Position: " + pos.transform.localPosition + " this.Piece: " + this + "OriginalCell.mCurrentPiece: " + mCurrentCell.mCurrentPiece);

        //Object
        transform.localPosition = pos.transform.localPosition;

        gameObject.SetActive(true);
    }

    public void Reset()
    {
        Kill();

        Place(mOriginalCell);

    }

    public virtual void Kill()
    {
        //Clear Cell
        mCurrentCell.mCurrentPiece = null;

        //Remove Piece
        gameObject.SetActive(false);

    }

    #region movement



    protected virtual void Move()
    {
        // if enemy piece -> back to currentCell
        if (mTargetCell.mCurrentPiece != null)
        {
            transform.position = mCurrentCell.transform.position;
            mTargetCell = null;

            return;
        }

        Color color = mColor;

        //check neighbors for Enemys & Allys
        Cell[] attackedCells = checkNeighbor();
        if (attackedCells != null) {
            for (int i = 0; i <= attackedCells.Length - 1; i++)
            {
                if (attackedCells[i] != null)
                {
                    attackedCells[i].RemovePiece();
                }
            }
        }
        if (attackedCells != null)
        {
            if (color == Color.white)
                color = Color.black;
            else
                color = Color.white;
        }

        mCurrentCell.mCurrentPiece = null;

        mCurrentCell = mTargetCell;
        mCurrentCell.mCurrentPiece = this;

        transform.position = mCurrentCell.transform.position;
        mTargetCell = null;
        mPieceManager.SwitchSides(color);

    }

    private Cell[] checkNeighbor()
    {
        int currentX = mTargetCell.mBoardPosition.x;
        int currentY = mTargetCell.mBoardPosition.y;

        int targetX = mTargetCell.mBoardPosition.x + 1;
        int targetY = mTargetCell.mBoardPosition.y;

        int allyX = mTargetCell.mBoardPosition.x + 2;
        int allyY = mTargetCell.mBoardPosition.y;

        List<Cell> attackedCells = new List<Cell>();


        int counter = 0;



            if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
                && mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) == CellState.Friendly)
            {
                counter++;
                attackedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
            }

            targetX = mTargetCell.mBoardPosition.x - 1;
            targetY = mTargetCell.mBoardPosition.y;

            allyX = mTargetCell.mBoardPosition.x - 2;
            allyY = mTargetCell.mBoardPosition.y;

            if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
        && mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) == CellState.Friendly)
            {
                counter++;
                attackedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
            }

            targetX = mTargetCell.mBoardPosition.x;
            targetY = mTargetCell.mBoardPosition.y + 1;

            allyX = mTargetCell.mBoardPosition.x;
            allyY = mTargetCell.mBoardPosition.y + 2;

            if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
        && mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) == CellState.Friendly)
            {
                counter++;
                attackedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
            }

            targetX = mTargetCell.mBoardPosition.x;
            targetY = mTargetCell.mBoardPosition.y - 1;

            allyX = mTargetCell.mBoardPosition.x;
            allyY = mTargetCell.mBoardPosition.y - 2;

            if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
        && mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) == CellState.Friendly)
            {
                counter++;
                attackedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
            }

        

        //CHECK FOR AI starts HERE

            if (attackedCells.ToArray().Length > 0)
                return attackedCells.ToArray();
            else
                return null;
        

        //TODO: erweitern auf "BEDROHUNG" prüfen!!!

    }


    public void FindMoves()
    {
        
            int currentX = mTargetCell.mBoardPosition.x;
            int currentY = mTargetCell.mBoardPosition.y;

            int targetX = mTargetCell.mBoardPosition.x + 1;
            int targetY = mTargetCell.mBoardPosition.y;

            int allyX = mTargetCell.mBoardPosition.x + 2;
            int allyY = mTargetCell.mBoardPosition.y;

            List<Cell> attackedCells = new List<Cell>();


        Move m = new Move();
            if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
                && mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) == CellState.Friendly)
            {
                attackedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
                m.mPiece = this;
                m.mScore = 10;
                m.attacked = true;
                m.threaten = false;
                m.hide = false;
                m.x = mCurrentCell.mBoardPosition.x;
                m.y = mCurrentCell.mBoardPosition.y;
                m.removeX = targetX;
                m.removeY = targetY;
                if (mColor == Color.black)
                    m.player = 2;
                else
                    m.player = 1;
                moves.Add(m);
                //??                m.mPiece = this;
            }

            if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
                && mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) != CellState.Friendly)
            {
                attackedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
                m.mPiece = this;
                m.mScore = 5;
                m.attacked = false;
                m.threaten = true;
                m.hide = false;
                m.x = mCurrentCell.mBoardPosition.x;
                m.y = mCurrentCell.mBoardPosition.y;
                m.removeX = targetX;
                m.removeY = targetY;
                if (mColor == Color.black)
                    m.player = 2;
                else
                    m.player = 1;
                moves.Add(m);
            }

            targetX = mTargetCell.mBoardPosition.x - 1;
            targetY = mTargetCell.mBoardPosition.y;

            allyX = mTargetCell.mBoardPosition.x - 2;
            allyY = mTargetCell.mBoardPosition.y;

            if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
&& mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) == CellState.Friendly)
            {
                attackedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
                m.mPiece = this;
                m.mScore = 10;
                m.attacked = true;
                m.threaten = false;
                m.hide = false;
                m.x = mCurrentCell.mBoardPosition.x;
                m.y = mCurrentCell.mBoardPosition.y;
                m.removeX = targetX;
                m.removeY = targetY;
                if (mColor == Color.black)
                    m.player = 2;
                else
                    m.player = 1;
                moves.Add(m);
            }

            if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
        && mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) != CellState.Friendly)
            {
                attackedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
                m.mPiece = this;
                m.mScore = 5;
                m.attacked = false;
                m.threaten = true;
                m.hide = false;
                m.x = mCurrentCell.mBoardPosition.x;
                m.y = mCurrentCell.mBoardPosition.y;
                m.removeX = targetX;
                m.removeY = targetY;
                if (mColor == Color.black)
                    m.player = 2;
                else
                    m.player = 1;
                moves.Add(m);
            }

            targetX = mTargetCell.mBoardPosition.x;
            targetY = mTargetCell.mBoardPosition.y + 1;

            allyX = mTargetCell.mBoardPosition.x;
            allyY = mTargetCell.mBoardPosition.y + 2;

            if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
        && mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) == CellState.Friendly)
            {
                attackedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
                m.mPiece = this;
                m.mScore = 10;
                m.attacked = true;
                m.threaten = false;
                m.hide = false;
                m.x = mCurrentCell.mBoardPosition.x;
                m.y = mCurrentCell.mBoardPosition.y;
                m.removeX = targetX;
                m.removeY = targetY;
                if (mColor == Color.black)
                    m.player = 2;
                else
                    m.player = 1;
                moves.Add(m);
            }

            if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
&& mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) != CellState.Friendly)
            {
                attackedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
                m.mPiece = this;
                m.mScore = 5;
                m.attacked = false;
                m.threaten = true;
                m.hide = false;
                m.x = mCurrentCell.mBoardPosition.x;
                m.y = mCurrentCell.mBoardPosition.y;
                m.removeX = targetX;
                m.removeY = targetY;
                if (mColor == Color.black)
                    m.player = 2;
                else
                    m.player = 1;
                moves.Add(m);
            }

            targetX = mTargetCell.mBoardPosition.x;
            targetY = mTargetCell.mBoardPosition.y - 1;

            allyX = mTargetCell.mBoardPosition.x;
            allyY = mTargetCell.mBoardPosition.y - 2;

            if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
        && mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) == CellState.Friendly)
            {
                attackedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
                m.mPiece = this;
                m.mScore = 10;
                m.attacked = true;
                m.threaten = false;
                m.hide = false;
                m.x = mCurrentCell.mBoardPosition.x;
                m.y = mCurrentCell.mBoardPosition.y;
                m.removeX = targetX;
                m.removeY = targetY;
                if (mColor == Color.black)
                    m.player = 2;
                else
                    m.player = 1;
                moves.Add(m);
            }
            if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
&& mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) != CellState.Friendly)
            {
                m.mPiece = this;
                m.mScore = 5;
                m.attacked = false;
                m.threaten = true;
                m.hide = false;
                m.x = mCurrentCell.mBoardPosition.x;
                m.y = mCurrentCell.mBoardPosition.y;
                m.removeX = targetX;
                m.removeY = targetY;
                if (mColor == Color.black)
                    m.player = 2;
                else
                    m.player = 1;
                moves.Add(m);
            }
            
        if (attackedCells.Count > 1)
            Debug.Log(attackedCells.ToArray()[0] + " " + attackedCells.ToArray()[1]);
        else if (attackedCells.Count > 0)
            Debug.Log(attackedCells.ToArray()[0].mBoardPosition);

        Debug.Log("iwas");
        }

    public void clearMoves()
    {
        moves = new List<Move>();
    }

    public void CreateCellPath(int xDirection, int yDirection, int movement,bool isMinMax)
    {
        if (isMinMax)
            clearMoves();
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;
        Move m = new Move();

        for (int i = 1; i<= movement; i++)
        {
            currentX += xDirection;
            currentY += yDirection;

            CellState cellState = CellState.None;
            cellState = mCurrentCell.mBoard.ValidateCell(currentX, currentY, this);

            //If enemy, break;
            if (cellState == CellState.Enemy)
            {
                if (isMinMax && moves.ToArray().Length > 0)
                {
                    moves.RemoveAt(moves.LastIndexOf(m));
                    m.mScore = 2;
                    moves.Add(m);
                }
                break;
            }

            //get CellState
            if (cellState != CellState.Free)
                break;

            //ADD to Highlighted List
            if (isMinMax)
            {
                // Move m = new Move(0, this, mCurrentCell.mBoardPosition.x, mCurrentCell.mBoardPosition.y, mCurrentCell.mBoard.mAllCells[currentX, currentY].mBoardPosition.x, mCurrentCell.mBoard.mAllCells[currentX, currentY].mBoardPosition.y);
                m = new Move();
                m.mPiece = this;
                m.mScore = 0;
                m.x = mCurrentCell.mBoard.mAllCells[currentX, currentY].mBoardPosition.x;
                m.y = mCurrentCell.mBoard.mAllCells[currentX, currentY].mBoardPosition.y;
                //   m.removeX = mCurrentCell.mBoardPosition.x;
                //   m.removeY = mCurrentCell.mBoardPosition.y;
                moves.Add(m);
                

            }
            else
            mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);




        }



    }

    protected virtual void CheckPathing()
    {
        //Horizontal
        CreateCellPath(1, 0, mMovement.x, false);
        CreateCellPath(-1, 0, mMovement.x, false);

        //Vertical
        CreateCellPath(0, 1, mMovement.y, false);
        CreateCellPath(0, -1, mMovement.y, false);

      /*  //Upper diagonal
         CreateCellPath(1, 1, mMovement.z);
         CreateCellPath(-1, 1, mMovement.z);

         //Lower diagonal
         CreateCellPath(-1, -1, mMovement.z);
         CreateCellPath(1, -1, mMovement.z);*/
    }


    protected void ShowCells()
    {
        foreach (Cell cell in mHighlightedCells)
            cell.highlight();

    }

    protected void ClearCells()
    {
        foreach (Cell cell in mHighlightedCells)
            //cell.mOutlineImage.enabled = false;
            cell.removeHighlight();

        mHighlightedCells.Clear();
    }
    #endregion


    #region Events
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);

        CheckPathing();

        ShowCells();
    }


    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        //follow cursor Position
        transform.position += (Vector3)eventData.delta;


        foreach (Cell cell in mHighlightedCells)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(cell.mRectTransform, Input.mousePosition))
            {
                // if mouse is in a valid cell -> get it
                mTargetCell = cell;
                break;
            }
            //if mouse is not in highlighted cell -> no valid move
            mTargetCell = null;
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        ClearCells();

        if (!mTargetCell)
        {
            transform.position = mCurrentCell.gameObject.transform.position;
            return;
        }
        Move();
        //NextPlayer();
        //mPieceManager.SwitchSides(mColor);
    }
    #endregion



    public bool isDead ()
    {
        if (mCurrentCell.mCurrentPiece == null)
            return true;
        else

            return false;
    }


    public Cell getTargetCell()
    {
        return mTargetCell;
    }


}
