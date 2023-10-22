using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    public static PieceManager _instance;

    [SerializeField] private GameObject whitePawn, whiteKnight, whiteBishop, whiteRook, whiteQueen, blackPawn, blackKnight, blackBishop, blackRook, blackQueen;

    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
