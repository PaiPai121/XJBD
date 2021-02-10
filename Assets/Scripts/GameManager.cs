using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string[] cardNameList = {"xiaoba","zoro","along"};
    public GameObject[,] cells = new GameObject[5,9];
    public GameObject[] chesses;
    public GameObject preChess;
    public GameObject preCell;
    public GameObject selected;
    public bool showingRange;
    private List<GameObject> changedList;
    private GUICard guiCard;
    public bool busy = false;
    public Chess selectedCard;
    public int PlayersNum = 2; // 玩家总数
    public int CurrentPlayerNum;// 当前是哪个玩家的回合
    private Skill[,] skills = {{new Skill("八刀流",8,8,8,2,"夏姬八砍"  )},
    {new Skill("狮子歌歌",1,1,1,1,"敏锐的居合斩，range 1\n challenge 2， 判定时对方defense -1")},
    {new Skill("水流击",1,1,1,0,"如子弹一般掷出\n水滴，range 2，对面前横向最多\n三个敌人challenge 2(钝器伤害)")}};
    private GUIInFo GuiInFo;
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1920, 1080, false);
        // 获取GUI
        guiCard = GameObject.Find("Canvas").GetComponent<GUICard>();
        GuiInFo = GameObject.Find("Canvas").GetComponent<GUIInFo>();

        changedList = new List<GameObject>();
        initGrid();

        // 初始化一个棋子
        // CardBase card = new CardBase(0,"xiaoba",1,1,null);
        int cardnum = 6; //Random.Range(2,6);
        chesses = new GameObject[cardnum];
        for(int i =0;i<cardnum;i++){
            chesses[i] = (GameObject)Instantiate(preChess, new Vector3(0,0,0), transform.rotation);
            initSingleChess(chesses[i],i%2);
        }
        
        // chesses = GameObject.FindGameObjectsWithTag("Chess");
        // 设置占据
        foreach(var che in chesses){
            cells[che.GetComponent<Chess>().row,che.GetComponent<Chess>().col].GetComponent<GridHandle>().setOccupied(che.GetComponent<Chess>());
            // cells[che.GetComponent<Chess>().row,che.GetComponent<Chess>().col].GetComponent<GridHandle>().occupied = true;
            // cells[che.GetComponent<Chess>().row,che.GetComponent<Chess>().col].GetComponent<GridHandle>().occupiedPlayerId = che.GetComponent<Chess>().playerNumber;
        }
        turnInit();
    }

    private void initSingleChess(GameObject chess,int _playernumber){
        Chess chessCom = chess.GetComponent<Chess>();
        int _row = Random.Range(0,5);
        int _col = Random.Range(0,9);

        while (cells[_row,_col].GetComponent<GridHandle>().occupied)
        {
            _row = Random.Range(0,5);
            _col = Random.Range(0,9);
        }
        int _cardId = Random.Range(0,3);// 随机卡牌
        cells[_row,_col].GetComponent<GridHandle>().occupied = true;
        int skillnums = skills.GetLength(1);
        Skill[] sks =  new Skill[skillnums];
        for (int i = 0;i<skillnums;i++){
            sks[i] = skills[_cardId,i];
        }
        
        CardBase _card = new CardBase(_cardId,cardNameList[_cardId],Random.Range(0,8),Random.Range(0,8),sks);
        chessCom.initChess(_card,_row,_col);
        chessCom.playerNumber = _playernumber;
        chessCom.freshAct(2);
    }

    private void initGrid(){
                int startx = -9;
        int starty = -4;
        // 初始化棋盘
        for(int i=0;i<5;i++) // 5行
            {
            for(int j = 0;j<9;j++) // 9列
                {
                    // 单元格横坐标
                    int celly = 2*i;
                    // 单元格纵坐标
                    int cellx = 2*j;
                    cells[i,j] = (GameObject)Instantiate(preCell, new Vector3(startx + cellx,starty + celly,0), transform.rotation);
                    cells[i,j].GetComponent<GridHandle>().gridCol = j; // 列
                    cells[i,j].GetComponent<GridHandle>().gridRow = i; // 行
                    
                }
            }
    }

    // Update is called once per frame
    void Update()
    {
        if (selected == null)
        {
            guiCard.drawCard = false;
        }
        else{
            // Debug.Log("sel:"+selected);
            // Debug.Log("selcard:"+selectedCard.card);
            guiCard.cardForShow = selectedCard.card;
            // guiCard.Profile = selected.GetComponent<SpriteRenderer>().sprite;
            // Debug.Log("gui:"+guiCard.cardForShow);
            guiCard.drawCard = true;
        }
    }

    public void ShowMoveRange(){
        CloseRange();
        showingRange = true;
        foreach(var cell in cells){
            GridHandle cellHandle = cell.GetComponent<GridHandle>();
            int range = selectedCard.moveRange;
            int cellRow = cellHandle.gridRow; 
            int cellCol = cellHandle.gridCol; 
            int selRow = selectedCard.row; 
            int selCol = selectedCard.col; 
            if(Distance(cellRow,cellCol,selRow,selCol)>0 && Distance(cellRow,cellCol,selRow,selCol) < range&&cellHandle.canMove){
                if (cellHandle.occupied )//&& !(cellHandle.occupyChess.GetComponent<Chess>().playerNumber == selectedCard.playerNumber))
                {   if(!(cellHandle.occupiedPlayerId == selectedCard.playerNumber) && Distance(cellRow,cellCol,selRow,selCol) <= selectedCard.attackRange){
                        cellHandle.SetGrid(Command.Attack);
                    }
                }
                else{
                    cellHandle.SetGrid(Command.Move);
                }
            }
            changedList.Add(cell);
        }
    }

    private int Distance(int row1,int col1,int row2,int col2){
        return Mathf.Abs(row1 - row2)+Mathf.Abs(col1 - col2); 
    }
    public void ShowAttackRange(){

    }

    public void CloseRange(){
        showingRange = false;
        foreach(var cell in changedList){
            cell.GetComponent<GridHandle>().ResetGrid();
        }
        changedList.Clear();
    }

    private void SetGrid(int i,int j, Command command){
        if (command == Command.Move)
        {
            cells[i,j].GetComponent<SpriteRenderer>().color = Color.green;
        }
        else if (command == Command.Attack){
            cells[i,j].GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    private void ResetGrid(int i,int j){
        cells[i,j].GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void moveChess(int row,int col){
        cells[selectedCard.row,selectedCard.col].GetComponent<GridHandle>().resetOccupied();
        // cells[selectedCard.row,selectedCard.col].GetComponent<GridHandle>().occupied = false;
        // cells[selectedCard.row,selectedCard.col].GetComponent<GridHandle>().occupiedPlayerId = -1;
        selectedCard.move(row,col);
        cells[selectedCard.row,selectedCard.col].GetComponent<GridHandle>().setOccupied(selectedCard);
        // cells[row,col].GetComponent<GridHandle>().occupied = true;
        // cells[row,col].GetComponent<GridHandle>().occupiedPlayerId = selectedCard.playerNumber;
        CloseRange();
    }

    private int _dice(int n){
        int r = 0;
        for(int i = 0;i<n;i++){
            r += Random.Range(1,7);
        }
        return r;
    }

    public void clearSelected(){
        selected = null;
        selectedCard = null;
    }

    public void setSelected(GameObject obj){
        selectedCard = obj.GetComponent<Chess>();
        selected = obj;
    }

    public bool checkAttackRange(int _row,int _col)
    {
        return (Distance(_row,_col,selectedCard.row,selectedCard.col) <= selectedCard.attackRange);
    }

    public void clearGrid(int _row,int _col){
        cells[_row,_col].GetComponent<GridHandle>().resetOccupied();
        // cells[_row,_col].GetComponent<GridHandle>().occupied = false;
        // cells[_row,_col].GetComponent<GridHandle>().occupiedPlayerId = -1;
    }

    public void closeRangeAndSelected(){
        CloseRange();
        clearSelected();
    }

    /// <summary>
    /// 初始化回合
    /// </summary>
    public void turnInit(){
        CurrentPlayerNum = PlayersNum -1;
        turnEnd();
    }

    /// <summary>
    /// 此处开始回合操作，玩家序号顺移
    /// </summary>
    /// <value></value>
    public void turnEnd(){
        if (CurrentPlayerNum == PlayersNum-1)
        {
            CurrentPlayerNum = 0;
        }
        else{
            CurrentPlayerNum ++ ;
        }
        GuiInFo.CurrentPlayer = CurrentPlayerNum;// 更新玩家回合显示
        foreach(var che in chesses){
            if(!(che==null)){
                Chess cheCom = che.GetComponent<Chess>();
                if(cheCom.playerNumber == CurrentPlayerNum){
                    cheCom.freshAct(2);
                }
                else{
                    cheCom.clearAct(); // 清空非当前玩家棋子的体力
                }
                    
            }
        }
    }

    public void sendGUIMessage(string message){
        GuiInFo.Message = message;
    }
}
