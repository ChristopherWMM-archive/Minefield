using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour {
    public int rows, columns;
    public int mines;
    public int totalPoints;

    private int totalTiles;

    public GameObject tilePrefab;

    public void PopulateGrid() {
        gameObject.GetComponent<GridLayoutGroup>().constraintCount = columns;
        totalTiles = rows * columns;

        for (int x = 0; x < totalTiles; x++) {
            GameObject gameTile = Instantiate(tilePrefab);

            PrepareTile(x, gameTile);
        }
        
        SetMines();
        SetAdjecentMines();
        SetPoints();
    }

    void PrepareTile(int tileNumber, GameObject tile) {
        tile.transform.SetParent(transform);

        tile.GetComponent<Tile>().row = (tileNumber / columns);
        tile.GetComponent<Tile>().column = (tileNumber % columns);

        tile.name = (tile.GetComponent<Tile>().row + "," + tile.GetComponent<Tile>().column); ;
    }

    void SetMines() {
        int x = 0;
        while(x < mines) {
            int id = Random.Range(0, totalTiles);

            GameObject tile = transform.GetChild(id).gameObject;
            if (!tile.GetComponent<Tile>().mine) {
                tile.GetComponent<Tile>().mine = true;
                x++;
            }
        }
    }

    void SetAdjecentMines() {
        foreach (Tile tile in GetComponentsInChildren<Tile>()) {
            tile.GetAdjacentMines();
        }
    }

    void SetPoints() {
        foreach (Tile tile in GetComponentsInChildren<Tile>()) {
            tile.CalculatePoints();
            totalPoints += tile.points;
        }
    }

    public int GetRemainingMines() {
        int remaining = mines;

        foreach (Tile tile in GetComponentsInChildren<Tile>()) {
            if (tile.mine && tile.gameObject.GetComponent<Image>().sprite == tile.flagImage) {
                remaining--;
            }
        }

        return remaining;
    }

    public int GetUnclickedTiles() {
        int unclicked = 0;

        foreach (Button button in GetComponentsInChildren<Button>()) {
            if (button.enabled) {
                unclicked++;
            }
        }

        return unclicked;
    }
}
