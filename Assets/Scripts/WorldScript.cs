using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldScript : MonoBehaviour {
    public Tilemap tilemap;

    // Start is called before the first frame update
    void Start() {
        tilemap = GetComponent<Tilemap>();
        
        Vector3Int
        tilemap.SetTile();
    }

    // Update is called once per frame
    void Update() {
        
    }
}