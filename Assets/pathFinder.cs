using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class pathFinder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void clicked()
    {
        onPieceClick();
    }
    public void onPieceClick()
    {
        string selectedButtonName = EventSystem.current.currentSelectedGameObject.name;
        Button actButton = GameObject.Find(selectedButtonName).GetComponent<Button>();
      //  board.LocateField(selectedButtonName);
    }
    public void FindPath(boardScript board)
    {
       // Debug.Log(board.board[1, 1].name);
       // Debug.Log(board.board[0,0].coordX);
       // Debug.Log(board.board[0,0].coordY);
        int smthing = board.board.Length;
        string selectedButtonName = EventSystem.current.currentSelectedGameObject.name;
        Button actButton = GameObject.Find(selectedButtonName).GetComponent<Button>();
        Vector2 test = actButton.transform.position;
        Debug.Log(selectedButtonName);
        FindPossiblePaths(selectedButtonName);
        int[] loc = LocateButton(board, selectedButtonName);

    }

    public int[] LocateButton(boardScript gameBoard, string name)
    {




        Field[,] board = gameBoard.board;
        int[] buttonLoc = new int[2];
        for (int i = 0; i < gameBoard.y; i++)
        {
            for (int j = 0; j < gameBoard.x; j++)
            {
                if (board[i, j].button == name)
                {
                    buttonLoc[0] = i;
                    buttonLoc[1] = j;
                    break;
                }
            }
        }
        Debug.Log(buttonLoc[0] + " " + buttonLoc[1]);
        return buttonLoc;
    }
    void FindPossiblePaths(string buttonName)
    {
        //search button in board
        int[] buttonPos = new int[2];
       // buttonPos = boardScript.findButton(buttonName);
        //search empty Fields around
        int found = buttonName.IndexOf("x");
        string y = buttonName.Substring(0, found);
        string x = buttonName.Substring(found, buttonName.Length);
        Debug.Log(y);
        Debug.Log(x);

    }

    void HighlightFields()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
