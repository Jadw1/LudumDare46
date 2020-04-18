using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour {

    public bool autoUpdate;
    public int seed;
    public float scale;
    [Range(0, 1)]
    public float threshold;
    [Range(0, 1)]
    public float rockThreshold;

    public int width;
    public int height;
    
    public float[,] map;
    void Start() {
        GenerateMap();
    }

    public void GenerateMap() {
        map = GenerateNoiseMap(width, height, scale);
    }

    private float[,] GenerateNoiseMap(int width, int height, float scale) {
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

    private void OnDrawGizmos() {
        if (map != null) {
            
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    if (map[x, y] >= threshold) {
                        Gizmos.color = (map[x, y] >= rockThreshold) ? Color.black : Color.white;
                        Vector3 pos= new Vector3(x, 0, y);
                        Gizmos.DrawCube(pos, Vector3.one);
                    }
                }
            }
        }
    }
}