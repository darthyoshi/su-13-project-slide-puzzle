/**
 * @author Kay Choi
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {
    private bool isStarted = false, isMenuActive = false, isGameComplete = false, isTutorial = false;
    private int toolbarInt = 1, min = 0, sec = 0, tutPgNo = 0, confType = -1;
    private Board board;
    private BoxCollider camScreen;
    private string[] toolbarStrings = {"Easy", "Normal", "Hard"};
    private string[] boardSize = {"Medium", "Large", "XLarge"};
    private string[] tutorialTitles = {"one","two","three","four"};
    private string[] tutorialPages = {
        "tutorial page one",
        "tutorial page two",
        "tutorial page three",
        "tutorial page four"
    };
    private float timer = 0f;
    public Texture2D logo;

    void Start() {
        camScreen = (BoxCollider)GetComponent(typeof(BoxCollider));
    }

    void Update() {
        if(isStarted && !isMenuActive && !isGameComplete) {
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

            drawPauseMenu();
        }

        else if(isTutorial) {
            drawTutorial();
        }

        else if(isGameComplete) {
            GUI.BeginGroup(new Rect(Screen.width/2-100,Screen.height/2-90,200,180));

            GUI.Box(new Rect(0,0,200,180), "Completion time: "+min.ToString("00")+":"+sec.ToString("00"));

            //display completion message

            //display score options

            if(GUI.Button(new Rect(20,130,160,30), "Quit")) {
                camScreen.enabled = true;
                isGameComplete = isStarted = isTutorial = false;
                toolbarInt = 1;
                timer = 0f;
            }

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
            toolbarInt = -1;
            board = (Board)GameObject.Find("/SmallBoard").GetComponent(typeof(Board));
        }

        //start options
        GUI.BeginGroup(new Rect(195,360,250,110));

        if(GUI.Button(new Rect(50, 75, 150, 30), "Start Game")) {
            isStarted = true;
            camScreen.enabled = false;

            board = (Board)GameObject.Find("/"+boardSize[toolbarInt]+"Board").GetComponent(typeof(Board));
        }

        GUI.Label(new Rect(75,0,100,20), "Select Difficulty");

        iTween.RotateTo(gameObject, new Vector3(0,-90*(toolbarInt+1),0), 0.5f);

        toolbarInt = GUI.Toolbar(new Rect(0, 30, 250, 30), toolbarInt, toolbarStrings);

        GUI.EndGroup();

        GUI.EndGroup();
    }

    void drawPauseMenu() {
        switch(confType) {
            case 0:
                drawConfirmBox("restart");
                break;
            case 1:
                drawConfirmBox("quit");
                break;
            default:
                GUI.BeginGroup(new Rect(Screen.width/2-100,Screen.height/2-90,200,180));

                GUI.Box(new Rect(0,0,200,180), "");

                if(GUI.Button(new Rect(20,20,160,30), "Resume")) {
                    isMenuActive = camScreen.enabled = false;
                }

                if(GUI.Button(new Rect(20,75,160,30), "Restart")) {
                    confType = 0;
                }

                if(GUI.Button(new Rect(20,130,160,30), "Quit")) {
                    confType = 1;
                }

                GUI.EndGroup();
                break;
        }
    }

    void drawConfirmBox(string type) {
        GUI.BeginGroup(new Rect(Screen.width/2-100,Screen.height/2-60,200,120));

        GUI.Box(new Rect(0,0,200,120), "Do you really wish to "+type+"?");

        if(GUI.Button(new Rect(10,35,180,30), "Yes")) {
            isMenuActive = isGameComplete = false;
            board.resetBoard();
            confType = -1;

            if(type == "restart") {
                camScreen.enabled = false;
                timer = 0f;
            }

            else {
                camScreen.enabled = true;
                isStarted = isTutorial = false;
                toolbarInt = 1;
            }
        }

        if(GUI.Button(new Rect(10,80,180,30), "No")) {
            isMenuActive = false;
            confType = -1;
        }

        GUI.EndGroup();
    }

    void drawTutorial() {
            /* tutorial outline
             * - introduce slide mechanic
             *   - free tile
             *   - highlight neighbor tiles
             *   - highlight tile and move
             * - introduce win condition
             *   - complete puzzle
             * - return to title screen
             */
        Dictionary<string, PuzzleTile> dict = board.getFreeTiles();

        GUI.BeginGroup(new Rect(Screen.width/2-200,20,400,200));

        GUI.Box(new Rect(0,0,400,200), tutorialTitles[tutPgNo]);

        GUI.Label(new Rect(10,20,380,140), tutorialPages[tutPgNo]);

        if(GUI.Button(new Rect(290,170,100,20),"Continue")) {
            tutPgNo++;
        }

        if(GUI.Button(new Rect(10,170,100,20),"Exit Tutorial") || tutPgNo == tutorialTitles.Length) {
            tutPgNo = 0;
            isTutorial = false;
            toolbarInt = 1;
        }

        GUI.EndGroup();
    }
}
