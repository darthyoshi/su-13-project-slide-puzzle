using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {
    private PuzzleTile[,] tiles;
    public PuzzleTile puzzleTile;
    private int dim;
    private int[] freeIndex;

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
        if(isComplete()) {
            Debug.Log("game complete");
        }
    }

    bool isComplete() {
        bool status = true;

        for(int i = 0, num = 1; i < dim; i++) {
            for(int j = 0; j < dim; j++, num++) {
                if(tiles[i,j] != null) {
                    if(tiles[i,j].getValue() != num) {
                        status = false;
                        break;
                    }
                }
            }
        }

        return status;
    }

    public void moveTile(PuzzleTile tile) {
        string direction = tile.getDirection();
        int[] tileIndex = tile.getIndex();

        if(direction != "none") {
            updateTileNeighbors(tileIndex);

            tiles[freeIndex[0],freeIndex[1]] = tile;
            tiles[tileIndex[0],tileIndex[1]] = null;

            freeIndex = (int[])tileIndex.Clone();

            updateTileNeighbors(freeIndex);

            switch(direction) {
                case "up":
                    tile.gameObject.transform.Translate(0,-2,0);
                    tileIndex[1]--;
                    break;
                case "left":
                    tile.gameObject.transform.Translate(-2,0,0);
                    tileIndex[0]--;
                    break;
                case "right":
                    tile.gameObject.transform.Translate(2,0,0);
                    tileIndex[0]++;
                    break;
                case "down":
                    tile.gameObject.transform.Translate(0,2,0);
                    tileIndex[1]++;
                    break;
            }

            tile.setIndex(tileIndex);
        }
    }

    public void updateTileNeighbors(int[] index) {
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

    void initializeBoard() {
        tiles = new PuzzleTile[dim,dim];
        Vector3 pos;
        List<int> list = new List<int>(dim*dim-1);
        int i;

        for(i = 1; i < dim*dim; i++) {
            list.Add(i);
        }

        for(int x = 0; x < dim; x++) {
            for(int y = 0; y < dim; y++) {
                if(list.Count > 0) {
                    i = Random.Range(0, list.Count);

                    tiles[x,y] = (PuzzleTile)Instantiate(puzzleTile, gameObject.transform.localPosition, gameObject.transform.rotation);
                    tiles[x,y].transform.parent = gameObject.transform;

                    pos = tiles[x,y].transform.localPosition;
                    pos.x += (2f*(float)x - (float)dim+1f) / ((float)dim+1f);
                    pos.y += (2f*(float)y - (float)dim+1f) / ((float)dim+1f);
                    pos.z += 2.5f;
                    tiles[x,y].transform.localPosition = pos;

                    tiles[x,y].setValue(list[i]);
                    tiles[x,y].name = "tile_" + tiles[x,y].getValue();
                    tiles[x,y].setIndex(new int[2] {x,y});
                    tiles[x,y].renderer.material.color = new Color((float)list[i]/10f, 0.5f, 0.5f, 0f);
                    tiles[x,y].setBoard(this);

                    list.RemoveAt(i);
                }
            }
        }
        freeIndex = new int[2] {dim-1,dim-1};
        updateTileNeighbors(freeIndex);
    }
}
