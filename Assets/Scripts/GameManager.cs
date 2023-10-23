using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public struct Position
{
    public int Column { get; set; }
    public int Row { get; set; }

    public Position(int column, int row)
    {
        Column = column;
        Row = row;
    }

    public readonly (int, int) GetIndex()
    {
        return (Column - 1, Row - 1);
    }

    public readonly bool InBoardCenter()
    {
        return (Column == 4 || Column == 5) && (Row == 4 || Row == 5);
    }

    public static Position operator +(Position a, (int column, int row) b)
    {
        return new Position(a.Column + b.column, a.Row + b.row);
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    [SerializeField] private GameObject 
        whitePawn, whiteKnight, whiteBishop, whiteRook, whiteQueen, whiteKing, 
        blackPawn, blackKnight, blackBishop, blackRook, blackQueen, blackKing;

    public GameObject selectedPiece = null;

    private GameObject[,] positions = new GameObject[8, 8];
    private List<GameObject> playerWhite;
    private List<GameObject> playerBlack;
    private GameObject wKing;
    private GameObject bKing;

    public float z;
    public float x;

    public enum GameStates
    {
        Menu,
        Playing,
        Paused,
        Over
    }

    public GameStates currentState;

    public enum Players { white, black }
    public Players currentPlayer;

    private int column;
    private int row;

    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerWhite = new List<GameObject> 
        {
            Spawn(whiteRook, 1, 1), Spawn(whiteKnight, 2, 1), Spawn(whiteBishop, 3, 1), Spawn(whiteQueen, 4, 1),
            Spawn(whiteKing, 5, 1), Spawn(whiteBishop, 6, 1), Spawn(whiteKnight, 7, 1), Spawn(whiteRook, 8, 1),
        };
        
        for (int i = 1; i < 9; i++)
        {
            playerWhite.Add(Spawn(whitePawn, i, 2));
        }

        playerBlack = new List<GameObject>
        {
            Spawn(blackRook, 1, 8), Spawn(blackKnight, 2, 8), Spawn(blackBishop, 3, 8), Spawn(blackQueen, 4, 8),
            Spawn(blackKing, 5, 8), Spawn(blackBishop, 6, 8), Spawn(blackKnight, 7, 8), Spawn(blackRook, 8, 8),
        };
        
        for (int i = 1; i < 9; i++)
        {
            playerBlack.Add(Spawn(blackPawn, i, 7));
        }

        for (int i = 0; i < 16; i++)
        {
            SetPosition(playerWhite[i]);
            SetPosition(playerBlack[i]);
        }

        currentState = GameStates.Playing;
        currentPlayer = Players.white;
    }
    
    public GameObject Spawn(GameObject pieceType, int column, int row)
    {
        z = -3.5f + (column - 1) * 1f;
        x = 3.5f - (row - 1) * 1f;
        GameObject newChessPiece = Instantiate(pieceType, new Vector3(x, 2.5f, z), Quaternion.identity, this.transform);
        ChessPiece cp = newChessPiece.GetComponent<ChessPiece>();
        cp.player = pieceType.name[..5];
        cp.pieceType = pieceType.name[5..];
        cp.position = new Position(column, row);
        
        if (cp.pieceType == "King")
        {
            if (cp.player == "white")
            {
                wKing = newChessPiece;
            }
            else
            {
                bKing = newChessPiece;
            }
        }

        return newChessPiece;
    }

    public void SetPosition(GameObject chessPiece)
    {
        (column, row) = chessPiece.GetComponent<ChessPiece>().position.GetIndex();
        positions[column, row] = chessPiece;
    }

    public void SetPositionEmpty(Position pos)
    {
        (column, row) = pos.GetIndex();
        positions[column, row] = null;
    }

    public GameObject GetPosition(Position pos)
    {
        (column, row) = pos.GetIndex();
        return positions[column, row];
    }

    public bool PositionOnBoard(Position pos)
    {
        (column, row) = pos.GetIndex();
        return !(column < 0 || row < 0 || column >= positions.GetLength(0) || row >= positions.GetLength(1));
    }

    public string GetCurrentPlayer()
    {
        return currentPlayer.ToString();
    }
    
    public void NextTurn()
    {
        if (wKing.GetComponent<ChessPiece>().position.InBoardCenter() && bKing.GetComponent<ChessPiece>().position.InBoardCenter())
        {
            currentState = GameStates.Over;
        }

        if (currentPlayer == Players.white)
        {
            currentPlayer = Players.black;
        }
        else
        {
            currentPlayer = Players.white;
        }
        
        selectedPiece = null;
    }

    public bool IsGameOver()
    {
        return currentState == GameStates.Over;
    }

    public void SelectNewPiece()
    {
        if (selectedPiece != null)
        {
            selectedPiece.GetComponent<ChessPiece>().DestroyValidMoves();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == GameStates.Over && Input.GetMouseButtonDown(0))
        {
            currentState = GameStates.Playing;
            SceneManager.LoadScene("ChessAtPeace");
        }
    }
}
