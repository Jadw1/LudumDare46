using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

public class ChunkGenerator : MonoBehaviour {
    public string seed;
    public bool useRandomSeed;
    
    public Vector2Int chunkSize;
    
    [Range(0, 100)]
    public int fillPercentage;
    public int smoothIterations;
    [Range(0, 8)]
    public int fillTreshhold;

    private Tilemap _tilemap;
    private int[,] _map;
    
    public RuleTile ruleTile;
    
    
    void Start() {
        _tilemap = GetComponentInChildren<Tilemap>();
        _map = new int[chunkSize.x, chunkSize.y];
        
        Generate();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Generate();
        }
    }

    private void Generate() {
        if (useRandomSeed) {
            seed = System.DateTime.Now.Ticks.ToString();
        }
        
        Debug.Log("Filling map randomly");
        FillMap();
        for (int i = 0; i < smoothIterations; i++) {
            Debug.Log("Smoothing iteration " + i);
            SmoothMap();
        }
        
        Debug.Log("Updating tilemap");
        UpdateTilemap();
    }
    
    private void FillMap() {
        Random rng = new Random(seed.GetHashCode());
        for (int x = 0; x < chunkSize.x; x++) {
            for (int y = 0; y < chunkSize.y; y++) {
                _map[x, y] = (rng.Next(100) < fillPercentage) ? 1 : 0;
            }
        }
    }

    private int GetEmptyNeighbour(int x, int y) {
        int count = 0;

        for (int i = x - 1; i <= x + 1; i++) {
            for (int j = y - 1; j <= y + 1; j++) {
                if ((i != x || j != y) && (i >= 0 && i < chunkSize.x && j >= 0 && j < chunkSize.y)) {
                    count += _map[i, j];
                }
            }   
        }

        return count;
    }

    private void SmoothMap() {
        for (int x = 0; x < chunkSize.x; x++) {
            for (int y = 0; y < chunkSize.y; y++) {
                int empty = GetEmptyNeighbour(x, y);

                if (empty > fillTreshhold) {
                    _map[x, y] = 1;
                }
                else if (empty < fillTreshhold) {
                    _map[x, y] = 0;
                }
            }
        }
    }

    private void UpdateTilemap() {
        _tilemap.ClearAllTiles();
        
        for (int x = 0; x < chunkSize.x; x++) {
            for (int y = 0; y < chunkSize.y; y++) {
                if (_map[x, y] != 0) {
                    SetTile(x, y);
                }
            }
        }
    }

    private void SetTile(int x, int y) {
        _tilemap.SetTile(new Vector3Int(2 * x, 2 * y, 0), ruleTile);
        _tilemap.SetTile(new Vector3Int(2 * x + 1, 2 * y, 0), ruleTile);
        _tilemap.SetTile(new Vector3Int(2 * x, 2 * y + 1, 0), ruleTile);
        _tilemap.SetTile(new Vector3Int(2 * x + 1, 2 * y + 1, 0), ruleTile);
    }
}