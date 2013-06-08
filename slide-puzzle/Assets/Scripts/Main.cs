using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
    private bool start = false, menu = false, complete = false, tutorial = false;
    private int toolbarInt = 1;
    private Board board;
    private BoxCollider camScreen;
    private string[] toolbarStrings = {"Easy", "Normal", "Hard"};
    private string[] boardSize = {"Medium", "Large", "XLarge"};
    private float timer = 0f;
    private int min = 0, sec = 0;
    public Texture2D logo;

    void Start() {
        camScreen = (BoxCollider)GetComponent(typeof(BoxCollider));
    }

    void Update() {
        if(start && !menu) {
            timer += Time.deltaTime;
            min = (int)timer/60;
            sec = (int)timer%60;

            complete = camScreen.enabled = board.isComplete();
        }
    }

    void OnGUI () {
        if(!start && !tutorial) {
            drawTitleScreen();
        }

        else if(menu) {
            GUI.Box(new Rect(Screen.width/2-50, 20, 100, 20), "Paused");

            //pause menu
            GUI.BeginGroup(new Rect(Screen.width/2-100,Screen.height/2-90,200,180));

            GUI.Box(new Rect(0,0,200,180), "");

            if(GUI.Button(new Rect(20,20,160,30), "Resume")) {
                menu = camScreen.enabled = false;
            }

            drawPauseMenu();

            GUI.EndGroup();
        }

        else if(tutorial && complete) {
        }

        else if(tutorial) {
            drawHud("Tutorial");
        }

        else if(complete) {
            GUI.BeginGroup(new Rect(Screen.width/2-100,Screen.height/2-75,200,180));

            GUI.Box(new Rect(0,0,200,180), "Completion time: "+min.ToString("00")+":"+sec.ToString("00"));

            drawPauseMenu();

            GUI.EndGroup();
        }

        else {
            drawHud("Time: "+min.ToString("00")+":"+sec.ToString("00"));
        }
    }

    void drawHud(string stat) {
        GUI.Box(new Rect(Screen.width/2-50, 20, 100, 20), stat);

        if(GUI.Button(new Rect(10,10,75,20), "Pause")) {
            menu = camScreen.enabled = true;
        }
    }

    void drawTitleScreen() {
        GUI.BeginGroup(new Rect(Screen.width/2-320,Screen.height/2-240,640,480));

        GUI.Box(new Rect(0,0,640,480), "Title");

        if(GUI.Button(new Rect(245, 300, 150, 30), "Tutorial")) {
        //    iTween.MoveBy(gameObject, new Vector3(0,-20,0), 0.25f);
            tutorial = true;
            camScreen.enabled = false;

            board = (Board)GameObject.Find("/SmallBoard").GetComponent(typeof(Board));
        }

        //start options
        GUI.BeginGroup(new Rect(195,360,250,110));

        if(GUI.Button(new Rect(50, 75, 150, 30), "Start Game")) {
            //iTween.MoveBy(gameObject, new Vector3(0,-20,0), 0.25f);
            iTween.RotateTo(gameObject, new Vector3(0,-90*(toolbarInt+1),0), 0.25f);
            start = true;
            camScreen.enabled = false;

            board = (Board)GameObject.Find("/"+boardSize[toolbarInt]+"Board").GetComponent(typeof(Board));
        }

        GUI.Label(new Rect(75,0,100,20), "Select Difficulty");
        toolbarInt = GUI.Toolbar(new Rect(0, 30, 250, 30), toolbarInt, toolbarStrings);

        GUI.EndGroup();

        GUI.EndGroup();
    }

    void drawPauseMenu() {
        if(GUI.Button(new Rect(20,75,160,30), "Restart")) {
            board.resetBoard();
            timer = 0f;
        }

        if(GUI.Button(new Rect(20,130,160,30), "Quit")) {
            board.resetBoard();
        //    iTween.MoveBy(gameObject, new Vector3(0,20,0), 0.25f);
            start = tutorial = menu = complete = false;
            camScreen.enabled = true;
        }
    }
}
