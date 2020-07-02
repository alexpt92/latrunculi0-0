using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class ChangeImage : MonoBehaviour
{
    public Button actButton;
    private string prevButtonName;
    public Sprite white;
    public Sprite red;
    private int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        actButton = GetComponent<Button>();
    }

    // Update is called once per frame
    public void changeButton()
    {
        Debug.Log("iwas");
        counter++;
        string selectedButtonName = EventSystem.current.currentSelectedGameObject.name;
        actButton = GameObject.Find(selectedButtonName).GetComponent<Button>();
        if (selectedButtonName != prevButtonName && prevButtonName != null)
        {
            Button prevButton = GameObject.Find(prevButtonName).GetComponent<Button>();
            prevButton.image.overrideSprite = white;
            counter = 1;
        }
        Debug.Log(actButton.image.sprite.name);
        Debug.Log(red.name);
        

        if (counter % 2 == 0)
        {
            actButton.image.overrideSprite = white;
        }
        else
        {
            actButton.image.overrideSprite = red;
        }
        prevButtonName = selectedButtonName;
        Debug.Log(selectedButtonName);

    }
}

