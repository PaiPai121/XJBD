using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHandle : MonoBehaviour
{
    public bool occupied = false;
    public bool movable = true;
    public bool attackable = false;
    public int gridRow;
    public int gridCol;
    private GameManager GameManager;
    private SpriteRenderer cellSpriteRender;
    public bool canMove = true;
    // public GameObject occupyChess = null;
    public int occupiedPlayerId = -1;
    public Chess OccupiedChess = null;
    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        cellSpriteRender = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown() {
        if(!GameManager.busy){
            if (occupied){
                OccupiedChess.OnMouseDown();
                return;
            }
            Debug.Log("this is "+tag+", Row:" + gridRow+", Col:" + gridCol);
            print("Movable is " + movable );
            if ( movable){
                GameManager.moveChess(gridRow,gridCol);
            }
            else{
                // 取消选择
                GameManager.CloseRange();
            }
            GameManager.clearSelected();
        }
    }

    public void SetGrid(Command command){
        if (command == Command.Move)
        {
            cellSpriteRender.color = Color.green;
            movable = true;
        }
        else if (command == Command.Attack){
            cellSpriteRender.color = Color.red;
        }
    }

    public void ResetGrid(){
        cellSpriteRender.color = Color.white;
        movable = false;
    }

    public void setOccupied(Chess _chess){
        occupied = true;
        OccupiedChess = _chess;
        occupiedPlayerId = _chess.playerNumber;
    }

    public void resetOccupied(){
        occupied = false;
        OccupiedChess = null;
        occupiedPlayerId = -1;
    }
}
