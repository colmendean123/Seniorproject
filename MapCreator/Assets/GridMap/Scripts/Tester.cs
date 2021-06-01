using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    [SerializeField] private TileMapVisual tileMapVisual;
    private TileMap tilemap;
    private TileMap.TileMapObject.TileMapSprite tileMapSprite;

    private void Start()
    {
        tilemap = new TileMap(5, 5, 6f, Vector3.zero);
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
