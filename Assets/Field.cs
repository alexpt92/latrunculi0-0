using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    // Start is called before the first frame update
    public string fieldName { get; set; }
    public float coordX { get; set; }
    public float coordY { get; set; }
    public string button { get; set; }

    public Field (string buttonName, string fieldName, float y, float x)
    {
        this.fieldName = fieldName;
        this.coordX = x;
        this.coordY = y;
        this.button = buttonName;
    }
}
