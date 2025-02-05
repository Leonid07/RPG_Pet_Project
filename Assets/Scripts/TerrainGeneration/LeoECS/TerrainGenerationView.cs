using Leopotam.Ecs;
using Radar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerationView : MonoBehaviour
{
    public EcsEntity ecsEntity;
    [Header ("Структуры для генерации объектов на местности и присвоение смешанной текстуры")]
    [SerializeField] public ListSpawnTrees[] listSpawnTrees;
    [SerializeField] public ListSpawnRocks[] listSpawnRocks;
    [SerializeField] public ListSpawnGrass[] listSpawnGrass;
    [SerializeField] public AlphaMap[] alphaMap;
    //[SerializeField] public AlphaMapLayer[] alphaMapLayers;
    [Header("Карта высот")]
    [Tooltip("Ширина текстуры")]
    public int textureWidth = 256;
    [Tooltip("Высота текстуры")]
    public int textureHeight = 256;
    [Tooltip("Толщина точек или пути")]
    public int thickness = 5;
    [Tooltip("Разрешение")]
    public int resolution = 100;
    [Tooltip("Амплитуда синусоиды")]
    public float amplitude = 1f;
    [Tooltip("Частота синусоиды")]
    public float frequency = 1f;
    [Tooltip("Интенсивность блюра")]
    public float blurIntensity = 0.5f;
    [Tooltip("Полученная текстура после генерации")]
    public Texture2D texture;
   /* [HideInInspector] */public MeshCollider Water;
    [HideInInspector] public List<Vector2> pathPoints;
    [HideInInspector] public Color[] pixels;
    public Material[] mat;
    public Vector3 size = new Vector3(100, 0, 100);
    [Range(0, 1)]
    public float quality = 0.001f;

    [Header("Генерация местности")]
    [Tooltip("Частота появления холмов")]
    public float HillFrequency = 10.0f;// Частота холмов
    [Tooltip("Минимальная высота холмов")]
    public float LowestHillHeight = 0.5f;// Минимальное значение холмов
    [Tooltip("Максимальная высота холмов")]
    public float HighestHillHeight = 1.5f;// Максимальное значение холмов

    public Texture2D[] textures; // Массив текстур
    public float[] textureHeights; // Высоты соответствующие каждой текстуре

    public float deeps = 2;
    public float perlinWeight = 1f; // Вес для высот шума Перлина
    public float textureWeight = 1f; // Вес для высот из текстуры
    
    public Map Map;
    public Terrain terrain;
    public TerrainData terrainData;

    private void Awake()
    {
        terrainData = terrain.terrainData;
    }

    [System.Serializable]
    public struct AlphaMapLayer
    {
        public Texture2D texture; // текстура      
        [Range(0.0f, 1f)]
        public float transparency; // величина прозрачности    
    }
    [System.Serializable]
    public struct AlphaMap
    {
        public AlphaMapLayer[] layers; // Слои текстур для данного диапазона высот
        public float minHeight; // минимальная высота для применения текстуры
        public float maxHeight; // максимальная высота для применения текстуры
    }
    [System.Serializable]
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
    [System.Serializable]
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
    [System.Serializable]
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
}
