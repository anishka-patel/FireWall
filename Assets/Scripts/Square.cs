using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    [HeaderAttribute("Populate In Inspector")]
    public List<Sprite> squareSprite;
    public GameObject squareGraphic;
    public GameObject squareEffect;
    public GameObject pointerEffect;
    public GameObject pieceEffect;

    [HeaderAttribute("Set colors for square effect")]
    public Color moveAvailable;
    public Color moveFrom;
    public Color moveTo;
    public Color flamed;
    public Color Walled;
    public Color pointerAt;
    public Color defalutColor;

    [HeaderAttribute("Do Not Modify!")]
    [HeaderAttribute("For Debug Only")]
    public SquareType type = SquareType.Default;
    public SquareStates state = SquareStates.Default;

    public Vector3 size;

    [SerializeField] public Vector index = new Vector();

    public bool pointed = false;


    private SpriteRenderer graphicSR;
    private SpriteRenderer pointerEffectSR;
    private SpriteRenderer pieceEffectSR;
    private SpriteRenderer squareEffectSR;

    private Player playerAtSquare;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        graphicSR = squareGraphic.GetComponent<SpriteRenderer>();
        pointerEffectSR = pointerEffect.GetComponent<SpriteRenderer>();
        pieceEffectSR = pieceEffect.GetComponent<SpriteRenderer>();
        squareEffectSR = squareEffect.GetComponent<SpriteRenderer>();
    }
 
    public void OccupySquare(Player _player)
    {
        this.playerAtSquare = _player;
        ChangeState(SquareStates.Occupied);
    }

    public void VacateSquare()
    {
        this.playerAtSquare = null;
        ChangeState(SquareStates.Default);
    }

    public void ChangeState(SquareStates _state)
    {
        this.state = _state;
    }

    private void OnMouseDown()
    {
        if(gameManager.onMove.playerMode == PlayerModes.Attacking)
        {
            gameManager.onMove.selectedPiece.ShootArrow(this);
        }
        
    }

    private void OnMouseEnter()
    {
        pointed = true;
        PointerEffect(pointerAt);
    }
    private void OnMouseExit()
    {
        pointed = false;
        PointerEffect(defalutColor);
    }

    public void SetSquareColor()
    {
        switch (type)
        {
            case SquareType.Light:
                graphicSR.sprite = squareSprite[0];
                graphicSR.enabled = true;
                break;
            case SquareType.Dark:
                graphicSR.sprite = squareSprite[1];
                graphicSR.enabled = true;
                break;
            case SquareType.Default:
                graphicSR.enabled = false;
                break;
        }
    }

    public void PieceEffect(Color _color)
    {
        pieceEffectSR.color = _color;
        pieceEffectSR.enabled = true;
    }

    public void SquareEffect(Color _color)
    {
        squareEffectSR.color = _color;
        squareEffectSR.enabled = true;
    }

    public void PointerEffect(Color _color)
    {
        pointerEffectSR.color = _color;
        pointerEffectSR.enabled = true;
    }

    public void FitToBoard(int _factor)
    {
        Vector3 newScale = new Vector3(1.0f / (_factor * 2.0f), 1.0f / (_factor * 2.0f), 1);
        graphicSR = squareGraphic.GetComponent<SpriteRenderer>();
        this.transform.localScale = newScale;
        this.size = graphicSR.bounds.size;
    }

}
