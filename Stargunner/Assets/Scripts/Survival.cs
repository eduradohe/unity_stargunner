using System.Collections;
using UnityEngine;

public class Survival : MonoBehaviour {

    private int level;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    private GameObject player;
    private GameObject[] enemies;

    private const int maxLevel = 5;
    private const int scoreChanger = 25000;

	// Use this for initialization
	void Start () {
        level = 0;
	    this.enemies = new GameObject[5];
	}
	
	// Update is called once per frame
	void Update () {
        SetPlayer();
        InitiateEnemy();
	}

    private void SetPlayer() {
        if (this.player == null)
        {
            this.player = (GameObject) Instantiate(playerPrefab, Player.playerInitialPos, Quaternion.Euler(55f, 0f, -90f));
        }
    }

    private void InitiateEnemy() {
        if ((Player.score >= (scoreChanger * level * level)) && (enemies[level] == null))
        {
            enemies[level] = (GameObject) Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
            if (level < maxLevel)
            {
                level++;
            }
        }
    }

    
}
