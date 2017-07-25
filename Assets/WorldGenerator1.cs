using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class WorldGenerator
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
        for(int i = 0; i < worldHeight; i++)
        {
            for(int j = 0; j < worldWidth; j++)
            {
                heightMap[i, j] = 2;
            }
        }
    }

    public int[,] basicHeightMap()
    {
        for (int z = 0; z < worldHeight; z++)
        {
            for (int x = 0; x <  worldWidth; x++)
            {
                float xCoordinate = (((float)x + seed) / (float)worldWidth) * chaos;
                float zCoordinate = (((float)z + seed) / (float)worldHeight) * chaos;
                int cellHeight = (int)Mathf.Floor(Mathf.PerlinNoise(xCoordinate, zCoordinate) * maxHeight);
                heightMap[x, z] = cellHeight;
            }
        }
        return heightMap;
    }

    public int[,] twoContinents()
    {
      
        int padding = worldWidth / 10;
        for(int z = padding; z < (worldHeight/2) + padding; z++)
        {
            for(int x = padding; x < (worldWidth/2); x++)
            {
                heightMap[x, z] = 3;
            }
        }
        for (int z = (worldHeight / 2) - padding; z < worldHeight - padding; z++)
        {
            for (int x = (worldWidth / 2) + padding; x < worldWidth - padding; x++)
            {
                heightMap[x, z] = 3;
            }
        }

        chaos /= 2;
        for (int z = 0; z < worldHeight; z++)
        {
            for (int x = 0; x < worldWidth; x++)
            {
                float xCoordinate = (((float)x + seed) / (float)worldWidth) * chaos;
                float zCoordinate = (((float)z + seed) / (float)worldHeight) * chaos;
                int offset = -1;
                offset += (int) Mathf.Floor(Mathf.PerlinNoise(xCoordinate, zCoordinate)*(maxHeight/2));
                heightMap[x, z] += offset;
            }
        }

        chaos *= 2;
        for (int z = 0; z < worldHeight; z++)
        {
            for (int x = 0; x < worldWidth; x++)
            {
                float xCoordinate = (((float)x + seed) / (float)worldWidth) * chaos;
                float zCoordinate = (((float)z + seed) / (float)worldHeight) * chaos;
                int offset = -2;
                offset += (int)Mathf.Floor(Mathf.PerlinNoise(xCoordinate, zCoordinate) * (maxHeight/2));
                heightMap[x, z] += offset;
            }
        }

        chaos *= 2;
        for (int z = 0; z < worldHeight; z++)
        {
            for (int x = 0; x < worldWidth; x++)
            {
                float xCoordinate = (((float)x + seed) / (float)worldWidth) * chaos;
                float zCoordinate = (((float)z + seed) / (float)worldHeight) * chaos;
                int offset = 0;
                offset += (int)Mathf.Floor(Mathf.PerlinNoise(xCoordinate, zCoordinate) * (maxHeight / 2));
                if (heightMap[x, z] >= 3)
                {
                    heightMap[x, z] += offset;
                }
                else
                {
                    heightMap[x, z] += offset/2;
                }
            }
        }

        for (int z = 0; z < worldHeight; z++)
        {
            for (int x = 0; x < worldWidth; x++)
            {
                if (z < padding || z > worldHeight - padding || x < padding || x > worldWidth - padding && heightMap[x, z] > 2)
                {
                    heightMap[x, z] -= 1;
                }
            }
        }
        for (int z = 0; z < worldHeight; z++)
        {
            for (int x = 0; x < worldWidth; x++)
            {
                if (heightMap[x, z] < 2)
                {
                    heightMap[x, z] = 2;
                }
            }
        }

        System.Random rnd = new System.Random();
        int numberOfRotations= rnd.Next(0, 4);
        for(int i = 0; i < numberOfRotations; i++)
        {
            rotateMap();
        }
        return heightMap;
    }

    public void rotateMap()
    {
        int[,] ret = new int[worldWidth, worldHeight];
        for (int z = 0; z < worldHeight; z++)
        {
            for (int x = 0; x < worldWidth; x++)
            {
                ret[x, z] = heightMap[(worldHeight - z) - 1, x];
            }
        }
        heightMap = ret;
    }

    public int[,] pangea()
    {
        int padding = worldWidth / 5;
        for (int z = padding; z < worldHeight - padding; z++)
        {
            for (int x = padding; x < worldWidth - padding; x++)
            {
                heightMap[x, z] = 3;
            }
        }
        return heightMap;
    }

}