using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesGrid : MonoBehaviour {
    public Enemy[] prefabs;
    public int rows = 4;
    public int columns = 5;
    public float gridSpace = 2.0f;
    public int optionsQty = 4;
    public int[,] optionsCoords;
    public Color[] colors = {Color.magenta, Color.green, Color.blue, Color.yellow};

    private void Awake() {
        optionsCoords = new int[optionsQty, 2];
        this.generateRandomOptions();

        int colorsCounter = 0;
        for(int row =0;row<this.rows;row++) {
            float width = gridSpace * (this.columns - 1);
            float height = gridSpace * (this.rows - 1);
            Vector2 centering = new Vector2(-width / 2, -height / 2);
            Vector3 rowPos = new Vector3(centering.x, centering.y + (row * gridSpace), 0.0f);

            for(int col=0;col<this.columns;col++) {
                Enemy enemy = Instantiate(this.prefabs[row], this.transform);

                for(int i=0;i<optionsQty;i++) {
                    int x = optionsCoords[i, 0];
                    int y = optionsCoords[i, 1];

                    if(x == col && y == row) {
                        enemy.GetComponent<Renderer>().material.color = colors[colorsCounter];
                        colorsCounter++;
                        break;
                    }
                }

                Vector3 position = rowPos;
                position.x += col * gridSpace;
                enemy.transform.localPosition = position;
            }
        }
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

        for(int i=0;i<optionsQty;i++) {
            Debug.Log("Coordenada " + i);
            for(int j=0;j<2;j++) {
                Debug.Log(optionsCoords[i, j]);
            }
        }
    }
}
