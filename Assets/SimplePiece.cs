using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePiece : Piece
{

    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager, Vector3Int movement)
    {
        base.Setup(newTeamColor, newSpriteColor, newPieceManager, movement);


        // mMovement = new Vector3Int(this.X, this.Y, 0);
        GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("simplePiece");
    }


}
