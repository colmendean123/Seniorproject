using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] private TileMapVisual tileMapVisual;
    private TileMap tilemap;
    private TileMap.TileMapObject.TileMapSprite tileMapSprite;

    private void Start()
    {
        int width = 0, height = 0;
        width = 15; height = 15;
        tilemap = new TileMap(width, height, 7.5f, new Vector3(-(width *3), -(height * 3)));
        tilemap.setTileMapVisual(tileMapVisual);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Utils.GetMouseWorldPosition();
            tilemap.SetTileMapSprite(mousePosition, tileMapSprite);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            tileMapSprite = TileMap.TileMapObject.TileMapSprite.None;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            tileMapSprite = TileMap.TileMapObject.TileMapSprite.Wall;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            tileMapSprite = TileMap.TileMapObject.TileMapSprite.Dirt;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            tileMapSprite = TileMap.TileMapObject.TileMapSprite.Grass;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            tileMapSprite = TileMap.TileMapObject.TileMapSprite.Water;
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            tilemap.SaveCollisionMap();
            Utils.TextPopupMouse("Saved Collision Map");
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            tilemap.SaveSpriteMap();
            Utils.TextPopupMouse("Saved Sprite Map");
        }
    }

}
