using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_Text currentPlayerText;
    public GameObject[] gridCells;
    public GameObject gameOverPanel;
    public TMP_Text gameOverText;
    public GameObject[] strikeThroughs; // Array of possible strike-through images

    private bool isPlayerX = true;
    private int[] gridState = new int[9];
    private int[,] winConditions = new int[,]
    {
        {0, 1, 2}, {3, 4, 5}, {6, 7, 8}, // Rows
        {0, 3, 6}, {1, 4, 7}, {2, 5, 8}, // Columns
        {0, 4, 8}, {2, 4, 6} // Diagonals
    };

    private void Start()
    {
        currentPlayerText.text = "Player X's Turn";
        gameOverPanel.SetActive(false);
        foreach (GameObject strikeThrough in strikeThroughs)
        {
            strikeThrough.SetActive(false);
        }
    }

    public void CellClicked(int cellIndex)
    {
        if (gridState[cellIndex] == 0)
        {
            gridState[cellIndex] = isPlayerX ? 1 : 2;
            gridCells[cellIndex].GetComponentInChildren<TMP_Text>().text = isPlayerX ? "X" : "O";

            if (CheckForWin())
            {
                string winner = isPlayerX ? "X" : "O";
                currentPlayerText.text = "Player " + winner + " wins!";
                gameOverPanel.SetActive(true);
                gameOverText.text = "Player " + winner + " wins!";
                int[] winningCondition = GetWinningCondition();
                ShowStrikeThrough(winningCondition);
                DisablePlayerPlacement(false);
            }
            else if (CheckForDraw())
            {
                currentPlayerText.text = "It's a draw!";
                gameOverPanel.SetActive(true);
                gameOverText.text = "It's a draw!";
                DisablePlayerPlacement(false);
            }
            else
            {
                isPlayerX = !isPlayerX;
                currentPlayerText.text = isPlayerX ? "Player X's Turn" : "Player O's Turn";
            }
        }
    }

    private bool CheckForWin()
    {
        for (int i = 0; i < 8; i++)
        {
            int a = winConditions[i, 0];
            int b = winConditions[i, 1];
            int c = winConditions[i, 2];

            if (gridState[a] != 0 && gridState[a] == gridState[b] && gridState[b] == gridState[c])
            {
                return true;
            }
        }

        return false;
    }

    private int[] GetWinningCondition()
    {
        for (int i = 0; i < 8; i++)
        {
            int a = winConditions[i, 0];
            int b = winConditions[i, 1];
            int c = winConditions[i, 2];

            if (gridState[a] != 0 && gridState[a] == gridState[b] && gridState[b] == gridState[c])
            {
                return new int[] { a, b, c };
            }
        }

        return null;
    }

    private bool CheckForDraw()
    {
        foreach (int cellState in gridState)
        {
            if (cellState == 0)
            {
                return false;
            }
        }

        return true;
    }

    public void RestartGame()
    {
        for (int i = 0; i < gridCells.Length; i++)
        {
            gridCells[i].GetComponentInChildren<TMP_Text>().text = "";
            gridState[i] = 0;
        }
        isPlayerX = true;
        currentPlayerText.text = "Player X's Turn";
        gameOverPanel.SetActive(false);
        DisablePlayerPlacement(true);
        foreach (GameObject strikeThrough in strikeThroughs)
        {
            strikeThrough.SetActive(false);
        }
    }

    void DisablePlayerPlacement(bool interactableState)
    {
        foreach (GameObject g in gridCells)
        {
            g.GetComponent<Button>().interactable = interactableState;
        }
    }

    void ShowStrikeThrough(int[] winCondition)
    {
        foreach (GameObject strikeThrough in strikeThroughs)
        {
            if (strikeThrough != null && strikeThrough.activeSelf)
            {
                strikeThrough.SetActive(false); // Disable all strike-through images first
            }
        }

        // Enable the specific strike-through image based on the win condition
        if (winCondition != null && winCondition.Length == 3 && winCondition[0] >= 0 && winCondition[1] >= 0 && winCondition[2] >= 0)
        {
            int conditionIndex = -1;
            for (int i = 0; i < winConditions.GetLength(0); i++)
            {
                if (winConditions[i, 0] == winCondition[0] && winConditions[i, 1] == winCondition[1] && winConditions[i, 2] == winCondition[2])
                {
                    conditionIndex = i;
                    break;
                }
            }

            if (conditionIndex >= 0 && conditionIndex < strikeThroughs.Length)
            {
                GameObject strikeThroughToDisplay = strikeThroughs[conditionIndex];
                if (strikeThroughToDisplay != null)
                {
                    strikeThroughToDisplay.SetActive(true);
                }
            }
        }
    }
}
