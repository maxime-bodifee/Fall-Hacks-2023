using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Position position;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnMouseUp()
    {
        GameManager gm = GameManager._instance;
        ChessPiece chessPiece = this.transform.parent.gameObject.GetComponent<ChessPiece>();

        gm.SetPositionEmpty(chessPiece.position);
        chessPiece.SetPosition(position);
        chessPiece.firstMove = false;
        chessPiece.selected = false;
        chessPiece.DestroyValidMoves();
        gm.SetPosition(this.transform.parent.gameObject);

        gm.NextTurn();
    }

    public void SetPosition(Position pos)
    {
        position = pos;
    }
}
