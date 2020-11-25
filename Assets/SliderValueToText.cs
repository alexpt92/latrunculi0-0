using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueToText : MonoBehaviour
{
    public Slider sliderUI;
    private Text textSliderValue;
    // Start is called before the first frame update
    void Start()
    {
        textSliderValue = GetComponent<Text>();
        ShowSliderValue();
    }

    public void ShowSliderValue()
    {
        string sliderMessage = "Slider value = " + sliderUI.value;
        textSliderValue.text = sliderMessage;
    }
    public void ChangeAttackPoints()
    {
        GameObject referenceObject;
        GameManager referenceScript;
        referenceObject = GameObject.FindGameObjectWithTag("GameManager");

        referenceScript = referenceObject.GetComponent<GameManager>();
        referenceScript.ChangeAttackPoints(sliderUI.value);

    }

    public void ChangeHidePoints()
    {
        GameObject referenceObject;
        GameManager referenceScript;
        referenceObject = GameObject.FindGameObjectWithTag("GameManager");

       // referenceObject = GameObject.FindGameObjectWithTag("BoardCanvas");
        referenceScript = referenceObject.GetComponent<GameManager>();
        // referenceScript.pointHide = sliderUI.value;
        referenceScript.ChangeHidePoints(sliderUI.value);
    }

    public void ChangeThreatPoints()
    {
        GameObject referenceObject;
        GameManager referenceScript;
        referenceObject = GameObject.FindGameObjectWithTag("GameManager");

        // referenceObject = GameObject.FindGameObjectWithTag("BoardCanvas");
        referenceScript = referenceObject.GetComponent<GameManager>();
        // referenceScript.pointHide = sliderUI.value;
        referenceScript.ChangeThreatPoints(sliderUI.value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
