using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {
    private bool start = false, menu = false;
    private int toolbarInt = 1;
    private Board board;
    private BoxCollider camScreen;
    private string[] toolbarStrings = {"Easy", "Normal", "Hard"};
    private string[] boardSize = {"Medium", "Large", "XLarge"};
    private float timer = 0;
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
        }
    }

    void OnGUI () {
        if(!start) {
            //title screen
            GUI.BeginGroup(new Rect(Screen.width/2-320,Screen.height/2-240,640,480));

            GUI.Box(new Rect(0,0,640,480), "Title");

            if(GUI.Button(new Rect(320-75, 300, 150, 30), "Tutorial")) {
                iTween.MoveBy(gameObject, new Vector3(0,-20,0), 0.25f);
                start = true;
                camScreen.enabled = false;

                board = (Board)GameObject.Find("/SmallBoard").GetComponent(typeof(Board));
            }

            //start options
            GUI.BeginGroup(new Rect(320-125,360,250,110));

            if(GUI.Button(new Rect(50, 75, 150, 30), "Start Game")) {
                iTween.MoveBy(gameObject, new Vector3(0,-20,0), 0.25f);
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

        else if(menu) {
            GUI.Box(new Rect(Screen.width/2-50, 20, 100, 20), "Paused");

            if(GUI.Button(new Rect(10,10,50,20), "Menu")) {
                menu = false;
                camScreen.enabled = false;
            }

            //pause menu
            GUI.BeginGroup(new Rect(70,10,300,400));

            GUI.Box(new Rect(0,0,300,400), "pause menu");

            GUI.EndGroup();
        }

/*        else if(board.isComplete()) {
            if(GUI.Button(new Rect(10,10,50,20), "Menu")) {
                menu = true;
                camScreen.enabled = true;
            }
        }
*/
        else {
            GUI.Box(new Rect(Screen.width/2-50, 20, 100, 20), "Time: "+min.ToString("00")+":"+sec.ToString("00"));

            if(GUI.Button(new Rect(10,10,50,20), "Menu")) {
                menu = true;
                camScreen.enabled = true;
            }
        }
    }
}
