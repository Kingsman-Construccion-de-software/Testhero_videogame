using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesGrid : MonoBehaviour {
    public Enemy[] prefabs;
    public Projectile missilePrefab;
    public int rows = 4;
    public int columns = 5;
    public float gridSpace = 2.0f;
    public int optionsQty = 4;
    public int[,] optionsCoords;
    public int enemiesAmountKilled { get; private set; }
    public int totalEnemies => this.rows * this.columns;
    public int totalEnemiesAlive => this.totalEnemies - this.enemiesAmountKilled;
    public float percentKilled => (float)this.enemiesAmountKilled / (float)this.totalEnemies;
    public Color[] colors = {Color.magenta, Color.green, Color.blue, Color.yellow};
    public float missileAttackRate = 1.0f;

    public SpaceShipGameController controller;

    private void Start() {
        optionsCoords = new int[optionsQty, 2];
        this.generateRandomOptions();

        Debug.Log(controller.ans);

        int colorsCounter = 0;

        for(int row =0;row<this.rows;row++) {
            float width = gridSpace * (this.columns - 1);
            float height = gridSpace * (this.rows - 1);
            Vector2 centering = new Vector2(-width / 2, -height / 2);
            Vector3 rowPos = new Vector3(centering.x, centering.y + (row * gridSpace), 0.0f);

            for(int col=0;col<this.columns;col++) {
                Enemy enemy = Instantiate(this.prefabs[row], this.transform);
                enemy.controller = controller;
                enemy.killed += EnemyKilled;

                for(int i=0;i<optionsQty;i++) {
                    int x = optionsCoords[i, 0];
                    int y = optionsCoords[i, 1];

                    if(x == col && y == row) {
                        enemy.GetComponent<Renderer>().material.color = colors[colorsCounter];
                        if(controller.ans == colorsCounter) {
                            enemy.haveCorrectedAnswer = true;
                        }
                        enemy.colorId = colorsCounter;
                        colorsCounter++;
                        break;
                    }
                }

                Vector3 position = rowPos;
                position.x += col * gridSpace;
                enemy.transform.localPosition = position;
            }
        }

        InvokeRepeating(nameof(MissilAttack), this.missileAttackRate, this.missileAttackRate);
    }

    private bool numberInArray(int number, int[] arr) {
        for(int i=0;i<arr.Length;i++)
            if(arr[i] == number) return true;
        return false;
    }

    private void generateRandomOptions() {
        int[] repeatedCols = {-1, -1, -1, -1};

        for(int i=0;i<optionsQty;i++) {
            int x;

            do x = Random.Range(0, this.columns);
            while(this.numberInArray(x, repeatedCols));

            repeatedCols[i] = x;

            int y = Random.Range(0, this.rows);

            optionsCoords[i, 0] = x;
            optionsCoords[i, 1] = y;
        }
    }

    private void MissilAttack() {
        foreach(Transform enemy in this.transform) {
            if(!enemy.gameObject.activeInHierarchy) {
                continue;
            }

            if(Random.value < (1.0f / (float)this.totalEnemiesAlive)) {
                Instantiate(this.missilePrefab, enemy.position, Quaternion.identity);
                break;
            }
        }
    }

    private void EnemyKilled() {
        this.enemiesAmountKilled++;
    }
}
