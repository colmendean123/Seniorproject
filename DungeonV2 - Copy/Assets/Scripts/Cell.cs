using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Cell
{
    public GameObject gameObject;
    public SpriteRenderer renderer;
    public int X;
    public int Y;

    public Cell(GameObject prefab, Sprite sprite, int x, int y)
    {
        X = x;
        Y = y;
        gameObject = GameObject.Instantiate(prefab, Utils.GridToWorldPosition(x, y), Quaternion.identity);
        renderer = gameObject.GetComponent<SpriteRenderer>();
        gameObject.name = "X: " + x + "Y: " + y;
        renderer.sortingLayerName = "Map";
        renderer.sprite = sprite;
    }

    public void UpdateTile(Sprite sprite)
    {
        renderer.sprite = sprite;
    }
}
