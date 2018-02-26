using UnityEngine;

public class Player : MonoBehaviour {
    public string name;
    public int score;
    public bool incorrect;

	void Start() {
        score = 0;
	}

    public void AddPoints(int points) {
        if (!incorrect) {
            if (points > 0) {
                score += points;
            } else /* if (-points < score) */ {
                score += points;
            }
            //else {
            //    score = 0;
            //}
        } else {
            //if (points < score) {
                score += -points;
            //} else {
            //    score = 0;
            //}
        }
    }
}
