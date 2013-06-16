/**
 * @author Kay Choi
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {
    public PuzzleTile puzzleTile;
    private PuzzleTile[,] tiles;
    private int dim;
    private int[] freeIndex;
    private int moves;

    // Use this for initialization
    void Start () {
        switch(gameObject.name) {
            case "SmallBoard":
                dim = 3;
                break;
            case "MediumBoard":
                dim = 4;
                break;
            case "LargeBoard":
                dim = 5;
                break;
            case "XLargeBoard":
                dim = 6;
                break;
        }

        initializeBoard();
    }

    // Update is called once per frame
    void Update () {
        if(Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 50)) {
                if (hit.transform.tag == "Tile") {
                    if(hit.transform.parent.gameObject.name == gameObject.name) {
                        moveTile((PuzzleTile)hit.transform.gameObject.GetComponent(typeof(PuzzleTile)));
                    }
                }
            }
        }
    }

    /**
     * Determines whether or not the puzzle is complete. The puzzle tiles must
     * be in sequential order by value.
     * @return true if the puzzle is complete
     */
    public bool isComplete() {
        for(int y = 0, num = 1; y < dim; y++) {
            for(int x = 0; x < dim; x++, num++) {
                if(tiles[x,y] != null) {
                    if(tiles[x,y].getValue() != num) {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    /**
     * Moves the designated tile.
     * @param tile the tile to move
     */
    public void moveTile(PuzzleTile tile) {
        int direction = tile.getDirection();
        int[] tileIndex = tile.getIndex();

        if(direction != 0) {
            tiles[freeIndex[0],freeIndex[1]] = tile;
            tiles[tileIndex[0],tileIndex[1]] = null;

            freeIndex = (int[])tileIndex.Clone();

            updateTileNeighbors(freeIndex);

            switch(direction) {
                case 1:
                    iTween.MoveBy(tile.gameObject,new Vector3(0,-2,0),0.25f);
                    tileIndex[1]--;
                    break;
                case 2:
                    iTween.MoveBy(tile.gameObject,new Vector3(-2,0,0),0.25f);
                    tileIndex[0]--;
                    break;
                case 3:
                    iTween.MoveBy(tile.gameObject,new Vector3(2,0,0),0.25f);
                    tileIndex[0]++;
                    break;
                case 4:
                    iTween.MoveBy(tile.gameObject,new Vector3(0,2,0),0.25f);
                    tileIndex[1]++;
                    break;
            }

            tile.setIndex(tileIndex);
            moves++;
        }
    }

    /**
     * Updates the directions that a group of tiles can move in.
     * @param index the indeces of the center tile
     */
    private void updateTileNeighbors(int[] index) {
        for(int i = 0; i < dim; i++) {
            for(int j = 0; j < dim; j++) {
                if(tiles[i,j] != null) {
                    tiles[i,j].setDirection(0);
                }
            }
        }

        if(index[0] > 0) {
            if(tiles[index[0]-1,index[1]] != null) {
                tiles[index[0]-1,index[1]].setDirection(3);
            }
        }

        if(index[0] < dim-1) {
            if(tiles[index[0]+1,index[1]] != null) {
                tiles[index[0]+1,index[1]].setDirection(2);
            }
        }

        if(index[1] > 0) {
            if(tiles[index[0],index[1]-1] != null) {
                tiles[index[0],index[1]-1].setDirection(4);
            }
        }

        if(index[1] < dim-1) {
            if(tiles[index[0],index[1]+1] != null) {
                tiles[index[0],index[1]+1].setDirection(1);
            }
        }
    }

    /**
     * Initializes the tiles and assigns the tile values at random.
     */
    private void initializeBoard() {
        tiles = new PuzzleTile[dim,dim];
        moves = 0;

        if(dim != 3) {
            List<int> list = new List<int>(dim*dim-1);
            int i;

            for(i = 1; i < dim*dim; i++) {
                list.Add(i);
            }

            for(int y = 0; y < dim; y++) {
                for(int x = 0; x < dim; x++) {
                    if(list.Count > 0) {
                        i = Random.Range(0, list.Count);

                        tiles[x,y] = createTile(x, y);

                        setTileValue(tiles[x,y], list[i]);

                        list.RemoveAt(i);
                    }
                }
            }
        }

        else {
            initializeTutorial();
        }

        freeIndex = new int[2] {dim-1,dim-1};
        updateTileNeighbors(freeIndex);
    }

    /**
     * Resets the board. The current state of the board is discarded, and the
     * tiles reinitialized.
     */
    public void resetBoard() {
        for(int x = 0; x < dim; x++) {
            for(int y = 0; y < dim; y++) {
                if(tiles[x,y] != null) {
                    Destroy(tiles[x,y].gameObject);
                }
            }
        }
        initializeBoard();
    }

    /**
     * Initializes the tutorial board. Only three tiles are out of place.
     */
    private void initializeTutorial() {
        for(int y = 0, i = 1; y < dim; y++) {
            for(int x = 0, j; x < dim && i < 9; x++, i++) {
                tiles[x,y] = createTile(x, y);

                switch(i) {
                    case 5:
                        j = 8;
                        break;
                    case 6:
                        j = 5;
                        break;
                    case 8:
                        j = 6;
                        break;
                    default:
                        j = i;
                        break;
                }

                setTileValue(tiles[x,y], j);
            }
        }
    }

    /**
     * Creates a new tile at the specified location.
     * @param x the x index of the new tile
     * @param y the y index of the new tile
     * @return the new tile
     */
    private PuzzleTile createTile(int x, int y) {
        PuzzleTile newTile = (PuzzleTile)Instantiate(puzzleTile, gameObject.transform.localPosition, gameObject.transform.rotation);
        newTile.transform.parent = gameObject.transform;

        Vector3 pos = newTile.transform.localPosition;
        pos.x += (2f*(float)x - (float)dim+1f) / ((float)dim+1f);
        pos.y += (2f*(float)y - (float)dim+1f) / ((float)dim+1f);
        pos.z += 2.5f;
        newTile.transform.localPosition = pos;

        newTile.setIndex(new int[2] {x,y});

        return newTile;
    }

    /**
     * Initializes the settings of a tile.
     * @param tile the tile to modify
     * @param val the value of the tile
     */
    private void setTileValue(PuzzleTile tile, int val) {
        tile.name = "tile_" + val;
        tile.setValue(val);
    }

    /**
     * Retrieves the tiles that can be moved. The tiles are associated with
     * the cardinal directions in relation to the free slot.
     * @return a Dictionary mapping PuzzleTiles to strings
     */
    public Dictionary<string, PuzzleTile> getFreeTiles() {
        Dictionary <string, PuzzleTile> dict = new Dictionary<string, PuzzleTile>(4);

        if(freeIndex[1] > 0) {
            dict.Add("north", tiles[freeIndex[0],freeIndex[1]-1]);
        }

        if(freeIndex[1] < dim-1) {
            dict.Add("south", tiles[freeIndex[0],freeIndex[1]+1]);
        }

        if(freeIndex[0] > 0) {
            dict.Add("west", tiles[freeIndex[0]-1,freeIndex[1]]);
        }

        if(freeIndex[0] < dim-1) {
            dict.Add("east", tiles[freeIndex[0]+1,freeIndex[1]]);
        }

        return dict;
    }

    public int getMoveCount() {
        return moves;
    }
}
