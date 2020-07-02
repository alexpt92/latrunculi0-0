using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{

    public string name { get; set; }
    public float x { get; set; }
    public float y { get; set; }
    // Start is called before the first frame update
    public Piece(string newName, float newX, float newY) 
    {
        name = newName;
        x = newX;
        y = newY;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
