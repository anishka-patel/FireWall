using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMotor : MonoBehaviour
{
    Piece piece;
    // Start is called before the first frame update
    void Start()
    {
        piece = GetComponent<Piece>();
    }

    private void OnMouseEnter()
    {
        switch (piece.controller.playerMode)
        {
            case (PlayerModes.Moving):
                piece.ZoomIn(piece.zoomMEFactor);
                piece.SetEffect(piece.onMove);
                break;
            case (PlayerModes.Attacking):
                if (piece.controller.selectedPiece.Equals(this.piece))
                {
                    piece.SetEffect(piece.onAttack);
                    piece.CheckAvailableMoves();
                    piece.HighlightAvailableMoves();
                }
                
                break;
            case (PlayerModes.Walling):
                break;
            case (PlayerModes.Default):
                break;
        }
        
    }

    private void OnMouseExit()
    {
        switch (piece.controller.playerMode)
        {
            case (PlayerModes.Moving):
                piece.ZoomOut(piece.zoomMEFactor);
                piece.SetEffect(piece.Default);
                break;
            case (PlayerModes.Attacking):
                break;
            case (PlayerModes.Walling):
                break;
            case (PlayerModes.Default):
                break;
        }
        
    }

    private void OnMouseDown()
    {
        switch (piece.controller.playerMode)
        {
            case (PlayerModes.Moving):
                piece.ZoomIn(piece.zoomMDFactor);
                //Debug.Log("This code is running!");
                piece.CheckAvailableMoves();
                piece.HighlightAvailableMoves();
                break;
            case (PlayerModes.Attacking):
                break;
            case (PlayerModes.Walling):
                break;
            case (PlayerModes.Default):
                break;
        }
    }

    private void OnMouseDrag()
    {
        switch (piece.controller.playerMode)
        {
            case (PlayerModes.Moving):
                {
                    piece.SnapToMouse();
                    piece.PointedSquare();
                }
                break;
            case (PlayerModes.Attacking):
                break;
            case (PlayerModes.Walling):
                break;
            case (PlayerModes.Default):
                break;
        }
        
    }

    private void OnMouseUp()
    {
        switch (piece.controller.playerMode)
        {
            case (PlayerModes.Moving):
                piece.ZoomOut(piece.zoomMDFactor);
                piece.RemoveHighlight();
                if (piece.CheckPointedSquareAvailable())
                {
                    piece.MoveToSquare(piece.pointedSquare);
                }
                else
                {
                    piece.MoveToSquare(piece.currentSquare);
                }
                break;
            case (PlayerModes.Attacking):
                break;
            case (PlayerModes.Walling):
                break;
            case (PlayerModes.Default):
                break;
        }
        
    }
}
