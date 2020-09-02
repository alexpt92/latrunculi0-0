using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardAI
{
    public BoardAI()
    {

    }

    public static float Minimax(
        Board board,
        int player,
        int maxDepth,
        int currentDepth,
        Piece piece,
        ref Move bestMove)
    {
        if (board.IsGameOver() || currentDepth == maxDepth)
            return board.Evaluate(player);


        bestMove = null;
        float bestScore = Mathf.Infinity;
        if (board.GetCurrentPlayer() == player)
            bestScore = Mathf.NegativeInfinity;
        ///Move[] test = board.GetMoves(piece);
        foreach (Move m in board.GetMoves(piece))
        {
            Board b = board.MakeMove(m);
            float currentScore = m.mScore;
            Move currentMove = m;
            currentScore = Minimax(b, player, maxDepth, currentDepth + 1, piece, ref currentMove);
            if (board.GetCurrentPlayer() == player)
            {
                if (currentScore > bestScore)
                {
                   
                        bestScore = currentScore;
                        bestMove = currentMove;
                    

                }

            }
            else
            {
                if (currentScore < bestScore)
                {
                    bestScore = currentScore;
                    bestMove = currentMove;
                }
            }
        }
        return bestScore;
    }
}
