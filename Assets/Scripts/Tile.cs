using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Position position;
    public GameObject reference = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnMouseUp()
    {
        GameManager gm = GameManager._instance;
        ChessPiece chessPiece = reference.GetComponent<ChessPiece>();

        gm.SetPositionEmpty(chessPiece.position);
        chessPiece.position = position;
        gm.SetPosition(reference);

        gm.NextTurn();
    }

    public void SetPosition(Position pos)
    {
        position = pos;
    }

    public void SetReference(GameObject chessPiece)
    {
        reference = chessPiece;
    }

    public GameObject GetReference() 
    { 
        return reference; 
    }
}
