using System.Collections;
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

    protected Cell mOriginalCell = null;
    protected Cell mCurrentCell = null;

    protected RectTransform mRectTransform = null;
    protected PieceManager mPieceManager;

    protected Cell mTargetCell = null;
    protected List<Move> mMoves = new List<Move>();
   // private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


    protected Vector3Int mMovement;
    protected List<Cell> mHighlightedCells = new List<Cell>();
    public List<Move> moves = new List<Move>();





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
        CheckPathing(true);
    }


    public List<Move> getPossibleActions()
    {
        if (isDead())
            return null;
        else
        {
            CheckPathing(true);
            ClearMoves();
            if (mHighlightedCells.Count > 0)
            {
                for (int i = 0; i < mHighlightedCells.ToArray().Length; i++)
                {
                    mTargetCell = mHighlightedCells.ToArray()[i];
                    FindMoves();
                }
            }
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

    public void PlaceByAI(Cell newCell)
    {
        Color color = mColor;
        if (this.name == null)
            Debug.Log("this.name is null");
        if (mCurrentCell.name == null)
            Debug.Log("mCurrentCell.name is null");
        if (newCell.name == null)
            Debug.Log("newCell.name is null");
        Debug.Log(this.name + " bewegt sich von " + mCurrentCell.name + " nach " + newCell.name);

        mTargetCell = newCell;
        Move();
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

        int counter = moves.Count;
        Move m = new Move();
        m.mPiece = this;
        m.attacked = false;
        m.threaten = false;
        m.hide = false;
        if (mColor == Color.black)
            m.player = 2;
        else
            m.player = 1;
        if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
            && mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) == CellState.Friendly)
        {
            attackedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
            m.mScore = 10;
            m.attacked = true;
            m.x = mCurrentCell.mBoardPosition.x;
            m.y = mCurrentCell.mBoardPosition.y;
            m.removeX = targetX;
            m.removeY = targetY;
        }

        if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
            && mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) != CellState.Friendly)
        {
            //attackedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
            m.mScore = 5;
            m.threaten = true;
            m.x = mCurrentCell.mBoardPosition.x;
            m.y = mCurrentCell.mBoardPosition.y;
            m.removeX = targetX;
            m.removeY = targetY;
        }

        targetX = mTargetCell.mBoardPosition.x - 1;
        targetY = mTargetCell.mBoardPosition.y;

        allyX = mTargetCell.mBoardPosition.x - 2;
        allyY = mTargetCell.mBoardPosition.y;

        if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
&& mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) == CellState.Friendly)
        {
            attackedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
            m.mScore = 10;
            m.attacked = true;
            m.x = mCurrentCell.mBoardPosition.x;
            m.y = mCurrentCell.mBoardPosition.y;
            m.removeX = targetX;
            m.removeY = targetY;
        }

        if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
    && mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) != CellState.Friendly)
        {
            m.mScore = 5;
            m.threaten = true;
            m.x = mCurrentCell.mBoardPosition.x;
            m.y = mCurrentCell.mBoardPosition.y;
            m.removeX = targetX;
            m.removeY = targetY;
        }

        targetX = mTargetCell.mBoardPosition.x;
        targetY = mTargetCell.mBoardPosition.y + 1;

        allyX = mTargetCell.mBoardPosition.x;
        allyY = mTargetCell.mBoardPosition.y + 2;

        if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
    && mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) == CellState.Friendly)
        {
            attackedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
            m.mScore = 10;
            m.attacked = true;
            m.x = mCurrentCell.mBoardPosition.x;
            m.y = mCurrentCell.mBoardPosition.y;
            m.removeX = targetX;
            m.removeY = targetY;
        }

        if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
&& mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) != CellState.Friendly)
        {
            m.mScore = 5;
            m.threaten = true;
            m.x = mCurrentCell.mBoardPosition.x;
            m.y = mCurrentCell.mBoardPosition.y;
            m.removeX = targetX;
            m.removeY = targetY;
        }

        targetX = mTargetCell.mBoardPosition.x;
        targetY = mTargetCell.mBoardPosition.y - 1;

        allyX = mTargetCell.mBoardPosition.x;
        allyY = mTargetCell.mBoardPosition.y - 2;

        if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
    && mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) == CellState.Friendly)
        {
            attackedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
            m.mScore = 10;
            m.attacked = true;
            m.x = mCurrentCell.mBoardPosition.x;
            m.y = mCurrentCell.mBoardPosition.y;
            m.removeX = targetX;
            m.removeY = targetY;
        }
        if (mCurrentCell.mBoard.ValidateCell(targetX, targetY, this) == CellState.Enemy
&& mCurrentCell.mBoard.ValidateCell(allyX, allyY, this) != CellState.Friendly)
        {
            m.mScore = 5;
            m.threaten = true;
            m.x = mCurrentCell.mBoardPosition.x;
            m.y = mCurrentCell.mBoardPosition.y;
            m.removeX = targetX;
            m.removeY = targetY;
        }
        moves.Add(m);
    }


    public void ClearMoves()
    {
        moves = new List<Move>();
    }

    public void CreateCellPath(int xDirection, int yDirection, int movement, bool isAI)
    {
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
                break;
            }

            //get CellState
            if (cellState != CellState.Free)
                break;

            //ADD to Highlighted List
            mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
        }



    }

    protected virtual void CheckPathing(bool isAI)
    {
        //Horizontal
        if (isAI)
        {
            ClearCells();
            ClearMoves();
        }
        CreateCellPath(1, 0, mMovement.x, isAI);
        CreateCellPath(-1, 0, mMovement.x, isAI);

        //Vertical
        CreateCellPath(0, 1, mMovement.y, isAI);
        CreateCellPath(0, -1, mMovement.y, isAI);

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
            cell.removeHighlight();

        mHighlightedCells.Clear();
    }
    #endregion


    #region Events
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);

        CheckPathing(false);

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
    }
    #endregion

    public bool isDead ()
    {
        if (mCurrentCell.mCurrentPiece == null || mCurrentCell.mCurrentPiece != null && mCurrentCell.mCurrentPiece.name != this.name)
            return true;
        else

            return false;
    }


    public Cell getTargetCell()
    {
        return mTargetCell;
    }

    public Cell getCurrentCell()
    {
        return mCurrentCell;
    }
}
