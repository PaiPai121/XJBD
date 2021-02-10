using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chess : MonoBehaviour
{
    public int ActPoints; // 行动点数
    public float movingSpeed = 1.0f;
    public CardBase card;
    public int moveRange = 3;
    public int attackRange = 1;
    public int playerNumber = 1;
    // public bool canMove = false;
    // public bool canAttack = false;
    public int row = 3;
    public int col = 2;
    public int attackNum = 0;
    public List<Sprite> cardSprite;
    private GameManager GameManager;
    private SpriteRenderer CardRender;

    private bool moving = false;

    private int desx;
    private int desy;
    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        setPos(row,col);
        CardRender = gameObject.GetComponent<SpriteRenderer>();
        // CardRender.sprite = cardSprite[2];
    }

    // Update is called once per frame
    void Update()
    {
        // setPos(-9+2*(row-1),-4+2*(col-1));
    }

    void FixedUpdate()
    {
        if(moving){
            if (!(transform.position.x == desx)){
                moveToDestinationX();
            }
            else if(!(transform.position.y == desy)){
                moveToDestinationY();
            }
            else{
                moving =false;
                GameManager.busy = false;
            }
        }
        if(ActPoints==0){
            CardRender.color = Color.gray;
        }
        else{
            CardRender.color = Color.white;
        }
    }

    /// <summary>
    /// 刷新体力
    /// </summary>
    /// <param name="n">新体力值</param>
    public void freshAct(int n){
        ActPoints = n;
    }

/// <summary>
/// 清空体力
/// </summary>
/// <param></param>
    public void clearAct(){
        ActPoints = 0;
    }

    private void moveToDestinationX(){
        // 向目的地移动
        if (Mathf.Abs(transform.position.x - desx) < 1 ){
            transform.position = new Vector3(desx,desy,transform.position.z);
        }
        else if (transform.position.x < desx){
            transform.position = new Vector3(transform.position.x + movingSpeed, transform.position.y,transform.position.z  );
        }
        else if(transform.position.x > desx){
            transform.position = new Vector3(transform.position.x - movingSpeed, transform.position.y,transform.position.z  );
        }
    }

    private void moveToDestinationY(){
        // 向目的地移动
        if (Mathf.Abs(transform.position.y - desy) < 1 ){
            transform.position = new Vector3(desx,desy,transform.position.z);
        }
        else if (transform.position.y < desy){
            transform.position = new Vector3(transform.position.x, transform.position.y + movingSpeed,transform.position.z  );
        }
        else if(transform.position.y > desy){
            transform.position = new Vector3(transform.position.x, transform.position.y - movingSpeed,transform.position.z  );
        }
    }



    public void initChess(CardBase cardinit,int _row,int _col){
        card = cardinit;
        CardRender = gameObject.GetComponent<SpriteRenderer>();
        // Debug.Log(CardRender.sprite);
        CardRender.sprite = cardSprite[card.CardID];
        setPos(_row,_col);
        attackNum = card.Attack;
    }

    public void OnMouseDown(){
        /// <summary>
        /// 棋子鼠标点击
        /// </summary>
        /// <value></value>
        if (!GameManager.busy){
            Debug.Log("this is "+tag+", row:" + row +", col:" + col);
            if (GameManager.selected == null&&ActPoints>0){ // 如果没有选中的棋子才能选
                GameManager.setSelected(gameObject);
            }
            // Debug.Log("Showingrange: "+GameManager.showingRange);
            // Debug.Log("selId"+ GameManager.selectedCard.playerNumber+" , clickId" + playerNumber);
            if(GameManager.showingRange){
                if(GameManager.checkAttackRange(row,col) && !(GameManager.selectedCard.playerNumber == playerNumber)){
                    attack();
                }
                GameManager.closeRangeAndSelected();
            }
            else{
                if(ActPoints>0){
                // 点击此棋子行动需要行动点数大于0 
                    GameManager.ShowMoveRange();
                }
            }
        }
    }

    public void attack(){
        // 执行攻击消耗选定棋子行动力1
        GameManager.selectedCard.ActPoints -= 1;
        int damage  =_dice(GameManager.selectedCard.card.Attack);
        int defense = _dice(card.Defense);
        GameManager.sendGUIMessage("Attack, Deal "+ damage+" , Defense "+defense);
        Debug.Log("Attack, Deal "+ damage+" , Defense "+defense);
        if (damage > defense ){
            die();
        }
        else if(damage < defense){
            GameManager.selectedCard.die();
        }
        GameManager.CloseRange();
    }

    public void die(){
        GameManager.clearGrid(row,col);
        Destroy(gameObject);
    }

    public void setPos(int _row,int _col){
        Debug.Log("Set Chess to Row "+_row+" Col "+_col);
        // 直接放过去
        int y = -4+ 2*_row;
        int x = -9 + 2*_col;
        transform.position = new Vector3(x,y,0);
        row = _row;
        col = _col;
        // GameManager.CloseRange();
    }
    public void move(int _row,int _col){
        // 移动消耗棋子行动力1
        ActPoints -= 1;
        Debug.Log("Move Chess to Row "+_row+" Col "+_col);
        GameManager.sendGUIMessage("Move Chess to Row "+_row+" Col "+_col);
        moving = true;
        // 走过去
        desy = -4+ 2*_row;
        desx = -9 + 2*_col;
        // transform.position = new Vector3(x,y,0);
        row = _row;
        col = _col;
        GameManager.CloseRange();
        GameManager.busy =true;
    }

    private int _dice(int n){
        int r = 0;
        for(int i = 0;i<n;i++){
            r += Random.Range(1,7);
        }
        return r;
    }
}
