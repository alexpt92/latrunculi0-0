using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{

    //public string mMove;
    //public Vector2 mTargetLoc;
    public int mScore;
    public Piece mPiece;
    //_______________________________________


    //public PieceDraught piece;
    public int x;
    public int y;
    public bool success;
    public bool threaten;
    public bool hide; //??
    public bool attacked;
    public bool attacked2;
    public int removeX;
    public int removeY;
    public int player;

     /* public Move(int score, Piece nPiece, int x, int y, int nowX, int nowY)
      {
         // this.mMove = move;
          //this.mTargetLoc = targetLoc;
          this.mScore = score;
          this.mPiece = nPiece;
        this.x = x;
        this.y = y;
        this.removeX = nowX;
        this.removeY = nowY;
      }*/
}
