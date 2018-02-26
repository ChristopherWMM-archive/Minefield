using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameMaster : MonoBehaviour {
    public int rows, columns, mines, totalPoints;
    public bool gameOver;
    public List<Player> players = new List<Player>();
    public int turn;
    public Grid grid;

	void Start() {
        StartGame();
	}

    void StartGame() {
        SetupPlayers();
        SetupMap();
    }

    void SetupMap() {
        grid.rows = rows;
        grid.columns = columns;
        grid.mines = mines;
        grid.PopulateGrid();

        totalPoints = grid.totalPoints;
    }

    void SetupPlayers() {
        foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(player.GetComponent<Player>());
        }
    }

    public void ChangeTurn() {
        GameOverCheck();

        if (gameOver) {
            GameOver();
        } else {
            turn++;
            turn %= players.Count;
            players[turn].incorrect = false;
        }
    }

    public Player GetCurrentPlayer() {
        return players[turn];
    }

    public void GameOverCheck() {
        if (grid.GetUnclickedTiles() < 1 || grid.GetRemainingMines() < 1) {
            gameOver = true;
        }
    }

    public void GameOver() {
        Player winner = GetWinningPlayer();
        string winningName = winner.name;
        int winningScore = winner.score;

        foreach (Tile tile in grid.gameObject.GetComponentsInChildren<Tile>()) {
            if (tile.gameObject.GetComponent<Button>().enabled) {
                tile.AddPoints();
                tile.ShowTile();
            }
        }

        print("Game Over!");
        if (winningScore >= totalPoints) {
            print(winningName + " is the winner with a perfect score of: " + winningScore);
        } else {
            print(winningName + " is the winner with a score of: " + winningScore + " out of " + totalPoints);
        }
    }

    public Player GetWinningPlayer() {
        Player winner = null;

        foreach (Player player in players) {
            if (winner == null) {
                winner = player;
            } else if (player.score > winner.score) {
                winner = player;
            }
        }

        return winner;
    }
}
