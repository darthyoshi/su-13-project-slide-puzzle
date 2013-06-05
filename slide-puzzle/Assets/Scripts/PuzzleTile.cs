using UnityEngine;
using System.Collections;

public class PuzzleTile : MonoBehaviour {
    private int value;
    private int free = 0;
    private static string[] directions = {
        "none", "up", "left", "right", "down"
    };
    private GameObject selected;
    private Board board;
    private int[] index;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        if(Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 500)) {
                if (hit.transform.tag == "Tile") {
                    selected = hit.transform.gameObject;
                    if(gameObject.name == selected.name && hit.transform.parent.gameObject.name == board.gameObject.name) {
                        board.moveTile(this);
                    }
                }
            }
        }
    }

    /**
     * Sets the value of the tile.
     * @param val the new value
     * @return the new value
     */
    public int setValue(int val) {
        return (value = val);
    }

    /**
     * Retrieves the value of the tile.
     * @return the integer value
     */
    public int getValue() {
        return value;
    }

    /**
     * Sets the direction that the tile can move towards.
     * @param dirCode the direction code
     * @return the new direction
     */
    public string setDirection(int dirCode) {
        free = dirCode;
        return directions[dirCode];
    }

    /**
     * Retrieves the direction that the tile can move towards.
     * @return the direction
     */
    public string getDirection() {
        return directions[free];
    }

    public int[] getIndex() {
        return index;
    }

    public bool setIndex(int[] newIndex) {
        index = newIndex;
        return index != null;
    }

    public bool setBoard(Board newBoard) {
        board = newBoard;
        return board != null;
    }
}
