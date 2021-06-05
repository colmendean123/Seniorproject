using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    [SerializeField] private TileMapVisual tileMapVisual;
    private TileMap tilemap;
    private TileMap.TileMapObject.TileMapSprite tileMapSprite;
    public int width, height;

    private void Start()
    {
        string[] mapConfig = File.ReadAllText(@"./MapConFig/MapConfig.txt").Split('\n');

        int w, h;
        if (int.TryParse(mapConfig[0], out w))
            width = w;
        if (int.TryParse(mapConfig[1], out h))
            height = h;

        tilemap = new TileMap(width, height, 7.5f, new Vector3(-(width * 3), -(height * 3)));
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
        if (Input.GetKeyDown(KeyCode.Y))
        {
            tileMapSprite = TileMap.TileMapObject.TileMapSprite.BridgeUD;
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            tileMapSprite = TileMap.TileMapObject.TileMapSprite.BridgeLR;
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            tilemap.SaveCollisionMap();
            tilemap.SaveSpriteMap();
            tilemap.Save();
            Utils.TextPopupMouse("Saved Collsion and Sprite Maps!");
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            tilemap.Load();
            Utils.TextPopupMouse("Loaded!");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}
