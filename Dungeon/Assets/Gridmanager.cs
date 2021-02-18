using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gridmanager : MonoBehaviour
{
    public GameObject tilePrefab;
    public Sprite[] sprites;
    public float[,] Grid;
    int Vertical, Horizontal, Columns, Rows;

    // Start is called before the first frame update
    void Start()
    {
        Vertical = (int)Camera.main.orthographicSize;
        Horizontal = Vertical * (Screen.width / Screen.height);
        Columns = Horizontal * 2;
        Rows = Vertical * 2;
        Grid = new float[Columns, Rows];

        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                Grid[j, i] = Random.Range(0.0f, 1.0f);
                spawnTile(i, j, Grid[j, i]);
            }
        }
    }

    private Vector3 GridToWorldPosition(int x, int y)
    {
        return new Vector3(x - (Horizontal - 0.5f), y - (Vertical - 0.5f));
    }


    private void spawnTile(int x, int y, float value)
    {
        SpriteRenderer sr = Instantiate(tilePrefab, GridToWorldPosition(x, y), Quaternion.identity).GetComponent<SpriteRenderer>();
        sr.name = "X: " + x + "Y: " + y;
        sr.sprite = sprites[IsEdge(x,y)];
    }

    private int IsEdge(int x, int y)
    {
        if (y == Rows - 1 && x == 0)
            return 0;
        else if (y == Rows - 1 && x != 0 && x != Columns - 1)
            return 1;
        else if (y == Rows - 1 && x == Columns - 1)
            return 2;
        else if (x == 0 && y != 0 && y != Rows - 1)
            return 3;
        else if (x == Columns - 1 && y != 0 && y != Rows - 1)
            return 5;
        else if (x == 0 && y == 0)
            return 6;
        else if (x != 0 && x != Columns - 1 && y == 0)
            return 7;
        else if (x == Columns - 1 && y == 0)
            return 8;
        else
            return 4;

    }
}
