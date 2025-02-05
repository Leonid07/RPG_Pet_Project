using Radar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TerrainStruct
{
    public ListSpawnTrees[] listSpawnTrees;
    public ListSpawnRocks[] listSpawnRocks;
    public ListSpawnGrass[] listSpawnGrass;
    public AlphaMap[] alphaMap;
    public int textureWidth;
    public int textureHeight;
    public int thickness;
    public int resolution;
    public float amplitude;
    public float frequency;
    public float blurIntensity;
    public Texture2D texture;
    public MeshCollider Water;
    public List<Vector2> pathPoints;
    public Color[] pixels;
    public Material[] mat;
    public Vector3 size;
    public float quality;
    public float HillFrequency;
    public float LowestHillHeight;
    public float HighestHillHeight;
    public Texture2D[] textures;
    public float[] textureHeights;
    public float deeps;
    public float perlinWeight;
    public float textureWeight;
    public Map Map;
    public Terrain terrain;
    public TerrainData terrainData;
}

public struct AlphaMapLayer
{
    public Texture2D texture; // текстура      
    [Range(0.0f, 1f)]
    public float transparency; // величина прозрачности    
}

public struct AlphaMap
{
    public AlphaMapLayer[] layers; // Слои текстур для данного диапазона высот
    public float minHeight; // минимальная высота для применения текстуры
    public float maxHeight; // максимальная высота для применения текстуры
}
public struct ListSpawnTrees
{
    public GameObject Tree;
    public float MinSize;
    public float MaxSize;
    public int NumberOfTrees;
    public bool CreationUnderTheRiver;
    public bool UseMeshCombiner;
    public bool AddMarker;
}
public struct ListSpawnRocks
{
    public GameObject Rocks;
    public float MinSize;
    public float MaxSize;
    public int NumberOfRocks;
    public bool CreationUnderTheRiver;
    public bool UseMeshCombiner;
    public bool AddMarker;
}
public struct ListSpawnGrass
{
    public GameObject Grass;
    public float MinSize;
    public float MaxSize;
    public int NumberOfGrass;
    public bool CreationUnderTheRiver;
    public bool UseMeshCombiner;
    public bool AddMarker;
}