using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class GameGenerator : MonoBehaviour
{
    //References
    
    [HideInInspector] public int playerNum = 1;
    [HideInInspector] public int pieceNum = 1;
    [HideInInspector] public int teamNum = 1;
    [HideInInspector] public int boardNum = 1;
    [HideInInspector] public int boardIndex = 0;

    [HeaderAttribute("Populate in Inspector")]
    public Text playersButtonText;
    public Text piecesButtonText;
    public Text boardtypeButtonText;
    public GameObject playerPanelPrefab;
    public VerticalLayoutGroup playerSettingsPanel;

    [HideInInspector] public List<GameObject> players;
    [HideInInspector] public List<PlayerData> playersData;
    [HideInInspector] public List<PieceColor> availableColors;
    [HideInInspector] public List<Teams> availableTeams;
    [HideInInspector] public List<BoardType> boardtypes;

    private void Start()
    {
        availableColors = Enum.GetValues(typeof(PieceColor)).Cast<PieceColor>().ToList();
        availableTeams = Enum.GetValues(typeof(Teams)).Cast<Teams>().ToList();
        boardtypes = Enum.GetValues(typeof(BoardType)).Cast<BoardType>().ToList();
    }

    public void OnClick_PlayerNumToggle()
    {
        if(playerNum == 8)
        {
            playerNum = 0;
            foreach(GameObject p in players)
            {
                Destroy(p);
            }
        }
        playerNum++;
        playersButtonText.text = playerNum.ToString();
        GameObject player = (GameObject) Instantiate(playerPanelPrefab, playerSettingsPanel.transform);
        players.Add(player);
        PlayerData playerData = (PlayerData) player.GetComponent<PlayerData>();
        playersData.Add(playerData);
        playerData.playerID = playerNum;
        playerData.pieceNum = pieceNum;
        playerData.color = (PieceColor) availableColors[0];
        availableColors.Remove(playerData.color);
        playerData.teamID = (Teams) availableTeams[0];
        player.SetActive(true);
    }

    public void OnClick_PieceNumToggle()
    {
        if(pieceNum == 8)
        {
            pieceNum = 0;
        }
        pieceNum++;
        piecesButtonText.text = pieceNum.ToString();
    }

    public void OnClick_BoardtypeToggle()
    {
        boardIndex++;
        if(!(boardIndex < boardtypes.Count())){
            boardIndex = 0;
        }
        boardNum = (int)boardtypes[boardIndex];
        boardtypeButtonText.text = boardNum.ToString();
    }

    public void OnClick_ToggleColor( )
    {
        //Debug.Log("ToggleColor Button Pressed...");
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        //Debug.Log(obj.name.ToString());
        PlayerPanelLogic logic = obj.GetComponentInParent<PlayerPanelLogic>();
        availableColors.Add(logic.GetColor());
        logic.ChangeColor((PieceColor)availableColors[0]);
        availableColors.Remove(availableColors[0]);
        

    }

    public void OnClick_ToggleTeam()
    {
        //Debug.Log("ToggleTeam Button Pressed...");
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        //Debug.Log(obj.name.ToString());
        PlayerPanelLogic logic = obj.GetComponentInParent<PlayerPanelLogic>();
        if(availableTeams.Contains( logic.GetTeam() + 1))
        {
            logic.ChangeTeam((Teams)availableTeams[availableTeams.IndexOf(logic.GetTeam() + 1)]);
        }
        else
        {
            logic.ChangeTeam((Teams)availableTeams[0]);
        }
    }

    public void OnClick_ToggleBot()
    {
        //Debug.Log("ToggleBot Button Pressed...");
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        //Debug.Log(obj.name.ToString());
        PlayerPanelLogic logic = obj.GetComponentInParent<PlayerPanelLogic>();
        logic.ToggleBot();
    }

    public void OnEndEdit_AssignName()
    {
        //Debug.Log("Name Edited!...");
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        //Debug.Log(obj.name.ToString());
        PlayerPanelLogic logic = obj.GetComponentInParent<PlayerPanelLogic>();
        logic.AssignName();
    }

    public void OnClick_PlayOffline()
    {
        GameData gameData = GameData.GetInstance();
        gameData.GameID = DateTime.UtcNow.ToString();
        gameData.GameDateTime = DateTime.Now.ToString();
        gameData.playersData = playersData;
        gameData.boradtype = (BoardType) boardNum;
        if (gameData)
        {
            Debug.Log(gameData.GameID);
        }
        SceneManager.LoadScene("MainScene");
    }
}
