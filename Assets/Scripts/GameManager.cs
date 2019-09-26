using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //References to be poputated in Unity Inspector
    [HeaderAttribute("Populate in Inspector")]
    [HeaderAttribute("Game Elements")]
    [SerializeField] private Square square;
    [SerializeField] private GameObject boardContainer;
    [SerializeField] private Player playerPrefab;
    [SerializeField] private Piece piecePrefab;

    [HeaderAttribute("Populate in Inspector")]
    [HeaderAttribute("UI Elements")]
    public Text boardText;
    public Text playersText;
    public Text piecesText;
    public Text onTurnText;
    public Text onMoveColorText;
    public Text onMoveModeText;
    public RectTransform moveDataList;
    public RectTransform playerDataList;
    public GameObject playerDataElementPrefab;
    public GameObject moveDataElement;

    [HeaderAttribute("Properties: Start and update turns")]
    public float startDelay = 3.0f;
    public float repeatRate = 0.5f;

    [SerializeField] public List<MoveData> moveDataSet;

    public Square[,] board;
    public BoardType boardtype;
    public Player onMove;
    public Player lastMoved;
    public int playerNum;
    public int pieceNum;

    private List<Player> players;
    private List<Player> turnOrder;

    private bool gameOver = false;

    #region: Game Manager
    // Start is called before the first frame update
    void Start()
    {
        players = new List<Player>();

        boardtype = GameData.GetInstance().boradtype;
        GenerateBoard((int)boardtype);

        playerNum = GameData.GetInstance().playersData.Count();
        pieceNum = GameData.GetInstance().playersData[0].pieceNum;

        FormationGenerator fG = new FormationGenerator();
        List<Vector> spawnPoints = fG.Generate((int)boardtype,(int)boardtype, playerNum, pieceNum);
        List<List<Vector>> spawnList = new List<List<Vector>>();
        for(int i =0; i < playerNum; i++)
        {
            List<Vector> set = new List<Vector>();
            for (int j = 0; j < pieceNum; j++)
            {
                
                int rand = Random.Range(0, spawnPoints.Count());
                set.Add(spawnPoints[0]);
                spawnPoints.Remove(spawnPoints[0]);
            }
            spawnList.Add(set);
        }
        for(int i = 0; i < playerNum; i++)
        {
            SummonPlayer(GameData.GetInstance().playersData[i], spawnList[i]);
        }
        
        SettingTurnOrder();
        moveDataSet = new List<MoveData>();
        InvokeRepeating("CheckIfGameOver", startDelay, repeatRate);
        InvokeRepeating("ChangeTurns", startDelay, repeatRate);
    }

    private void UpdateMoveData(MoveData moveData)
    {
        moveDataSet.Add(moveData);
    }

    private void GenerateBoard(int _type)
    {
        board = new Square[_type, _type];
        Vector3 _positionStart = new Vector3(boardContainer.transform.position.x, 
            boardContainer.transform.position.y, 
            boardContainer.transform.position.z) ;
        Vector3 _position = _positionStart;
        Vector3 squareSize = new Vector3();
        int a = 0;
        for (int j = 0; j < _type; j++)
        {
            for (int i = 0; i < _type; i++)
            {
                Square squareSpawned = Instantiate(square, _position, Quaternion.identity, boardContainer.transform);
                squareSpawned.FitToBoard(_type);

                squareSpawned.index.x = i;
                squareSpawned.index.y = j;
                if (a % 2 == 0)
                {
                    squareSpawned.type = SquareType.Dark;
                }
                else
                {
                    squareSpawned.type = SquareType.Light;
                }
                squareSpawned.SetSquareColor();
                squareSpawned.state = SquareStates.Default;
                squareSize = squareSpawned.size;
                board[i, j] = squareSpawned;
                _position.x += squareSize.x;
                a++;
            }
            _position.y -= squareSize.y;
            _position.x = _positionStart.x;
            a++;
        }
    }


    private void SummonPlayer(PlayerData data, List<Vector> position)
    {
        Player player = (Player) Instantiate(playerPrefab);
        GameObject playerDataElement = Instantiate(playerDataElementPrefab, playerDataList);
        player.playerDataElement = playerDataElement;
        player.ID = data.playerID;
        player.isBot = data.isBot;
        player.teamID = data.teamID;
        player.pieceColor = data.color;
        player.playerState = PlayerStates.Default;
        player.UpdatePlayerDataElement();
        players.Add(player);
        for (int i = 0; i < data.pieceNum; i++)
        {
            Piece summoned = SummonPieces(player, position[i], piecePrefab, (int) boardtype);
            player.pieces.Add(summoned);
        }
        
    }

    private Piece SummonPieces(Player _player, Vector position, Piece _piece , int _type)
    {
        Vector3 spawnPosition = new Vector3();
        spawnPosition = board[position.x,position.y].transform.position;
        Piece summoned = (Piece) Instantiate(_piece, spawnPosition, Quaternion.identity, _player.transform);
        summoned.controller = _player;
        summoned.FitToBoard(_type);
        summoned.SetPieceColor();
        summoned.currentSquare = board[position.x, position.y];
        board[position.x, position.y].state = SquareStates.Occupied;
        summoned.ShowAvailableMoves = true;
        return summoned;
    }


    private void SettingTurnOrder()
    {
        turnOrder = new List<Player>();
        List<Player> playerList = players;
        while (playerList.Count > 0)
        {
            int random = Random.Range(0, playerList.Count);
            turnOrder.Add(playerList[random]);
            players.Remove(playerList[random]);
        }
    }

    public void CheckIfGameOver()
    {
        if (onMove == null)
        {
            return;
        }
        if (onMove.CheckPiecesCanMove() == false)
        {
            
        }
    }

    public void ChangeTurns()
    {
        
        if (gameOver)
        {
            Debug.Log("GameOver!");
        }
        else
        {
            UpdateTurnUI();
            if (onMove == null)
            {
                onMove = turnOrder[0];
                onMove.playerState = PlayerStates.OnMove;
                onMove.playerMode = PlayerModes.Moving;
            }
            if (onMove.CheckPiecesCanMove() == false)
            {
                Debug.Log("PlayerCantMovePieces");
                Debug.Log("SkippingTurn");
                NextInTurn();

            }
            if (onMove.playerMode == PlayerModes.Default)
            {
                moveDataSet.Add(onMove.currentMoveData);
                lastMoved = onMove;
                lastMoved.playerState = PlayerStates.NotOnMove;
                NextInTurn();
                onMove.playerState = PlayerStates.OnMove;
                onMove.playerMode = PlayerModes.Moving;
            }
        }
    }

    public void NextInTurn()
    {
        int index = turnOrder.IndexOf(onMove);
        if (index + 1 < turnOrder.Count)
        {
            onMove = turnOrder[index + 1];
        }
        else
        {
            onMove = turnOrder[0];
        }
    }
    #endregion

    #region: UI Manager
    private void UpdateTurnUI()
    {

    }
    #endregion

}
