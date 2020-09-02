using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardDraughts : Board
{

    public int size = 8; 
    public int numPieces = 12; 
    public GameObject prefab;
    protected Piece[,] board;


    private void Awake()
    {
        {
            board = new Piece[size, size];
            
        }
    }




    // Start is called before the first frame update
    void Start()
    {
        //TODO: Initalization and board set up


        Piece pd = prefab.GetComponent<Piece>();
        int piecesLeft = numPieces; 
        for (int i = 0; i < size; i++) 
        { 
            if (piecesLeft == 0) break; 
            int init = 0; 
            if (i % 2 != 0) init = 1; 
            for (int j = init; j < size; j += 2) 
            { 
                if (piecesLeft == 0) break; 
                PlacePiece(j, i); 
                piecesLeft--; 
            } 
        }

        piecesLeft = numPieces; 
        for (int i = size - 1; i >= 0; i--) 
        { 
            if (piecesLeft == 0) break; 
            int init = 0; 
            if (i % 2 != 0) 
                init = 1; 
            for (int j = init; j < size; j += 2) 
            { 
                if (piecesLeft == 0) break;
                
                PlacePiece(j, i); 
                piecesLeft--; 
            } 
        }
    }

    public override float Evaluate()
    {
        Color color = Color.white;
        if (player == 1)
            color = Color.black;
        return Evaluate(color);
    }

    public override float Evaluate(int player)
    {
        Color color = Color.white;
        if (player == 1)
            color = Color.black;
        return Evaluate(color);
    }

   /* private float Evaluate(Color color)
    {
        float eval = 1f;
        float pointSimple = 1f;
        float pointSuccess = 5f;
        int rows = board.GetLength(0);
        int cols = board.GetLength(1);

        int i;
        int j;

        for (i =0; i< rows; i++)
        {
            for (j=0; j<cols; j++)
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
        return eval;
    }*/


   /* public override Move[] GetMoves()
    {
        List<Move> moves = new List<Move>();
        int rows = board.GetLength(0);
        int cols = board.GetLength(1);
        int i;
        int j;
        for (i = 0; i < rows; i++)
        {
            for (j = 0; i < cols; j++)
            {
                Piece p = board[i, j];
                if (p == null)
                    continue;
                p.GetMovesMan(ref board);
                moves.AddRange(p.GetMoveList().ToArray());
            }
        }
        return moves.ToArray();
    }*/



    private void PlacePiece(int x, int y)
    {
        Vector3 pos = new Vector3();
        pos.x = (float)x;
        pos.y = -(float)y;


        //Place(pos);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
