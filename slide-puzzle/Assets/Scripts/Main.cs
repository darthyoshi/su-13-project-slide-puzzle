/**
 * @author Kay Choi
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Main : MonoBehaviour {
    private bool isStarted = false, isMenuActive = false, isGameComplete = false, isTutorial = false;
    private int toolbarInt = 1, min = 0, sec = 0, tutPgNo = 0, confType = -1;
    private Board board;
    private BoxCollider camScreen;
    private string[] toolbarStrings = {"Easy", "Normal", "Hard"};
    private string[] boardSize = {"Medium", "Large", "XLarge"};

    private string[] tutorialTitles = {
        "Introduction",
        "Manipulating the Puzzle",
        "Manipulating the Puzzle",
        "Manipulating the Puzzle",
        "Scoring",
        "Scoring",
        "Completing the Puzzle",
        "Completing the Puzzle",
        "Completing the Puzzle",
        "End"
    };
    private string[] tutorialPages = {
        "Welcome! If you are familiar with slide puzzles and wish to skip this "+
            "tutorial, go ahead and click the \"Exit\" button. Otherwise, click "+
            "the \"Continue\" button to proceed.",
        "As you can see, the lower right corner is empty. This is the free slot, "+
            "and only the highlighted tiles that border the free slot can be "+
            "moved.",
        "While disabled for the duration of the tutorial, in normal gameplay tiles "+
            "are selected using the mouse. For now, we will be moving the #6 tile, "+
            "highlighted in blue.",
        "Once a tile is moved, the slot that it was originally in becomes the "+
            "new free slot, which allows a new group of tiles, which have been "+
            "highlighted, to move.",
        "Notice that the number in the box labeled \"Moves\" has changed. This "+
            "number, which tracks the total number of tiles moved, is one of your "+
            "scores in the game.",
        "The other score is the total time spent on the puzzle, which in normal "+
            "gameplay is tracked in the box currently labeled \"Tutorial\".",
        "To complete the puzzle, all the tiles must be rearranged in the proper "+
            "order."/* + " This may be numerical order, as in this tutorial, or to form "+
            "an image."*/,
        "This tutorial is already mostly in order, with only the #5, #6, and #8 "+
            "tiles out of place.",
        "Once those tiles are in the proper order, the game will automatically "+
            "end, and you will be given the option of sharing or recording your "+
            "scores.",
        "And that's the end of the tutorial. Have fun!"
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
            GUI.BeginGroup(new Rect(Screen.width/2-100,Screen.height/2-95,200,190));

            GUI.Box(new Rect(0,0,200,190), "\nCongratulations!\n\nCompletion time: "+
                min.ToString("00")+":"+sec.ToString("00")+"\nMoves made: "+
                board.getMoveCount());

            if(GUI.Button(new Rect(20,100,160,30), "Save Scores")) {
                //calls to server, Facebook, etc
            }

            if(GUI.Button(new Rect(20,140,160,30), "Quit")) {
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

    private void drawHud(string stat) {
        GUI.Box(new Rect(Screen.width/2-50, 20, 100, 20), stat);
        GUI.Box(new Rect(Screen.width/2-50, 50, 100, 20), "Moves: "+board.getMoveCount());

        if(GUI.Button(new Rect(10,10,75,20), "Pause")) {
            isMenuActive = camScreen.enabled = true;
        }
    }

    private void drawTitleScreen() {
        GUI.BeginGroup(new Rect(Screen.width/2-320,Screen.height/2-240,640,480));

        GUI.Box(new Rect(0,0,640,480), "");
        GUI.DrawTexture(new Rect(120,-60,400,400), logo, ScaleMode.ScaleToFit);

        if(GUI.Button(new Rect(245, 240, 150, 30), "Tutorial")) {
            isTutorial = true;
            toolbarInt = -1;
            board = (Board)GameObject.Find("/SmallBoard").GetComponent(typeof(Board));
        }

        //start options
        GUI.BeginGroup(new Rect(195,300,250,150));

        if(GUI.Button(new Rect(23, 100, 100, 30), "Start Game")) {
            isStarted = true;
            camScreen.enabled = false;

            board = (Board)GameObject.Find("/"+boardSize[toolbarInt]+"Board").GetComponent(typeof(Board));
        }

        GUI.Label(new Rect(75,0,100,20), "Select Difficulty");

        iTween.RotateTo(gameObject, new Vector3(0,-90*(toolbarInt+1),0), 0.5f);

        toolbarInt = GUI.Toolbar(new Rect(0, 30, 250, 30), toolbarInt, toolbarStrings);

        if(GUI.Button(new Rect(127, 100, 100, 30), "Exit Game")) {
            Application.Quit();
        }

        GUI.EndGroup();

        GUI.EndGroup();
    }

    private void drawPauseMenu() {
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

    private void drawConfirmBox(string type) {
        GUI.BeginGroup(new Rect(Screen.width/2-100,Screen.height/2-60,200,120));

        GUI.Box(new Rect(0,0,200,120), "Do you really wish to "+type+"?");

        if(GUI.Button(new Rect(10,35,180,30), "Yes")) {
            isMenuActive = isGameComplete = false;
            board.resetBoard();
            confType = -1;
            timer = 0f;

            if(type == "restart") {
                camScreen.enabled = false;
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

    private void drawTutorial() {
        Dictionary<string, PuzzleTile> dict = board.getFreeTiles();

        drawHud("Tutorial");

        GUI.BeginGroup(new Rect(Screen.width/2-200,100,400,150));

        GUI.Box(new Rect(0,0,400,150), "Tutorial - "+tutorialTitles[tutPgNo]);

        GUI.Label(new Rect(10,20,380,90), tutorialPages[tutPgNo]);

        switch(tutPgNo) {
            case 1:
                dict["north"].renderer.material.color = new Color(.8f, .5f, .5f, 1);
                dict["west"].renderer.material.color = new Color(.8f, .5f, .5f, 1);
                break;
            case 2:
                dict["north"].renderer.material.color = new Color(.8f, .8f, .8f, 1);
                dict["west"].renderer.material.color = new Color(.5f, .5f, .8f, 1);
                break;
            case 3:
                if(dict.ContainsKey("west") && dict["west"].getValue() == 6) {
                    board.moveTile(dict["west"]);
                }

                else {
                    dict["north"].renderer.material.color = new Color(.8f, .5f, .5f, 1);
                    dict["west"].renderer.material.color = new Color(.8f, .5f, .5f, 1);
                    dict["east"].renderer.material.color = new Color(.8f, .5f, .5f, 1);
                }
                break;
            case 4:
                dict["north"].renderer.material.color = new Color(.8f, .8f, .8f, 1);
                dict["west"].renderer.material.color = new Color(.8f, .8f, .8f, 1);
                dict["east"].renderer.material.color = new Color(.8f, .8f, .8f, 1);
                break;
            case 8:
            case 9:
                if(dict.ContainsKey("north") && dict["north"].getValue() == 8) {
                    board.moveTile(dict["north"]);
                    timer = Time.time;
                }

                else if(Time.time >= timer+.25f && dict.ContainsKey("east") && dict["east"].getValue() == 5) {
                    board.moveTile(dict["east"]);
                    timer = Time.time;
                }

                else if(Time.time >= timer+.5f && dict.ContainsKey("south") && dict["south"].getValue() == 6) {
                    board.moveTile(dict["south"]);
                }
                break;
        }

        if(tutPgNo < tutorialTitles.Length-1 && GUI.Button(new Rect(310,120,80,20),"Continue")) {
            tutPgNo++;
        }

        if(GUI.Button(new Rect(10,120,80,20),"Exit")) {
            tutPgNo = 0;
            toolbarInt = 1;
            timer = 0f;
            isTutorial = false;
            board.resetBoard();
        }

        GUI.EndGroup();
    }
}
