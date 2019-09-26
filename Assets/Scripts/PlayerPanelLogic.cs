using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerData))]
public class PlayerPanelLogic : MonoBehaviour
{
    PlayerData playerData;
    Image playerImage;
    Text playerIDText;
    Text playerTeamButtonText;
    Text playerColorButtonText;
    Text playerNameInputText;
    Text botButtonText;


    private void OnEnable()
    {
        playerData = GetComponent<PlayerData>();
        Text[] textObjects = GetComponentsInChildren<Text>();
        Image[] images = GetComponentsInChildren<Image>();
        foreach(Text text in textObjects)
        {
            if (text.gameObject.name.Equals("PlayerIDText"))
            {
                playerIDText = text;
                //Debug.Log("PlayerIDText found!");
            }
            if(text.gameObject.name.Equals("PlayerTeamButtonText"))
            {
                playerTeamButtonText = text;
            }
            if (text.gameObject.name.Equals("PlayerColorButtonText"))
            {
                playerColorButtonText = text;
            }
            if (text.gameObject.name.Equals("PlayerNameInputText"))
            {
                playerNameInputText = text;
            }
            if (text.gameObject.name.Equals("BotButtonText"))
            {
                botButtonText = text;
            }
        }
        foreach(Image image in images)
        {
            if (image.gameObject.name.Equals("PlayerImage"))
            {
                playerImage = image;
            }
        }

        UpdatePanel();
    }

    public void ChangeTeam(Teams _team)
    {
        playerData.teamID = _team;
        UpdatePanel();
    }

    public void ChangeColor(PieceColor _color)
    {
        playerData.color = _color;
        UpdatePanel();
    }

    public PieceColor GetColor()
    {
        return playerData.color;
    }
    public Teams GetTeam()
    {
        return playerData.teamID;

    }
    public void AssignName()
    {
        playerData.playerName = playerNameInputText.text;
        UpdatePanel();
    }

    public void ToggleBot()
    {
        playerData.isBot = !playerData.isBot;
        UpdatePanel();
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

    private void UpdatePanel()
    {
        playerIDText.text = "Player " + playerData.playerID.ToString();
        playerImage.color = SetColor((int)playerData.color);
        playerTeamButtonText.text = playerData.teamID.ToString();
        if (playerData.isBot)
        {
            botButtonText.text = "Bot\nOn";
            botButtonText.fontStyle = FontStyle.Bold;
        }
        else
        {
            botButtonText.text = "Bot\nOff";
            botButtonText.fontStyle = FontStyle.Normal;
        }
    }
}
