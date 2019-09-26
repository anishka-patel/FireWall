using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [HeaderAttribute("Poputate in Inspector")]
    public GameObject pieceGraphic;
    public GameObject pieceEffect;
    [HeaderAttribute("Piece Color Sprites")]
    public List<Sprite> pieceSprite;

    [HeaderAttribute("Set Colors")]
    public Color onMove;
    public Color onAttack;
    public Color Default;
    [SerializeField] private float offsetFactor = 1.20f;
    [SerializeField] private float shrinkFactor = 0.85f;
    [SerializeField] public float zoomMEFactor = 1.10f;
    [SerializeField] public float zoomMDFactor = 1.33f;
    [HideInInspector] public bool ShowAvailableMoves { get; set; }
    [HideInInspector] public Square currentSquare;
    [HideInInspector] public Square previousSquare;
    [HideInInspector] public Square pointedSquare;
    [HideInInspector] public Square attackedSquare;
    [HideInInspector] public Player controller;
    [HideInInspector] public bool NoMovesAvailable;
    private List<List<Square>> potentialMoves;
    private Square[,] board;
    private PieceColor pieceColor;
    
    private SpriteRenderer graphicSR;
    private SpriteRenderer effectSR;
    private GameManager gameManager;

    private const string selectionLayer = "SelectionLayer";
    private const string defaultLayer = "PieceLayer";
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        board = gameManager.board;
        graphicSR = pieceGraphic.GetComponent<SpriteRenderer>();
        effectSR = pieceEffect.GetComponent<SpriteRenderer>();
    }

    public void SnapToMouse()
    {
        Vector3 currentPosition = this.transform.position;
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        this.transform.position = new Vector3(newPosition.x + offsetFactor, newPosition.y - offsetFactor, currentPosition.z);
        graphicSR.sortingLayerName = selectionLayer;
    }

    public void MoveToSquare(Square _square)
    {
        previousSquare = currentSquare;
        currentSquare.VacateSquare();
        _square.OccupySquare(this.controller);
        currentSquare = _square;
        SnapToSquareCenter(_square);
        graphicSR.sortingLayerName = defaultLayer;
        CheckIfMoved();
    }

    public void CheckIfMoved()
    {
        if(previousSquare != null && currentSquare!= null)
        {
            if (previousSquare.index.x == currentSquare.index.x && previousSquare.index.y == currentSquare.index.y)
            {
                return;
            }
            else
            {
                controller.playerMode = PlayerModes.Attacking;
                controller.selectedPiece = this;
            }
        }
        
    }

    public void CheckAvailableMoves()
    {
        int x = currentSquare.index.x;
        int y = currentSquare.index.y;
        bool rankPBlocked = false;
        bool rankMBlocked = false;
        bool filePBlocked = false;
        bool fileMBlocked = false;
        bool diagonalMMBlocked = false;
        bool diagonalPPBlocked = false;
        bool diagonalMPBlocked = false;
        bool diagonalPMBlocked = false;
        potentialMoves = new List<List<Square>>();
        float length = (float) gameManager.boardtype;
        List<Square> filePMoves = new List<Square>();
        List<Square> fileMMoves = new List<Square>();
        List<Square> rankPMoves = new List<Square>();
        List<Square> rankMMoves = new List<Square>();
        List<Square> diagonalMMMoves = new List<Square>();
        List<Square> diagonalPPMoves = new List<Square>();
        List<Square> diagonalPMMoves = new List<Square>();
        List<Square> diagonalMPMoves = new List<Square>();
        int xP, yP, xM, yM;
        for (int i = 1; i < length; i++)
        {
            xP = x + i;
            yP = y + i;
            xM = x - i;
            yM = y - i;

            if(CheckSquareInBoard(xP, y))
            {
                if (!rankPBlocked)
                {
                    if (board[xP, y].state == SquareStates.Default)
                    {
                        rankPMoves.Add(board[xP, y]);
                    }
                    else
                    {
                        rankPBlocked = true;
                    }
                }
                
            }
            if(CheckSquareInBoard(xM, y))
            {
                if (!rankMBlocked)
                {
                    if (board[xM, y].state == SquareStates.Default)
                    {
                        rankMMoves.Add(board[xM, y]);
                    }
                    else
                    {
                        rankMBlocked = true;
                    }
                }
            }
            if (CheckSquareInBoard(x, yP))
            {
                if (!filePBlocked)
                {
                    if (board[x, yP].state == SquareStates.Default)
                    {
                        filePMoves.Add(board[x, yP]);
                    }
                    else
                    {
                        filePBlocked = true;
                    }
                }
            }
            if (CheckSquareInBoard(x, yM))
            {
                if (!fileMBlocked)
                {
                    if (board[x, yM].state == SquareStates.Default)
                    {
                        fileMMoves.Add(board[x, yM]);
                    }
                    else
                    {
                        fileMBlocked = true;
                    }
                }
            }
            if (CheckSquareInBoard(xM, yM))
            {
                if (!diagonalMMBlocked)
                {
                    if (board[xM, yM].state == SquareStates.Default)
                    {
                        diagonalMMMoves.Add(board[xM, yM]);
                    }
                    else
                    {
                        diagonalMMBlocked = true;
                    }
                }
            }
            if (CheckSquareInBoard(xP, yP))
            {
                if (!diagonalPPBlocked)
                {
                    if (board[xP, yP].state == SquareStates.Default)
                    {
                        diagonalPPMoves.Add(board[xP, yP]);
                    }
                    else
                    {
                        diagonalPPBlocked = true;
                    }
                }
            }
            if (CheckSquareInBoard(xM, yP))
            {
                if (!diagonalMPBlocked)
                {
                    if (board[xM, yP].state == SquareStates.Default)
                    {
                        diagonalMPMoves.Add(board[xM, yP]);
                    }
                    else
                    {
                        diagonalMPBlocked = true;
                    }
                }
            }
            if (CheckSquareInBoard(xP, yM))
            {
                if (!diagonalPMBlocked)
                {
                    if (board[xP, yM].state == SquareStates.Default)
                    {
                        diagonalPMMoves.Add(board[xP, yM]);
                    }
                    else
                    {
                        diagonalPMBlocked = true;
                    }
                }
            }
        }
        potentialMoves.Add(rankPMoves);
        potentialMoves.Add(filePMoves);
        potentialMoves.Add(rankMMoves);
        potentialMoves.Add(fileMMoves);
        potentialMoves.Add(diagonalMMMoves);
        potentialMoves.Add(diagonalPPMoves);
        potentialMoves.Add(diagonalPMMoves);
        potentialMoves.Add(diagonalMPMoves);
        //Debug.Log("Found Potential Moves");
    }

    public bool CheckSquareInBoard(int _x, int _y)
    {
        if( _x >= 0 && _x < (int) gameManager.boardtype && _y >= 0 && _y < (int) gameManager.boardtype)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void HighlightAvailableMoves()
    {
        int i = 0;
            foreach (List<Square> sList in potentialMoves)
            {
                foreach (Square s in sList)
                {
                    if (s.state == SquareStates.Default)
                    {
                        s.PieceEffect(s.moveAvailable);
                        i++;
                        //Debug.Log("Highlighting available moves!");
                    }
                }

            } 
        if (i == 0)
        {
            NoMovesAvailable = true;
        }
    }


    public void RemoveHighlight()
    {
        //Debug.Log("Removing Highlight!");
        foreach (List<Square> sList in potentialMoves)
        {
            foreach (Square s in sList)
            {
                if (s.state == SquareStates.Default)
                {
                    s.PieceEffect(s.defalutColor);
                }
            }

        }
    }
    
    public bool CheckPointedSquareAvailable()
    {
        foreach(List<Square> sList in potentialMoves)
        {
            if(sList.Contains(pointedSquare))
                return true;
        }
        return false;
    }

    public void ShootArrow(Square _square)
    {
        foreach(List<Square> sList in potentialMoves)
        {
            foreach(Square s in sList)
            {
                if (s.Equals(_square))
                {
                    attackedSquare = s;
                    s.SquareEffect(s.flamed);
                    s.ChangeState(SquareStates.Flamed);
                    RemoveHighlight();
                    controller.playerMode = PlayerModes.Default;
                    SetEffect(this.Default);
                    controller.SetMoveData(previousSquare.index, 
                        currentSquare.index, 
                        attackedSquare.index);
                }
            }
            
        }
    }

    private void BuildWall()
    {
        //TODO: Code to build wall
    }


    private void SnapToSquareCenter(Square _square)
    {
        Vector3 currentPosition = this.transform.position;
        Vector3 squarePosition = _square.transform.position;
        Vector3 newPosition = new Vector3(squarePosition.x, squarePosition.y, currentPosition.z);
        this.transform.position = newPosition;
        
    }

    public void FitToBoard(int _factor)
    {   
        Vector3 newScale = new Vector3(1.0f / (_factor * 2.0f) * shrinkFactor, 
            1.0f / (_factor * 2.0f) * shrinkFactor, 
            1);
        this.transform.localScale = newScale;
    }

    public void PointedSquare()
    {
        foreach (Square s in board)
        {
            if (s.pointed)
                pointedSquare = s;
        }
    }

    public void ZoomIn(float _zoom)
    {
        Vector3 _scale = new Vector3(this.transform.localScale.x * _zoom,
            this.transform.localScale.y * _zoom,
            this.transform.localScale.z * _zoom);
        this.transform.localScale = _scale;
    }

    public void ZoomOut(float _zoom)
    {
        Vector3 _scale = new Vector3(this.transform.localScale.x * (1 / _zoom),
            this.transform.localScale.y * (1 / _zoom),
            this.transform.localScale.z * (1 / _zoom));
        this.transform.localScale = _scale;
    }
    
    public void SetPieceColor()
    {
        pieceColor = controller.pieceColor;
        graphicSR = pieceGraphic.GetComponent<SpriteRenderer>();
        graphicSR.sprite = pieceSprite[(int)pieceColor];
        graphicSR.enabled = true;
    }

    
    public void SetEffect(Color _color)
    {
        effectSR.color = _color;
        effectSR.enabled = true;
    }
}
