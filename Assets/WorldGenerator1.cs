using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{

    private int worldWidth;
    private int worldHeight;
    private float chaos;
    private float seed;
    private int[,] heightMap;
    private int maxHeight;

    public WorldGenerator(int width, int height, float chaosFloat, int mountainHeight)
    {
        this.worldWidth = width;
        this.worldHeight = height;
        this.chaos = chaosFloat;
        this.seed = (float)DateTimeOffset.Now.Millisecond;
        this.heightMap = new int[worldWidth, worldHeight];
        this.maxHeight = mountainHeight;
        Array.Clear(heightMap, 0, heightMap.Length);
    }

    public int[,] basicHeightMap()
    {
        for (int z = 0; z < worldWidth; z++)
        {
            for (int x = 0; x < worldHeight; x++)
            {
                float xCoordinate = (((float)x + seed) / (float)worldWidth) * chaos;
                float zCoordinate = (((float)z + seed) / (float)worldHeight) * chaos;
                int cellHeight = (int)Mathf.Floor(Mathf.PerlinNoise(xCoordinate, zCoordinate) * maxHeight);
                heightMap[z,x] = cellHeight;
            }
        }
        return heightMap;
    }


}