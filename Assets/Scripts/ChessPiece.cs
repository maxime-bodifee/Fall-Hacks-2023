using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChessPiece : MonoBehaviour
{
    public GameObject tile;
    public string player;
    public string pieceType;
    public Position position;
    public bool firstMove = true;

    private Position newPos;
    private int column;
    private int row;
    private float z;
    private float x;
    private (int, int) increment;

    public void CreateValidMoves()
    {
        GameManager gm = GameManager._instance;

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
                PawnMoves(position + (1, 0));
                if (firstMove) { PointMove(position + (2, 0)); }
            }
            else
            {
                PawnMoves(position + (-1, 0));
                if (firstMove) { PointMove(position + (-2, 0)); }
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
            for (int j = -2; i < 3; i++)
            {
                if (i * j == 2 || i * j == -2) { PointMove(newPos + (i, j)); }
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
                // MovePlateSpawn(pos);
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
            // MovePlateSpawn(x, y);
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
                // MovePlateSpawn(x, y);
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
        x = 4.5f - row * 1f;

        GameObject t = Instantiate(tile, new Vector3(x, 2.5f, z), Quaternion.identity);
        Tile tScript = t.GetComponent<Tile>();
    }
}
