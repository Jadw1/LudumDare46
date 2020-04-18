using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

public class WorldScript : MonoBehaviour {
    public Tilemap tilemap;
    public Tile[] groundTiles;
    public Tile[] borderTiles;

    // Start is called before the first frame update
    void Start() {
        tilemap = GetComponentInChildren<Tilemap>();
        
        CreateMap();
    }

    private void CreateMap() {
        Random rng = new Random();

        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < 5; j++) {
                Tile tile = groundTiles[rng.Next(groundTiles.Length)];
                tilemap.SetTile(new Vector3Int(i, j, 0), tile);
            }
        }
        
        Debug.Log("test");
        Debug.Log(tilemap.localBounds.size);
        var teest = tilemap.GetTile(new Vector3Int(10, 10, 0));
    }

}