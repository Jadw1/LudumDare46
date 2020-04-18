using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour {
    public int seed;
    public bool autoUpdate;

    [Range(0, 1)] public float threshold;
    [Range(0, 1)] public float rockThreshold;
    public float scale;

    public int width;
    public int height;

    public TileType[,] map;

    [Range(1, 3)] public int tileSizeScale;
    public TileBase rockTile;
    public TileBase grassTile;
    public TileBase waterTile;

    private Tilemap _rockTilemap;
    private Tilemap _grassTilemap;
    private Tilemap _waterTilemap;

    void Start() {
        var tilemaps = GetComponentsInChildren<Tilemap>();
        foreach (var tilemap in tilemaps) {
            switch (tilemap.name) {
                case "RockLayer":
                    _rockTilemap = tilemap;
                    break;
                case "GrassLayer":
                    _grassTilemap = tilemap;
                    break;
                case "WaterLayer":
                    _waterTilemap = tilemap;
                    break;
            }
        }

        GenerateMap();
    }

    public void GenerateMap() {
        var noiseMap = GenerateNoiseMap();
        map = NormalizePerlinMap(noiseMap);
        UpdateTilemaps();
    }

    private float[,] GenerateNoiseMap() {
        float[,] map = new float[width, height];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                float scaledX = x / scale;
                float scaledY = y / scale;

                float perlinValue = Mathf.PerlinNoise(seed + scaledX, seed + scaledY);
                map[x, y] = perlinValue;
            }
        }

        return map;
    }

    private TileType[,] NormalizePerlinMap(float[,] noiseMap) {
        TileType[,] map = new TileType[width, height];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                map[x, y] = CastNoise(noiseMap[x, y]);
            }
        }

        return map;
    }

    private TileType CastNoise(float noiseValue) {
        if (noiseValue >= threshold) {
            return (noiseValue >= rockThreshold) ? TileType.ROCK : TileType.GRASS;
        }
        else {
            return TileType.WATER;
        }
    }

    private void UpdateTilemaps() {
        _rockTilemap.ClearAllTiles();
        _grassTilemap.ClearAllTiles();
        _waterTilemap.ClearAllTiles();
        
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                TileBase currentTile;
                Tilemap currentTilemap;

                switch (map[x, y]) {
                    case TileType.ROCK:
                        currentTile = rockTile;
                        currentTilemap = _rockTilemap;
                        break;
                    case TileType.GRASS:
                        currentTile = grassTile;
                        currentTilemap = _grassTilemap;
                        break;
                    case TileType.WATER:
                        currentTile = waterTile;
                        currentTilemap = _waterTilemap;
                        break;

                    default:
                        currentTile = waterTile;
                        currentTilemap = _waterTilemap;
                        break;
                }

                SetTile(currentTilemap, x, y, currentTile);
                if (map[x, y] != TileType.GRASS) {
                    ComplementNeighbours(currentTilemap, x, y, currentTile);
                }
            }
        }
    }

    private void SetTile(Tilemap tilemap, int x, int y, TileBase tile) {
        tilemap.SetTile(new Vector3Int(x * tileSizeScale, 0, y * tileSizeScale), tile);
        tilemap.SetTile(new Vector3Int(x * tileSizeScale + 1, 0, y * tileSizeScale), tile);
        tilemap.SetTile(new Vector3Int(x * tileSizeScale, 0, y * tileSizeScale + 1), tile);
        tilemap.SetTile(new Vector3Int(x * tileSizeScale + 1, 0, y * tileSizeScale + 1), tile);
    }

    private void ComplementNeighbours(Tilemap tilemap, int x, int y, TileBase tile) {
        TileType currentTile = map[x, y];
        
        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                if (i != 0 || j != 0) {
                    if (DoComplement(currentTile, map[x+i, y+j])) {
                        SetTile(tilemap, x + i, y + j, tile);
                    }
                }
            }
        }
    }

    private bool DoComplement(TileType currentTile, TileType complementTile) {
        switch (currentTile) {
            case TileType.ROCK:
                return complementTile == TileType.GRASS;
            case TileType.GRASS:
                return false;
            case TileType.WATER:
                return true;
        }

        return false;
    }
}