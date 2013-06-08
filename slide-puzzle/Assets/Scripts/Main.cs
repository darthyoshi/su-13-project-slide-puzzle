/**
 * @author Kay Choi
 */

using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
    private bool isStarted = false, isMenuActive = false, isGameComplete = false, isTutorial = false;
    private int toolbarInt = 1, min = 0, sec = 0, tutPgNo = 0;
    private Board board;
    private BoxCollider camScreen;
    private string[] toolbarStrings = {"Easy", "Normal", "Hard"};
    private string[] boardSize = {"Medium", "Large", "XLarge"};
    private float timer = 0f;
    public Texture2D logo;

    void Start() {
        camScreen = (BoxCollider)GetComponent(typeof(BoxCollider));
    }

    void Update() {
        if(isStarted && !isMenuActive) {
            timer += Time.deltaTime;
            min = (int)timer/60;
            sec = (int)timer%60;

            isGameComplete = camScreen.enabled = board.isComplete();
        }
    }

    void OnGUI () {
        if(!isStarted && !isTutorial) {
            drawTitleScreen();
        }

        //pause screen
        else if(isMenuActive) {
            GUI.Box(new Rect(Screen.width/2-50, 20, 100, 20), "Paused");

            GUI.BeginGroup(new Rect(Screen.width/2-100,Screen.height/2-90,200,180));

            GUI.Box(new Rect(0,0,200,180), "");

            if(GUI.Button(new Rect(20,20,160,30), "Resume")) {
                isMenuActive = camScreen.enabled = false;
            }

            drawPauseMenu();

            GUI.EndGroup();
        }

        else if(isTutorial) {
            drawHud("Tutorial");
            /* tutorial outline
             * - introduce slide mechanic
             *   - free tile
             *   - highlight neighbor tiles
             *   - highlight tile and move
             * - introduce win condition
             *   - complete puzzle
             * - return to title screen
             */
        }

        else if(isGameComplete) {
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
            isMenuActive = camScreen.enabled = true;
        }
    }

    void drawTitleScreen() {
        GUI.BeginGroup(new Rect(Screen.width/2-320,Screen.height/2-240,640,480));

        GUI.Box(new Rect(0,0,640,480), "Title");

        if(GUI.Button(new Rect(245, 300, 150, 30), "Tutorial")) {
            isTutorial = true;
            iTween.RotateTo(gameObject, new Vector3(0,0,0), 0.25f);

            board = (Board)GameObject.Find("/SmallBoard").GetComponent(typeof(Board));
        }

        //start options
        GUI.BeginGroup(new Rect(195,360,250,110));

        if(GUI.Button(new Rect(50, 75, 150, 30), "Start Game")) {
            iTween.RotateTo(gameObject, new Vector3(0,-90*(toolbarInt+1),0), 0.25f);
            isStarted = true;
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
            drawConfirmBox("restart");
        }

        if(GUI.Button(new Rect(20,130,160,30), "Quit")) {
            drawConfirmBox("quit");
        }
    }

    void drawConfirmBox(string type) {
        GUI.BeginGroup(new Rect(25,22,150,135));

        GUI.Box(new Rect(0,0,150,135), "Do you really wish to "+type+"?");

        if(GUI.Button(new Rect(10,50,130,30), "Yes")) {
            isMenuActive = isGameComplete = false;
            board.resetBoard();

            if(type == "restart") {
                camScreen.enabled = false;
                timer = 0f;
            }

            else {
                camScreen.enabled = true;
                isStarted = isTutorial = false;
            }
        }

        if(GUI.Button(new Rect(10,90,130,30), "No")) {
            isMenuActive = false;
        }
    }
}
