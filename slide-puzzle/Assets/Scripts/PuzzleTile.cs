/**
 * @author Kay Choi
 */
using UnityEngine;
using System.Collections;

public class PuzzleTile : MonoBehaviour {
    private int value;
    private int free = 0;
    private int[] index;

    /**
     * Sets the value of the tile.
     * @param val the new value
     * @return the new value
     */
    public int setValue(int val) {
        value = val;

        TextMesh label = (TextMesh)transform.Find("label").GetComponent(typeof(TextMesh));
        label.renderer.material.color = Color.black;
        label.text = val.ToString();

        return value;
    }

    /**
     * Retrieves the value of the tile.
     * @return the integer value
     */
    public int getValue() {
        return value;
    }

    /**
     * Sets the direction that the tile can move towards. The codes are as follows:
     * 0 = none
     * 1 = up
     * 2 = left
     * 3 = right
     * 4 = down
     * @param dirCode the direction code
     * @return the new direction code
     */
    public int setDirection(int dirCode) {
        free = dirCode;
        return free;
    }

    /**
     * Retrieves the direction that the tile can move towards.
     * @return the direction code
     */
    public int getDirection() {
        return free;
    }

    /**
     * Retrieves the indeces of the tile.
     * @return the indeces as an array
     */
    public int[] getIndex() {
        return index;
    }

    /**
     * Sets the indeces of the tile.
     * @param newIndex the new indeces of the tile as an array
     * @return true if successful
     */
    public bool setIndex(int[] newIndex) {
        index = newIndex;
        return index != null;
    }
}
