using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
  //  public string button;
    #region build board at Runtime
  /*  public Image outlineImage;

    [HideInInspector]
    public Vector2Int boardPosition = Vector2Int.zero;
    [HideInInspector]
    public boardScript board = null;
    [HideInInspector]
    public RectTransform rectTransform = null;*/
  /*  public void Setup(Vector2Int newBoardPosition, boardScript newBoard)
    {
        boardPosition = newBoardPosition;
        board = newBoard;

    }*/

    #endregion


}
