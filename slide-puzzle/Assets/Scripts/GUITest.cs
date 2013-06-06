using UnityEngine;
using System.Collections;

public class GUITest : MonoBehaviour {
    private bool start = false;
    private int toolbarInt = 0;
    private Board board;
    private BoxCollider camScreen;
    private string[] toolbarStrings = {"Easy", "Normal", "Hard"};

    void Start() {
        board = (Board)GameObject.Find("/SmallBoard").GetComponent(typeof(Board));
        camScreen = (BoxCollider)GetComponent(typeof(BoxCollider));
    }

    void OnGUI () {
        if(!start) {
            GUI.Box(new Rect(100, 100, Screen.width-200, Screen.height-200), "Title");
            if(GUI.Button(new Rect(Screen.width/2-25, Screen.height*3/4, 50, 20), "Start")) {
                iTween.MoveBy(gameObject, new Vector3(0,-20,0), 0.25f);
                iTween.RotateTo(gameObject, new Vector3(0,-90*(toolbarInt+1),0), 0.25f);
                start = true;
                camScreen.enabled = false;
            }
            if(GUI.Button(new Rect(Screen.width/2-25, Screen.height*3/4-60, 50, 20), "Start Tutorial")) {
                start = true;
                camScreen.enabled = false;
            }
            toolbarInt = GUI.Toolbar(new Rect(25, 25, 250, 30), toolbarInt, toolbarStrings);
        }
        else
        // Make a background box
            if(GUI.Button(new Rect(10,10,50,20), "Menu")) {
                if(board.isComplete())
                    Debug.Log("complete");
                else
                    Debug.Log("not complete");
            }

        // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
   /*     if(GUI.Button(new Rect(20,40,80,20), "Level 1")) {
        //    Application.LoadLevel(1);
        }

        // Make the second button.
        if(GUI.Button(new Rect(20,70,80,20), "Level 2")) {
        //    Application.LoadLevel(2);
        }*/
    }
}
