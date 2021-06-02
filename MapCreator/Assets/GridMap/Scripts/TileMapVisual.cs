using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapVisual : MonoBehaviour {

    [System.Serializable]
    public struct TileMapSpriteUV
    {
        public TileMap.TileMapObject.TileMapSprite tileMapSprite;
        public Vector2Int uv00Pixels, uv11Pixels;
    }

    private struct UVCoords
    {
        public Vector2 uv00, uv11;
    }

    [SerializeField] private TileMapSpriteUV[] tileMapSpriteUVArray;
    private Grid<TileMap.TileMapObject> grid;
    private Mesh mesh;
    private bool updateMesh;
    private Dictionary<TileMap.TileMapObject.TileMapSprite, UVCoords> uvCoordsDictionary;

    private void Awake() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;


        Texture texture = GetComponent<MeshRenderer>().material.mainTexture;
        float textureWidth = texture.width;
        float textureHeight = texture.height;

        uvCoordsDictionary = new Dictionary<TileMap.TileMapObject.TileMapSprite, UVCoords>();
        foreach (TileMapSpriteUV sprite in tileMapSpriteUVArray)
        {
            uvCoordsDictionary[sprite.tileMapSprite] = new UVCoords
            {
                uv00 = new Vector2(sprite.uv00Pixels.x / textureWidth, sprite.uv00Pixels.y / textureHeight),
                uv11 = new Vector2(sprite.uv11Pixels.x / textureWidth, sprite.uv11Pixels.y / textureHeight),
            };
        }
    }

    public void SetGrid(Grid<TileMap.TileMapObject> grid) {
        this.grid = grid;
        UpdateTileMapVisual();

        grid.OnGridObjectChanged += Grid_OnGridValueChanged;
    }

    private void Grid_OnGridValueChanged(object sender, Grid<TileMap.TileMapObject>.OnGridObjectChangedEventArgs e) {
        updateMesh = true;
    }

    private void LateUpdate() {
        if (updateMesh) {
            updateMesh = false;
            UpdateTileMapVisual();
        }
    }

    private void UpdateTileMapVisual() {
        MeshUtils.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                int index = x * grid.GetHeight() + y;
                Vector3 quadSize = new Vector3(1, 1) * grid.GetCellSize();

                TileMap.TileMapObject gridObject = grid.GetGridObject(x, y);
                TileMap.TileMapObject.TileMapSprite tileMapSprite = gridObject.GetTilemapSprite();

                Vector2 gridValueUV00, gridValueUV11;
                if(tileMapSprite == TileMap.TileMapObject.TileMapSprite.None)
                {
                    gridValueUV00 = Vector2.zero;
                    gridValueUV11 = Vector2.zero;
                    quadSize = Vector3.zero;
                } else {
                    UVCoords uvCoords = uvCoordsDictionary[tileMapSprite];
                    gridValueUV00 = uvCoords.uv00;
                    gridValueUV11 = uvCoords.uv11;
                }

                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, grid.GetWorldPosition(x, y) + quadSize * .5f, 0f, quadSize, gridValueUV00, gridValueUV11);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

}