using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public string GameID;
    public string GameDateTime;
    public BoardType boradtype;
    public List<PlayerData> playersData;
    public List<MoveData> movesData;

    private static GameData instance;

    #region: Singleton

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        if(instance != this)
        {
            Destroy(this.gameObject);
        }
    }
    #endregion
    public static GameData GetInstance()
    {
        return instance;
    }
}
