using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class ChessPiece : MonoBehaviour
{
    public GameObject tile;
    public string player;
    public string pieceType;
    public Position position;
    public bool firstMove = true;
    public bool selected = false;

    private Position newPos;
    private int column;
    private int row;
    private float z;
    private float x;
    private (int, int) increment;

    public void SetPosition(Position pos)
    {
        (column, row) = pos.GetIndex();
        z = -3.5f + column * 1f;
        x = 3.5f - row * 1f;
        this.transform.position = new Vector3(x, 2.5f, z);
        position = pos;
    }

    public void DestroyValidMoves()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
    }

    public void CreateValidMoves()
    {
        if (pieceType == "King")
        {
            KingMoves();
        }
        else if (pieceType == "Queen")
        {
            QueenMoves();
        }
        else if (pieceType == "Rook")
        {
            LineMove(1, 0);
            LineMove(-1, 0);
            LineMove(0, 1);
            LineMove(0, -1);
        }
        else if (pieceType == "Bishop")
        {
            LineMove(1, 1);
            LineMove(1, -1);
            LineMove(-1, 1);
            LineMove(-1, -1);
        }
        else if (pieceType == "Knight")
        {
            KnightMoves();
        }
        else if (pieceType == "Pawn")
        {
            if (player == "white")
            {
                PawnMoves(position + (0, 1));
                if (firstMove) { PointMove(position + (0, 2)); }
            }
            else
            {
                PawnMoves(position + (0, -1));
                if (firstMove) { PointMove(position + (0, -2)); }
            }
        }
    }

    private void KingMoves()
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if ((i, j) != (0, 0)) { PointMove(position + (i, j)); }
            }
        }
    }

    private void QueenMoves()
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if ((i, j) != (0, 0)) { LineMove(i, j); }
            }
        }
    }

    private void KnightMoves()
    {
        for (int i = -2; i < 3; i++)
        {
            for (int j = -2; j < 3; j++)
            {
                if (i * j == 2 || i * j == -2) { PointMove(position + (i, j)); }
            }
        }
    }

    private void PawnMoves(Position pos)
    {
        GameManager gm = GameManager._instance;

        if (gm.PositionOnBoard(pos))
        {
            if (gm.GetPosition(pos) == null)
            {
                TileSpawn(pos);
            }

            newPos = pos + (1, 0);
            if (gm.PositionOnBoard(newPos) && gm.GetPosition(newPos) != null && gm.GetPosition(newPos).GetComponent<ChessPiece>().player != player)
            {
                // MovePlateAttackSpawn(x + 1, y);
            }

            newPos = pos + (-1, 0);
            if (gm.PositionOnBoard(newPos) && gm.GetPosition(newPos) != null && gm.GetPosition(newPos).GetComponent<ChessPiece>().player != player)
            {
                // MovePlateAttackSpawn(x - 1, y);
            }
        }
    }

    public void LineMove(int columnIncrement, int rowIncrement)
    {
        GameManager gm = GameManager._instance;

        increment = (columnIncrement, rowIncrement);
        newPos = position + increment;

        while (gm.PositionOnBoard(newPos) && gm.GetPosition(newPos) == null)
        {
            TileSpawn(newPos);
            newPos += increment;
        }

        if (gm.PositionOnBoard(newPos) && gm.GetPosition(newPos).GetComponent<ChessPiece>().player != player)
        {
            // MovePlateAttackSpawn(x, y);
        }
    }

    public void PointMove(Position pos)
    {
        GameManager gm = GameManager._instance;
        if (gm.PositionOnBoard(pos))
        {
            GameObject cp = gm.GetPosition(pos);

            if (cp == null)
            {
                TileSpawn(pos);
            }
            else if (cp.GetComponent<ChessPiece>().player != player)
            {
                // MovePlateAttackSpawn(x, y);
            }
        }
    }

    public void TileSpawn(Position pos)
    {
        (column, row) = pos.GetIndex();
        z = -3.5f + column * 1f;
        x = 3.5f - row * 1f;

        GameObject t = Instantiate(tile, new Vector3(x, 2.5f, z), Quaternion.identity, this.transform);
        Tile tScript = t.GetComponent<Tile>();
        tScript.SetPosition(pos);
    }

    private void OnMouseUp() 
    {
        if (!selected)
        {
            GameManager gm = GameManager._instance;
            if (!gm.IsGameOver() && gm.GetCurrentPlayer() == player)
            {
                gm.SelectNewPiece();
                CreateValidMoves();
                gm.selectedPiece = this.transform.gameObject;
                selected = !selected;
            }
        }
        else
        {
            DestroyValidMoves();
            selected = !selected;
        }
    }
}
