using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TileMap
{
    private Grid<TileMapObject> grid;

    public TileMap(int width, int height, float cellSize, Vector3 originPosoition)
    {
        grid = new Grid<TileMapObject>(width, height, cellSize, originPosoition, (Grid<TileMapObject> g, int x, int y) => new TileMapObject(g, x, y));
    }

    public void SetTileMapSprite(Vector3 worldPosition, TileMapObject.TileMapSprite tilemapSprite)
    {
        TileMapObject tilemapObject = grid.GetGridObject(worldPosition);
        if (tilemapObject != null)
        {
            tilemapObject.SetTileMapSprite(tilemapSprite);
        }
    }
    public void setTileMapVisual(TileMapVisual tilemapVisual)
    {
        tilemapVisual?.SetGrid(grid);
    }

    public void SaveCollisionMap()
    {
        using (var sw = new StreamWriter("CollisionMap.txt"))
        {
            for (int i = 0; i < grid.GetWidth(); i++)
            {
                for (int j = 0; j < grid.GetHeight(); j++)
                {
                    TileMapObject tMapObj = grid.GetGridObject(i, j);
                    if (tMapObj.GetTilemapSprite() == TileMapObject.TileMapSprite.None)
                        break;
                    else
                    {
                        if (tMapObj.GetTilemapSprite() == TileMapObject.TileMapSprite.Wall)
                            sw.Write("1" + ",");
                        else
                            sw.Write("0" + ",");
                    }
                }
                sw.Write("\n");
            }
            sw.Flush();
            sw.Close();
        }
    }

    public void SaveSpriteMap()
    {
        using (var sw = new StreamWriter("SpriteMap.txt"))
        {
            for (int i = 0; i < grid.GetWidth(); i++)
            {
                for (int j = 0; j < grid.GetHeight(); j++)
                {
                    TileMapObject tMapObj = grid.GetGridObject(i, j);
                    
                    if (tMapObj.GetTilemapSprite() == TileMapObject.TileMapSprite.None)
                        break;
                        sw.Write(tMapObj.ToString() + ",");
                }
                sw.Write("\n");
            }
            sw.Flush();
            sw.Close();
        }
    }

    public class TileMapObject
    {
        public enum TileMapSprite
        {
            None,
            Wall,
            Dirt,
            Grass,
            Water,
            BridgeUD,
            BridgeLR
        }

        private Grid<TileMapObject> grid;
        private int x;
        private int y;
        private TileMapSprite tilemapSprite;

        public TileMapObject(Grid<TileMapObject> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void SetTileMapSprite(TileMapSprite tilemapSprite)
        {
            this.tilemapSprite = tilemapSprite;
            grid.TriggerGridObjectChanged(x, y);
        }
        public TileMapSprite GetTilemapSprite()
        {
            return tilemapSprite;
        }

        public override string ToString()
        {
            return tilemapSprite.ToString();
        }

        //public class SaveObject
        //{
        //    public TileMapSprite tileMapSprite;
        //    public int x, y;
        //}

        //public SaveObject Save()
        //{
        //    return new SaveObject
        //    {
        //        tileMapSprite = tilemapSprite,
        //        x = x,
        //        y = y
        //    };
        //}
    }
}
