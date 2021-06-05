using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TileMap
{
    private Grid<TileMapObject> grid;
    public event EventHandler OnLoaded;

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
        tilemapVisual.SetGrid(this, grid);
    }

    public void SaveCollisionMap()
    {
        using (var sw = new StreamWriter("./MapConFig/CollisionMap.txt"))
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
        using (var sw = new StreamWriter("./MapConFig/SpriteMap.txt"))
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

        [System.Serializable]
        public class SaveObject
        {
            public TileMapSprite tilemapSprite;
            public int x;
            public int y;
        }

        public SaveObject Save()
        {
            return new SaveObject
            {
                tilemapSprite = tilemapSprite,
                x = x,
                y = y,
            };
        }

        public void Load(SaveObject saveObject)
        {
            tilemapSprite = saveObject.tilemapSprite;
        }
    }

    public class SaveObject
    {
        public TileMapObject.SaveObject[] tilemapObjectSaveObjectArray;
    }

    public void Save()
    {
        List<TileMapObject.SaveObject> tilemapObjectSaveObjectList = new List<TileMapObject.SaveObject>();
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                TileMapObject tilemapObject = grid.GetGridObject(x, y);
                tilemapObjectSaveObjectList.Add(tilemapObject.Save());
            }
        }

        SaveObject saveObject = new SaveObject { tilemapObjectSaveObjectArray = tilemapObjectSaveObjectList.ToArray() };

        Utils.SaveObject(saveObject);
    }

    public void Load()
    {
        SaveObject saveObject = Utils.LoadMostRecentObject<SaveObject>();
        foreach (TileMapObject.SaveObject tilemapObjectSaveObject in saveObject.tilemapObjectSaveObjectArray)
        {
            TileMapObject tilemapObject = grid.GetGridObject(tilemapObjectSaveObject.x, tilemapObjectSaveObject.y);
            tilemapObject.Load(tilemapObjectSaveObject);
        }
        OnLoaded?.Invoke(this, EventArgs.Empty);
    }
}
