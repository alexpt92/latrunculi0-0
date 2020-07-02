using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public boardScript board;

    public int x { get; set; }//Brettgröße X & Y
    public int y { get; set; }
    public float xCoord;
    public float yCoord;
    public int sizeX;
    public int sizeY;
    public pathFinder pathFinder;
    public Sprite o_Sprite;
    string lastSelectedButtonName;

    public Sprite m_Sprite;

    void Start()
    {
        board = new boardScript(sizeX, sizeY, xCoord, yCoord, m_Sprite, o_Sprite);
        pathFinder = new pathFinder();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clicked()
    {
        string selectedName = EventSystem.current.currentSelectedGameObject.name;
        if (selectedName.Contains("Button")) {
            Button actButton = GameObject.Find(selectedName).GetComponent<Button>();
            board.LocateField(selectedName);
            lastSelectedButtonName = selectedName;
           // board.HighlightFields();
        }

    else
        {
            //String[] seperator = { "x" };
            //String[] fieldPos = selectedName.Split(seperator);
            Image clickedField = GameObject.Find(selectedName).GetComponent<Image>();
            board.moveButton(lastSelectedButtonName, clickedField);
            board.checkAttack(lastSelectedButtonName);
        }
    }
}
