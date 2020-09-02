﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{

    public Image mOutlineImage;
    public Sprite n_Sprite;
    public Sprite o_Sprite;
    public CellState state;

    [HideInInspector]
    public Vector2Int mBoardPosition = Vector2Int.zero;
    [HideInInspector]
    public Board mBoard = null;
    [HideInInspector]
    public RectTransform mRectTransform = null;
    [HideInInspector]
    public Piece mCurrentPiece = null;


    public void Setup(Vector2Int newBoardPosition, Board newBoard)
    {
        mBoardPosition = newBoardPosition;
        mBoard = newBoard;

        mRectTransform = GetComponent<RectTransform>();
    }


    public void highlight ()
    {
        this.GetComponent<Image>().sprite = n_Sprite;
    }

    public void removeHighlight()
    {
        this.GetComponent<Image>().sprite = o_Sprite;
    }

    public void RemovePiece()
    {
        if (mCurrentPiece != null)
        {
            mCurrentPiece.Kill();
        }
    }

}
