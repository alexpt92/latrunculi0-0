using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{

    Dictionary<Color, List<Piece>> pieceLists;
    string aiType;


    // Start is called before the first frame update
    void Start()
    {
        
    }


    public virtual void Setup (string newAIType)
    {
        if (newAIType == "minimax")
        {

        }
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
        /* Move[][] allMoves = new Move[board.sizeX][];
         for (int i = 0; i < board.sizeX; i++)
             allMoves[i] = board.GetMoves();*/
        foreach (Move m in board.GetMoves(piece))
        {
            Board b = board.MakeMove(m);
            float currentScore;
            Move currentMove = null;//m; ??
            currentScore = Minimax(b, player, maxDepth, currentDepth + 1, piece, ref currentMove);
            if (board.GetCurrentPlayer() == player)
            {
                if (currentScore > bestScore)
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
        }
        return bestScore;
    }

}
