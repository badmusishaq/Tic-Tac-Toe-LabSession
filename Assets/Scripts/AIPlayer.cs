using UnityEngine;
using System.Collections.Generic;

public class AIPlayer : MonoBehaviour
{
    private NewGameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<NewGameManager>();
        Debug.Log(gameManager.name);
    }

    public void MakeMove()
    {
        int[] gridState = gameManager.GetGridState();
        List<int> availableMoves = new List<int>();

        for (int i = 0; i < gridState.Length; i++)
        {
            if (gridState[i] == 0)
            {
                availableMoves.Add(i);
            }
        }

        if (availableMoves.Count > 0)
        {
            int botMove = GetBestMove(gridState);
            gameManager.CellClicked(botMove);
            gameManager.CheckForWinAndDisplay(); // Check for win and display strike-through after AI makes a move
        }
    }

    private int GetBestMove(int[] board)
    {
        int bestScore = int.MinValue;
        int move = -1;

        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == 0)
            {
                board[i] = 2;
                int score = Minimax(board, 0, false);
                board[i] = 0;

                if (score > bestScore)
                {
                    bestScore = score;
                    move = i;
                }
            }
        }

        return move;
    }

    private int Minimax(int[] board, int depth, bool isMaximizing)
    {
        if (gameManager.CheckForWin())
        {
            return isMaximizing ? -1 : 1;
        }
        else if (gameManager.CheckForDraw())
        {
            return 0;
        }

        int bestScore = isMaximizing ? int.MinValue : int.MaxValue;

        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == 0)
            {
                int marker = isMaximizing ? 2 : 1;
                board[i] = marker;
                int score = Minimax(board, depth + 1, !isMaximizing);
                board[i] = 0;

                if (isMaximizing)
                {
                    bestScore = Mathf.Max(bestScore, score);
                }
                else
                {
                    bestScore = Mathf.Min(bestScore, score);
                }
            }
        }

        return bestScore;
    }
}
