using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIInFo : MonoBehaviour
{
    public int StartPointx;
    public int StartPointy;
    public int ButtonWidth;
    public int ButtonHeight;
    public Font font;
    public int fontSize;
    public int CurrentPlayer;
    public int TurnNumStartx;
    public int TurnNumStarty;
    public int EndGamex;
    public int EndGamey;
    public int Messagex;
    public int Messagey;
    public string Message;
    public int Sliderx;
    public int Slidery;
    public int Sliderw;
    public int Sliderh;
    private GameManager gameManager;
    private GUIStyle _buttonStyle;
    private Transform _cameraTransform;
    private float _angle = -0.2f;
    private float _cameray = -4.2f;
    // Start is called before the first frame update
    void Start()
    {
        _cameraTransform = GameObject.Find("Main Camera").transform;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _buttonStyle = new GUIStyle ();
		_buttonStyle.fontSize = fontSize;
		_buttonStyle.fontStyle = FontStyle.Bold;
		_buttonStyle.alignment = TextAnchor.MiddleCenter;
        _buttonStyle.font = font;
        
    }

    // Update is called once per frame
    void OnGUI()
    {
        if (GUI.Button(new Rect(StartPointx,StartPointy,ButtonWidth,ButtonHeight),"EndTurn",_buttonStyle)  ||   GUI.Button(new Rect(StartPointx,StartPointy,ButtonWidth,ButtonHeight),"") )
        {
            Debug.Log("End Turn");
            gameManager.turnEnd();
        }

        GUI.Label(new Rect(TurnNumStartx,TurnNumStarty,ButtonWidth,ButtonHeight),"Player " +CurrentPlayer.ToString() + "  's Turn",_buttonStyle);

        if (GUI.Button(new Rect(EndGamex,EndGamey,ButtonWidth,ButtonHeight),"EndGame",_buttonStyle) ||   GUI.Button(new Rect(EndGamex,EndGamey,ButtonWidth,ButtonHeight),"") ){
            Application.Quit();
            // UnityEditor.EditorApplication.isPlaying = false;
        }

        // message
        GUI.Label(new Rect(Messagex,Messagey,100,100),"Last message: "+Message,_buttonStyle);

        // camera
        _angle = GUI.VerticalSlider(new Rect(Sliderx,Slidery,Sliderw,Sliderh),_angle,1,-1);
        _cameray = GUI.VerticalSlider(new Rect(Sliderx-50,Slidery,Sliderw,Sliderh),_cameray,10,-10);
        _cameraTransform.position = new Vector3( _cameraTransform.position.x,_cameray,_cameraTransform.position.z );
        _cameraTransform.rotation = new Quaternion(_angle,_cameraTransform.rotation.y,_cameraTransform.rotation.z,_cameraTransform.rotation.w);
        GUI.Label(new Rect(Sliderx-100,Slidery-100,100,100),"相机视角调整",_buttonStyle);
    }
}
