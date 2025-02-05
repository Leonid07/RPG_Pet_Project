using Leopotam.Ecs;
using Radar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerationView : MonoBehaviour
{
    public EcsEntity ecsEntity;
    [Header ("��������� ��� ��������� �������� �� ��������� � ���������� ��������� ��������")]
    [SerializeField] public ListSpawnTrees[] listSpawnTrees;
    [SerializeField] public ListSpawnRocks[] listSpawnRocks;
    [SerializeField] public ListSpawnGrass[] listSpawnGrass;
    [SerializeField] public AlphaMap[] alphaMap;
    //[SerializeField] public AlphaMapLayer[] alphaMapLayers;
    [Header("����� �����")]
    [Tooltip("������ ��������")]
    public int textureWidth = 256;
    [Tooltip("������ ��������")]
    public int textureHeight = 256;
    [Tooltip("������� ����� ��� ����")]
    public int thickness = 5;
    [Tooltip("����������")]
    public int resolution = 100;
    [Tooltip("��������� ���������")]
    public float amplitude = 1f;
    [Tooltip("������� ���������")]
    public float frequency = 1f;
    [Tooltip("������������� �����")]
    public float blurIntensity = 0.5f;
    [Tooltip("���������� �������� ����� ���������")]
    public Texture2D texture;
   /* [HideInInspector] */public MeshCollider Water;
    [HideInInspector] public List<Vector2> pathPoints;
    [HideInInspector] public Color[] pixels;
    public Material[] mat;
    public Vector3 size = new Vector3(100, 0, 100);
    [Range(0, 1)]
    public float quality = 0.001f;

    [Header("��������� ���������")]
    [Tooltip("������� ��������� ������")]
    public float HillFrequency = 10.0f;// ������� ������
    [Tooltip("����������� ������ ������")]
    public float LowestHillHeight = 0.5f;// ����������� �������� ������
    [Tooltip("������������ ������ ������")]
    public float HighestHillHeight = 1.5f;// ������������ �������� ������

    public Texture2D[] textures; // ������ �������
    public float[] textureHeights; // ������ ��������������� ������ ��������

    public float deeps = 2;
    public float perlinWeight = 1f; // ��� ��� ����� ���� �������
    public float textureWeight = 1f; // ��� ��� ����� �� ��������
    
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
        public Texture2D texture; // ��������      
        [Range(0.0f, 1f)]
        public float transparency; // �������� ������������    
    }
    [System.Serializable]
    public struct AlphaMap
    {
        public AlphaMapLayer[] layers; // ���� ������� ��� ������� ��������� �����
        public float minHeight; // ����������� ������ ��� ���������� ��������
        public float maxHeight; // ������������ ������ ��� ���������� ��������
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
