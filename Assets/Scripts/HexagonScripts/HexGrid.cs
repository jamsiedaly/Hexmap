﻿using UnityEngine.UI;
using UnityEngine;
using System;

public class HexGrid : MonoBehaviour
{

    public int chunkCountX = 4, chunkCountZ = 3;
    int cellCountX, cellCountZ;
    float chaos = 8.0f;

    public HexCell cellPrefab;
    public Color defaultColor = Color.white;

    HexCell[] cells;

    public Text cellLabelPrefab;

    public HexGridChunk chunkPrefab;

    HexGridChunk[] chunks;

    void Awake()
    {
        cellCountX = chunkCountX * HexMetrics.chunkSizeX;
        cellCountZ = chunkCountZ * HexMetrics.chunkSizeZ;

        GenerateGrid();
    }

    public void GenerateGrid()
    {
        CreateChunks();
        CreateCells();
    }

    public void RegenerateGrid()
    {
        foreach (HexGridChunk chunk in chunks)
        {
            Destroy(chunk.gameObject);
        }
        GenerateGrid();
    }

    void CreateChunks()
    {
        chunks = new HexGridChunk[chunkCountX * chunkCountZ];
        for (int z = 0, i = 0; z < chunkCountZ; z++)
        {
            for (int x = 0; x < chunkCountX; x++)
            {
                HexGridChunk chunk = chunks[i++] = Instantiate(chunkPrefab);
                chunk.transform.SetParent(transform);
            }
        }
    }


    void CreateCells()
    {
        int mountainHeight = 8;
        cells = new HexCell[cellCountZ * cellCountX];
        WorldGenerator myGenerator = new WorldGenerator(cellCountX, cellCountZ, chaos, mountainHeight);
        int[,] heightMap = myGenerator.twoContinents();
        for (int z = 0, i = 0; z < cellCountZ; z++)
        {
            for (int x = 0; x < cellCountX; x++)
            {
                CreateCell(x, z, i++, heightMap[x, z]);
            }
        }
    }

    void CreateCell(int x, int z, int i, int height)
    {
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        //cell.color = defaultColor;


        if (x > 0)
        {
            cell.SetNeighbor(HexDirection.W, cells[i - 1]);
        }
        if (z > 0)
        {
            if ((z & 1) == 0)
            {
                cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX]);
                if (x > 0)
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX - 1]);
                }
            }
            else
            {
                cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX]);
                if (x < cellCountX - 1)
                {
                    cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX + 1]);
                }
            }
        }

        //Text label = Instantiate<Text>(cellLabelPrefab);
        //label.rectTransform.anchoredPosition =
        //    new Vector2(position.x, position.z);
        //label.text = cell.coordinates.ToStringOnSeparateLines();

        //cell.uiRect = label.rectTransform;

        cell.Elevation = height;

        AddCellToChunk(x, z, cell);
    }

    void AddCellToChunk(int x, int z, HexCell cell)
    {
        int chunkX = x / HexMetrics.chunkSizeX;
        int chunkZ = z / HexMetrics.chunkSizeZ;
        HexGridChunk chunk = chunks[chunkX + chunkZ * chunkCountX];

        int localX = x - chunkX * HexMetrics.chunkSizeX;
        int localZ = z - chunkZ * HexMetrics.chunkSizeZ;
        chunk.AddCell(localX + localZ * HexMetrics.chunkSizeX, cell);
    }

    public HexCell GetCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * cellCountX + coordinates.Z / 2;
        if (index > 0 && index < cellCountX * cellCountZ)
            return cells[index];
        else
            return null;
    }

}