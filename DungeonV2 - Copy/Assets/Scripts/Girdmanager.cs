﻿using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class Girdmanager : MonoBehaviour
{ 
    public GameObject tilePrefab;
    public TileThemeObject[] themes;
    public int currentTheme;
    public ArrayList spriteList = new ArrayList();
    
    public Cell[,] Grid;
    public int[,] CollisionMap;
    public string[,] spriteMap;

    // Start is called before the first frame update
    void Start()
    {
        //setUpSpriteList();
        setUpCollisionMap();
        writeCollisionMap(CollisionMap);
        setUpMap();
        setupSpriteMap();
        writeSpriteMap(spriteMap);

    }

    public void setUpMap()
    {
        string input = File.ReadAllText(@".\TilemapInput.txt");//change this for correct rows and cols
        Utils.Rows = getRowsfromFile(input);
        Utils.Columns = getColumnsfromFile(input);
        Grid = new Cell[Utils.Rows, Utils.Columns];
        Utils.Vertical = (int)Camera.main.orthographicSize;
        Utils.Horizontal = Utils.Vertical * (Screen.width / Screen.height);
        
        for (int i = 0; i < Utils.Rows; i++)
            for (int j = 0; j < Utils.Columns; j++)
                Grid[i, j] = new Cell(tilePrefab, themes[currentTheme].tiles[getTile(i, j)], i, j); //old way that takes sprites from unity
                //Grid[i, j] = new Cell(tilePrefab, (Sprite)spriteList[getTile(i, j)], i, j);  //new way that dynamicly loads sprites

    }

    public void setUpCollisionMap()
    {
        string input = File.ReadAllText(@".\TilemapInput.txt");//changes this for dif map input
        int r = getRowsfromFile(input);
        int c = getColumnsfromFile(input);
        CollisionMap = new int[r, c];


        int i = 0, j = 0;
        foreach (var row in input.Split('\n'))
        {
            j = 0;
            foreach (var column in row.Trim().Split(','))
            {
                int res;
                if(Int32.TryParse(column.Trim(), out res))
                CollisionMap[i, j] = res;
                j++;
            }
            i++;
        }
    }

    public void UpdateTileTheme(int index)
    {
        currentTheme = index;

        for (int i = 0; i < Utils.Rows; i++)
            for (int j = 0; j < Utils.Columns; j++)
                Grid[i, j].UpdateTile(themes[currentTheme].tiles[getTile(i, j)]);
    }

    public void writeCollisionMap(int[,] cmap)
    {
        int rows = cmap.GetLength(0);
        int cols = cmap.GetLength(1);

        using (var sw = new StreamWriter("TileMapOutput.txt"))
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    sw.Write(cmap[i, j] + ",");
                }
                sw.Write("\n");
            }
            sw.Flush();
            sw.Close();
        }
    }

    private void setupSpriteMap()
    {
        spriteMap = new string[Utils.Rows, Utils.Columns];
        for (int i = 0; i < Utils.Rows; i++)
            for (int j = 0; j < Utils.Columns; j++)
                spriteMap[i, j] = Grid[i, j].renderer.sprite.name;
    }

    //private void setUpSpriteList()
    //{
    //    string input = File.ReadAllText(@".\SpriteList.txt");
    //    string[] paths = input.Split('\n');
    //    foreach (var image in paths)
    //    {
    //        spriteList.Add(LoadSprite(image.Trim()));
    //    }
    //}

    //private Sprite LoadSprite(string path)
    //{
    //    path = "./" + path;
    //    if (string.IsNullOrEmpty(path)) 
    //        return null;
    //    if (System.IO.File.Exists(path))
    //    {
    //        byte[] bytes = System.IO.File.ReadAllBytes(path);
    //        Texture2D texture = new Texture2D(1, 1);
    //        texture.LoadImage(bytes);
    //        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    //        return sprite;
    //    }
    //    return null;
    //}

    private void writeSpriteMap(string[,] sMap)
    {
        int rows = sMap.GetLength(0);
        int cols = sMap.GetLength(1);

        using (var sw = new StreamWriter("SpriteMap.txt"))
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    sw.Write(sMap[i, j] + ", ");
                }
                sw.Write("\n");
            }
            sw.Flush();
            sw.Close();
        }
    }

    public  int getTile(int x, int y)
    {

        if (CollisionMap[x,y] == 1)
            return 0;
        else
            return 1;

    }

    public int getSprite(int x, int y)
    {

        if (CollisionMap[x, y] == 1)
            return 0;
        if (CollisionMap[x, y] == 0)
            return 1;
        else 
        return 2;

    }

    private static int getRowsfromFile(string input)
    {
        string[] rows = input.Split('\n');
        return rows.Length;
    }

    private static int getColumnsfromFile(string input)
    {
        string[] rows = input.Split('\n');
        string[] cols = rows[0].Split(',');
        return cols.Length;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            UpdateTileTheme(currentTheme < themes.Length - 1 ? currentTheme += 1 : 0);
    }
}