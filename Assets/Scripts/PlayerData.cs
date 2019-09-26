using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData: MonoBehaviour
{
    public int playerID;
    public int pieceNum;
    public string playerName;
    public PieceColor color;
    public Teams teamID;
    public bool isBot;
}
