using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int ID { get; set; }
    public Teams teamID;

    public PieceColor pieceColor;

    public PlayerModes playerMode = PlayerModes.Default;
    public PlayerStates playerState = PlayerStates.Default;

    public List<Piece> pieces;
    public Piece selectedPiece;

    public MoveData currentMoveData;

    public bool PiecesCanMove = false;
    public bool isBot = false;

    public GameObject playerDataElement;

    private void Start()
    {
        currentMoveData = new MoveData();
    }

    public bool CheckPiecesCanMove()
    {
        foreach (Piece p in pieces)
        {
            if (!p.NoMovesAvailable)
            {
                PiecesCanMove = true;
            }
            else
            {
                PiecesCanMove = false;
            }
        }
        return PiecesCanMove;
    }

    public void SetMoveData(Vector previous, Vector current, Vector attack)
    {
        currentMoveData.playerID = ID;
        currentMoveData.currentPos = previous;
        currentMoveData.finalPos = current;
        currentMoveData.attackPos = attack;
    }

    public void UpdatePlayerDataElement()
    {
        Text[] texts = playerDataElement.GetComponentsInChildren<Text>();
        Image image = playerDataElement.GetComponentInChildren<Image>();

        image.color = SetColor((int)pieceColor);

        foreach(Text text in texts)
        {
            if(text.gameObject.name == "PlayerIDText")
            {
                text.text = "Player " + ID.ToString();
            }
            if (text.gameObject.name == "TeamIDText")
            {
                text.text = "Team " + ((int)teamID).ToString();
            }
            if (text.gameObject.name == "PlayerIDText")
            {
                if (isBot)
                {
                    text.text = "BOT ON";
                }
                else
                {
                    text.text = "BOT OFF";
                }
            }
        }
    }

    private Color SetColor(int num)
    {
        switch (num)
        {
            case 0:
                return Color.red;
            case 1:
                return Color.green;
            case 2:
                return Color.blue;
            case 3:
                return Color.cyan;
            case 4:
                return Color.magenta;
            case 5:
                return Color.yellow;
            case 6:
                return Color.white;
            case 7:
                return Color.black;
            default:
                return Color.clear;
        }
    }


}
