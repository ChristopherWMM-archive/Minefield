using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class Tile : MonoBehaviour, IPointerClickHandler {
    public int row, column, adjacentMines, points;
    public bool mine;
    public Sprite[] emptyTextures;
    public Sprite mineImage;
    public Sprite flagImage;

    private GameMaster gameMaster;
    private Sprite defaultImage;
    private Sprite realImage;
    private List<GameObject> adjacentTiles = new List<GameObject>();

    void Start () {
        defaultImage = gameObject.GetComponent<Image>().sprite;
        gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();

        GetRealTileImage();
    }

    public void OnLeftClick() {
        ShowTile();
        ShowAdjacentTiles();

        gameMaster.ChangeTurn();
    }

    public void OnRightClick() {
        SetFlag();

        gameMaster.ChangeTurn();
    }

    public void ShowTile() {
        if(GetComponent<Image>().sprite == defaultImage) {
            SetTileImage(realImage);
            GetComponent<Button>().enabled = false;

            if (mine) {
                gameMaster.GetCurrentPlayer().incorrect = true;
            }

            AddPoints();
        }
    }

    public void ShowAdjacentTiles() {
        if(adjacentMines < 1 && !mine) {
            FloodFillShow(row, column, new bool[GetComponentInParent<Grid>().rows, GetComponentInParent<Grid>().columns]);
        }
    }

    public void FloodFillShow(int x, int y, bool[,] visited) {
        if (x >= 0 && y >= 0 && x < GetComponentInParent<Grid>().rows && y < GetComponentInParent<Grid>().columns) {
            if (visited[x, y]) {
                return;
            }

            visited[x, y] = true;

            GameObject.Find(x + "," + y).GetComponent<Tile>().ShowTile();

            if (GameObject.Find(x + "," + y).GetComponent<Tile>().adjacentMines > 0) {
                return;
            }

            FloodFillShow(x + 1, y + 1, visited);
            FloodFillShow(x + 1, y - 1, visited);
            FloodFillShow(x - 1, y + 1, visited);
            FloodFillShow(x - 1, y - 1, visited);

            FloodFillShow(x + 1, y, visited);
            FloodFillShow(x - 1, y, visited);
            FloodFillShow(x, y + 1, visited);
            FloodFillShow(x, y - 1, visited);
        }
    }

    public void SetFlag() {
        if(GetComponent<Image>().sprite == defaultImage) {
            if (mine) {
                SetTileImage(flagImage);
                AddPoints();

                GetComponent<Button>().enabled = false;
            } else {
                gameMaster.GetCurrentPlayer().incorrect = true;

                ShowTile();
                ShowAdjacentTiles();
            }
        } //else if (GetComponent<Image>().sprite == flagImage) {
        //    SetTileImage(defaultImage);
        //}
    }

    void SetTileImage(Sprite sprite) {
        GetComponent<Image>().sprite = sprite;
    }

    void GetRealTileImage() {
        if (mine) {
            realImage = mineImage;
        } else {
            realImage = emptyTextures[adjacentMines];
        }
    }

    void GetAdjacentTiles() {
        GameObject upLeft = GameObject.Find((row - 1) + "," + (column + 1));
        GameObject up = GameObject.Find((row) + "," + (column + 1));
        GameObject upRight = GameObject.Find((row + 1) + "," + (column + 1));
        GameObject left = GameObject.Find((row - 1) + "," + (column));
        GameObject right = GameObject.Find((row + 1) + "," + (column));
        GameObject downLeft = GameObject.Find((row - 1) + "," + (column - 1));
        GameObject down = GameObject.Find((row) + "," + (column - 1));
        GameObject downRight = GameObject.Find((row + 1) + "," + (column - 1));

        if (adjacentTiles != null) {
            if (upLeft != null) adjacentTiles.Add(upLeft);
            if (up != null) adjacentTiles.Add(up);
            if (upRight != null) adjacentTiles.Add(upRight);
            if (left != null) adjacentTiles.Add(left);
            if (right != null) adjacentTiles.Add(right);
            if (downLeft != null) adjacentTiles.Add(downLeft);
            if (down != null) adjacentTiles.Add(down);
            if (downRight != null) adjacentTiles.Add(downRight);
        } 
    }

    public void GetAdjacentMines() {
        GetAdjacentTiles();
        for (int x = 0; x < adjacentTiles.Count; x++) {
            if (adjacentTiles[x].GetComponent<Tile>().mine) {
                adjacentMines++;
            }
        }
    }

    public void AddPoints() {
        gameMaster.GetCurrentPlayer().AddPoints(points);
    }

    public void CalculatePoints() {
        if (mine) {
            points = 100;
        } else if (adjacentMines > 0) {
            points = 5 * adjacentMines;
        } else {
            points = 1;
        }
    }

    public void OnPointerClick(PointerEventData data) {
        if (data.button == PointerEventData.InputButton.Right) {
            OnRightClick();
        }
    }
}
