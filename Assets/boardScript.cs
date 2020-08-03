using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class boardScript : MonoBehaviour
{
    public Field[,] board;
    public int x { get; set; }//Brettgröße X & Y
    public int y { get; set; }
    public float xCoord;
    public float yCoord;
    public List<Button> wList;
    public List<Button> bList;
    public List<Image> highlightedFields;
    public Sprite m_Sprite;
    public Sprite o_Sprite;
    public bool moveAgain = false;
    public string winner = null;


    #region runtime Creation
    public GameObject fieldPrefab;

    [HideInInspector]
   // public Field[,] allFields;

   /* public void Create()
    {
        board = new Field[this.x, this.y];

        for (int y = 0; y < this.y; y++) {

            for (int x = 0; x < this.x; x++)
            {
                //create the Field
                GameObject newCell = Instantiate(fieldPrefab, transform);

                //Position
                RectTransform rectTransform = newCell.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2((x * 100) + 50, (y * 100) + 50);

                //Setup
                board[x, y] = newCell.GetComponent<Field>();
                board[x, y].Setup(new Vector2Int(x, y), this);
            }
        }
    }
   */
    #endregion

    // Start is called before the first frame update
    public boardScript(int newX, int newY, float newXCoord, float newYCoord, Sprite newSprite, Sprite oldSprite)
    {   // Constructor Board
        x = newX;
        y = newY;
        xCoord = newXCoord;
        yCoord = newYCoord;
        wList = new List<Button>();
        bList = new List<Button>();
        m_Sprite = newSprite;
        o_Sprite = oldSprite;
        //Create();
        initializeBoard();
        initializePieces();



    }

    void Update()
    {

    }

    void initializePieces()
    {   //Initializes Pieces
        for (int i = 1; i <= ((x * 2)); i++)
        {
            string name = "wButton" + i.ToString();
            Debug.Log(name);
            Button actButton = GameObject.Find(name).GetComponent<Button>();
            wList.Add(actButton);
            actButton = GameObject.Find("bButton" + i.ToString()).GetComponent<Button>();
            bList.Add(actButton);
        }
    }


    void initializeBoard()
    {   //initializes Board
        board = new Field[y, x];
        int wCounter = 0;
        int bCounter = 0;
        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                if (i < 2)
                {
                    wCounter++;

                    board[i, j] = new Field("wButton" + wCounter, (i) + "x" + (j), GameObject.Find((i) + "x" + (j)).GetComponent<Image>().transform.position.y, GameObject.Find((i) + "x" + (j)).GetComponent<Image>().transform.position.x);
                }
                else if (i > (x - 3))
                {
                    bCounter++;
                    board[i, j] = new Field("bButton" + bCounter, (i) + "x" + (j), GameObject.Find((i) + "x" + (j)).GetComponent<Image>().transform.position.y, GameObject.Find((i) + "x" + (j)).GetComponent<Image>().transform.position.x);

                }
                else
                    board[i, j] = new Field(null, (i) + "x" + (j), GameObject.Find((i) + "x" + (j)).GetComponent<Image>().transform.position.y, GameObject.Find((i) + "x" + (j)).GetComponent<Image>().transform.position.x);
            }
        }
    }
  
    public void drawBoard()
    {

    }

    public void CleanHighlights()
    {   //clears Highlighted Fields
        if (highlightedFields != null) 
        { 
            Image[] possibleFields = highlightedFields.ToArray();

            for (int i = 0; i < possibleFields.Length; i++)
            {
                possibleFields[i].sprite = o_Sprite;
            }
        }
    }

    public void findPossiblePaths(int[]pieceField)
    {   //Searching for possible Paths
        CleanHighlights();
        highlightedFields = new List<Image>();

        for (int i = 1; i<=pieceField[0]; i ++)
        {   //suche an der x-Achse
            if (board[pieceField[0] - i, pieceField[1]].button == null)
                highlightedFields.Add(GameObject.Find((pieceField[0] - i) + "x" + (pieceField[1])).GetComponent<Image>());
            else
                break;
        }

        for (int i = pieceField[0] + 1; i< this.y; i++)
        {
            //suche an der x-Achse
            if (board[i, pieceField[1]].button == null)
                highlightedFields.Add(GameObject.Find((i) + "x" + (pieceField[1])).GetComponent<Image>());
            else
                break;
        }

        for (int i=1; i<= pieceField[1]; i++)
        {
            //suche an der y-Achse
            if (board[pieceField[0], pieceField[1] - i].button == null)
                highlightedFields.Add(GameObject.Find((pieceField[0]) + "x" + (pieceField[1] - i)).GetComponent<Image>());
            else
                break;
        }

        for (int i=pieceField[1] + 1; i< this.x; i++)
        {
            //suche an der y-Achse
            if (board[pieceField[0], i].button == null)
                highlightedFields.Add(GameObject.Find((pieceField[0]) + "x" + (i)).GetComponent<Image>());
            else
                break;

        }

     
    }

    public void HighlightFields(int[] pieceField)
    {   // Highlight Possible Paths
        findPossiblePaths(pieceField);

        Image[] possibleFields = highlightedFields.ToArray();

        for (int i = 0; i < possibleFields.Length; i++)
        {
            possibleFields[i].sprite = m_Sprite;
        }
    }

    public void LocateField(string pieceName)
    {   //Locate Field of clicked Piece
        Button actButton = GameObject.Find(pieceName).GetComponent<Button>();
        float buttonX = 0;
        float buttonY = 0;
        int listIdx = 0;

        if (pieceName.Contains("wButton"))
        {
            Button[] tempArray = wList.ToArray();
            listIdx = Array.IndexOf(tempArray, actButton);
            buttonX = tempArray[listIdx].transform.position.x;
            buttonY = tempArray[listIdx].transform.position.y;
            //actButton.transform.position = new Vector3(actButton.transform.position.x, actButton.transform.position.y, 0);
        }
        else if (pieceName.Contains("bButton"))
        {
            Button[] tempArray = bList.ToArray();
            listIdx = Array.IndexOf(tempArray, actButton);

            //listIdx = bList.IndexOf(actButton);
            buttonX = tempArray[listIdx].transform.position.x;
            buttonY = tempArray[listIdx].transform.position.y;

        }


        int[] pieceField = new int[2];
        for (int i = 0; i < y; i++)
        {
            
            for (int j = 0; j < x; j++)
            {
                if (board[i,j].coordX == buttonX && board[i,j].coordY == buttonY) 
                {
                    pieceField[0] = i;
                    pieceField[1] = j;
                    break;
                }

            }
        }
        HighlightFields(pieceField);
        Debug.Log(pieceField[1] + " und " + pieceField[0]);
        Debug.Log(board[pieceField[0], pieceField[1]].coordX + " und " + board[pieceField[0], pieceField[1]].coordY);
        Debug.Log(wList[listIdx].transform.position.x + " und " + wList[listIdx].transform.position.y);

    }

    public void checkAttack(string lastSelection)
    {   //check if attack on Piece
        Button actButton = GameObject.Find(lastSelection).GetComponent<Button>();
        Vector2 buttonPos = actButton.transform.position;
        Vector2 boardPos = new Vector2(-1,-1);
        for (int i = 0; i < this.y; i++)
        {
            for (int j = 0; j < this.x; j++)
            {
                if (board[i, j].button == lastSelection)
                {
                    Button[] wButtons = wList.ToArray();
                    Button[] bButtons = bList.ToArray();
                    int wPos = Array.IndexOf(wButtons, actButton);
                    int bPos = Array.IndexOf(bButtons, actButton);
                    if (wPos != -1)
                    {
                        //buttonPos = new Vector2(actButton.transform.position.y, actButton.transform.position.x);
                        boardPos = getFieldPos(buttonPos);
                        checkNeighbors(boardPos);

                    }
                    if (bPos != -1)
                    {
                        boardPos = getFieldPos(buttonPos);
                        checkNeighbors(boardPos);
                    }


                }
            }
        }


    }

    void checkNeighbors(Vector2 boardPos)
    {   // Checking neighbors for removing
        int i = (int) boardPos.x;
        int j = (int)(boardPos.y);
        string attackButton = "wButton";
        string defenseButton = "bButton";
        if (board[i, j].button != null)
        {
            if (board[i, j].button.Contains("bButton"))
            {
                attackButton = "bButton";
                defenseButton = "wButton";
            }
        }
        if (i > 1) 
        {
            if (board[i - 2, j].button != null && board[i - 1,j ].button != null)
            {
                if (board[i - 2, j].button.Contains(attackButton))
                {
                    if (board[i - 1, j].button.Contains(defenseButton))
                    {
                        removeButton(new Vector2(i - 1, j));
                        moveAgain = true;
                        return;
                    }

                }
            }
        }

        if (i<(this.y-2))
        {
            if (board[i + 2, j].button != null && board[i + 1, j].button != null)
            {
                if (board[i + 2, j].button.Contains(attackButton))
                {
                    if (board[i + 1, j].button.Contains(defenseButton))
                    {
                        removeButton(new Vector2(i + 1, j));
                        moveAgain = true;
                        return;
                    }
                }
            }
        }

        if (j > 1 && (board[i, j-2].button != null && board[i, j-1].button != null))
        {
            if (board[i, j-2].button.Contains(attackButton))
            {
                if (board[i, j - 1].button.Contains(defenseButton))
                {
                    removeButton(new Vector2(i, j - 1));
                    moveAgain = true;
                    return;
                }
            }
        }

        if ((j < (this.x - 2)  && board[i, j+1].button != null && board[i, j + 2].button != null))
        {
            if (board[i, j+2].button.Contains(attackButton))
            {
                if (board[i, j+1].button.Contains(defenseButton))
                {
                    removeButton(new Vector2(i, j+1));
                    moveAgain = true;
                    return;
                }
            }
        }
        moveAgain = false;
    }

    void removeButton(Vector2 pos)
    {
        // removes attacked Piece
        Button attackedButton = GameObject.Find(board[(int)pos.x, (int)pos.y].button).GetComponent<Button>();
        string buttonName = board[(int)pos.x, (int)pos.y].button;
        Button[] wButtons = wList.ToArray();
        Button[] bButtons = bList.ToArray();
        int wPos = Array.IndexOf(wButtons, attackedButton);
        int bPos = Array.IndexOf(bButtons, attackedButton);
        if (wPos != -1)
        {
            wList.Remove(attackedButton);
        }
        else if (bPos != -1)
        {
            bList.Remove(attackedButton);

        }
        board[(int)pos.x, (int)pos.y].button = null;
       
        Debug.Log("removed" + pos.y + pos.x);
        Destroy(attackedButton.gameObject);
    }

    Vector2 getFieldPos(Vector2 buttonPos)
    {   //returns new Position as Vector2
        for (int i = 0; i < this.y; i++)
        {
            for (int j = 0; j < this.x; j++)
            {
                if (buttonPos.x == board[i, j].coordX && buttonPos.y == board[i, j].coordY)
                {

                    return new Vector2(i, j);
                    
                }
            }
        }
        return new Vector2(-1, -1);

    }
    public void moveButton(string lastSelection, Image clickedField)
    {   // moves Piece
        Button actButton = GameObject.Find(lastSelection).GetComponent<Button>();
        string y = clickedField.name.Substring(0, 1);
        string x = clickedField.name.Substring(clickedField.name.IndexOf("x")+1, 1);
        int newX;
        int newY;
        if (Int32.TryParse(x, out newX))
        {
            newX = Int32.Parse(x);
        }
        if (Int32.TryParse(y, out newY))
        {
            newY = Int32.Parse(y);
        }
        actButton = GameObject.Find(lastSelection).GetComponent<Button>();
        Image[] possibleFields = highlightedFields.ToArray();
        int pos = Array.IndexOf(possibleFields, clickedField);
        if (pos > -1)
        {
            Vector3 newPosition = new Vector3(board[newY, newX].coordX, board[newY, newX].coordY, actButton.transform.position.z);
            actButton.transform.position = newPosition;
            //actButton.transform.

            for (int i = 0; i < this.y; i++)
            {
                for (int j = 0; j < this.x; j++)
                {
                    if (board[i, j].button == lastSelection)
                    {
                        board[i, j].button = null;
                    } 
                } 
            }
            board[newY, newX].button = lastSelection;

        }
        CleanHighlights();

    }
   /* public void onPieceClick() //outdated
    {   
        string selectedButtonName = EventSystem.current.currentSelectedGameObject.name;
        Button actButton = GameObject.Find(selectedButtonName).GetComponent<Button>();
        LocateField(selectedButtonName);
    }
   */
    public string checkForWin()
    {
        Button[] bArray = bList.ToArray();
        Button[] wArray = wList.ToArray();

        if (bArray.Length<2)
        {
            winner = "white";
            disablePieces();
        }
        else if (wArray.Length<2)
        {
            winner = "black";
            disablePieces();

        }
        return winner;
    }

    public void disablePieces()
    {
        Button[] bArray = bList.ToArray();
        Button[] wArray = wList.ToArray();
        Button actButton;
        for (int i = 0; i < bArray.Length; i++)
        {
            actButton = bArray[i];
            actButton.interactable = false;
        }
        for (int i = 0; i < wArray.Length; i++)
        {
            actButton = wArray[i];

            actButton.interactable = false;
        }
    }
    // Update is called once per frame

}
