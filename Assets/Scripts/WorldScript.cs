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
    public RuleTile ruleTile;

    // Start is called before the first frame update
    void Start() {
        tilemap = GetComponentInChildren<Tilemap>();
        
        CreateMap();
    }

    private void CreateMap() {
        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < 5; j++) {
                tilemap.SetTile(new Vector3Int(i, j, 0), ruleTile);
            }
        }
    }

}