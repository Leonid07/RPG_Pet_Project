using Radar;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GenerateTerrain
{
    public class GenerateTerrain : MonoBehaviour
    {
        [SerializeField]
        public ListSpawnTrees[] listSpawnTrees;
        [SerializeField]
        public ListSpawnRocks[] listSpawnRocks;
        [SerializeField]
        public ListSpawnGrass[] listSpawnGrass;
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

        public Texture2D heightMap;
        public Map Map;
        public AlphaMap[] alphaMap;
        private MeshCollider Water;
        private HeightMapGeneration heightMapGeneration;
        private Terrain terrain;
        private TerrainData terrainData;

        [System.Obsolete]
        private void Start()
        {
            heightMapGeneration = GetComponent<HeightMapGeneration>();
            Water = heightMapGeneration.Water;
            terrain = GetComponent<Terrain>();
            this.heightMap = heightMapGeneration.texture;
            Texture2D heightMap = heightMapGeneration.texture;

            // Получите ссылку на TerrainData
            terrainData = terrain.terrainData;

            int width = terrainData.heightmapWidth;
            int height = terrainData.heightmapHeight;
            float[,] heights = new float[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    heights[x, y] = 0f;
                }
            }
            terrainData.SetHeights(0, 0, heights);
            terrainData.SetHeights(0, 0, PerlinNoise(terrain, HillFrequency));
            terrainData.SetHeights(0, 0, CombineHeights(PerlinNoise(terrain, HillFrequency), GetHeightsFromTexture(heightMap, terrainData.heightmapWidth, terrainData.heightmapHeight), terrainData));

            CreateTextureLayers();
            ApplyTextures();

            SpawnObjectsOnTerrain();
        }

        #region // Создание холмов и применение карты высот для реки
        [System.Obsolete]
        public float[,] PerlinNoise(Terrain terrain, float tileSize)
        {
            Debug.Log($"Минимальная высота холмов {LowestHillHeight} Максимальная высота холмов {HighestHillHeight}");
            float RandomHeightOfHills = Random.Range(LowestHillHeight, HighestHillHeight);
            Debug.Log($"Рандомная высота холмов {RandomHeightOfHills}");
            float hillHeight = (float)RandomHeightOfHills / ((float)terrain.terrainData.heightmapHeight / 2);
            float baseHeight = LowestHillHeight / ((float)terrain.terrainData.heightmapHeight / 2);
            float[,]  heights = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];

            for (int i = 0; i < terrain.terrainData.heightmapWidth; i++)
            {
                for (int k = 0; k < terrain.terrainData.heightmapHeight; k++)
                {
                    heights[i, k] = baseHeight + (Mathf.PerlinNoise((float)i / terrain.terrainData.heightmapWidth * tileSize, (float)k / terrain.terrainData.heightmapHeight * tileSize) * (float)hillHeight);
                }
            }

            return heights;
        }

        private float[,] GetHeightsFromTexture(Texture2D texture, int width, int height)
        {
            float deep = deeps;
            float depthDifference, grayscaleValue, heightValue;
            float[,] heightsForTexture = terrain.terrainData.GetHeights(0, 0, width, height);

            // Преобразуйте значения цветов пикселей текстуры в высоты только для рек
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // Получите значение яркости (оттенка серого) пикселя
                    grayscaleValue = texture.GetPixel(x, y).grayscale;

                    // Преобразуйте значение яркости в диапазон высот от 0 до 1
                    heightValue = grayscaleValue / 255f;

                    // Примените высоту только для рек, оставив холмы без изменений
                    if (heightValue < heightsForTexture[x, y])
                    {
                        // Уменьшите высоту углублений в два раза
                        depthDifference = heightsForTexture[x, y] - heightValue;
                        heightsForTexture[x, y] = heightsForTexture[x, y] - (depthDifference * deep);
                    }
                }
            }

            return heightsForTexture;
        }

        private float[,] CombineHeights(float[,] perlinHeights, float[,] textureHeights, TerrainData terrainData)
        {
            int width = perlinHeights.GetLength(0);
            int height = perlinHeights.GetLength(1);
            float[,] combinedHeights = new float[width, height];
            float combinedHeight;

            // Объединение высот из разных методов (например, среднее значение)
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    combinedHeight = (perlinHeights[x, y] * perlinWeight + textureHeights[x, y] * textureWeight) / (perlinWeight + textureWeight);

                    combinedHeights[x, y] = combinedHeight;
                }
            }
            return combinedHeights;
        }
        #endregion

        #region // Создание слоёв текстур и их применение на местности

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

        public void CreateTextureLayers()
        {
            int totalTextureCount = 0;
            for (int i = 0; i < alphaMap.Length; i++)
            {
                totalTextureCount += alphaMap[i].layers.Length;
            }

            TerrainLayer[] terrainLayers = new TerrainLayer[totalTextureCount];
            int layerIndex = 0;

            for (int j = 0; j < alphaMap.Length; j++)
            {
                for (int i = 0; i < alphaMap[j].layers.Length; i++)
                {
                    TerrainLayer terrainLayer = new TerrainLayer();
                    terrainLayer.diffuseTexture = alphaMap[j].layers[i].texture;
                    terrainLayer.tileSize = new Vector2(10, 10);
                    terrainLayer.diffuseRemapMin = new Vector4(0, 0, 0, 0);
                    terrainLayer.diffuseRemapMax = new Vector4(1, 1, 1, 1);

                    terrainLayers[layerIndex] = terrainLayer;
                    layerIndex++;
                }
            }

            terrainData.terrainLayers = terrainLayers;
        }

        public void ApplyTextures()
        {
            int textureCount = 0;
            foreach (AlphaMap alphaMapInfo in alphaMap)
            {
                textureCount += alphaMapInfo.layers.Length;
            }

            float[,,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, textureCount];

            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {
                for (int y = 0; y < terrainData.alphamapHeight; y++)
                {
                    float height = terrainData.GetHeight(y, x);

                    int layerIndex = 0;
                    for (int i = 0; i < alphaMap.Length; i++)
                    {
                        AlphaMap alphaMapInfo = alphaMap[i];

                        if (height >= alphaMapInfo.minHeight && height <= alphaMapInfo.maxHeight)
                        {
                            for (int j = 0; j < alphaMapInfo.layers.Length; j++)
                            {
                                if (layerIndex < textureCount)
                                {
                                    splatmapData[x, y, layerIndex] = alphaMapInfo.layers[j].transparency;
                                }
                                layerIndex++;
                            }
                        }
                        else
                        {
                            // Пропускать слои, выходящие за пределы диапазона высот.
                            layerIndex += alphaMapInfo.layers.Length;
                        }
                    }
                }
            }

            // Установите данные splatmap на местность
            terrainData.SetAlphamaps(0, 0, splatmapData);
        }
        #endregion

        #region // генерация объектов на terrain
        private void SpawnObjectsOnTerrain()
        {
            TerrainData terrainData = terrain.terrainData;
            float terrainWidth = terrainData.size.x;
            float terrainLength = terrainData.size.z;
            float terrainPosX = terrain.transform.position.x;
            float terrainPosZ = terrain.transform.position.z;

            #region // Создание деревьев на местности и использование Mesh Combiner
            GameObject trees = new GameObject("Trees");
            trees.AddComponent<MeshFilter>();
            trees.AddComponent<MeshRenderer>();
            trees.transform.SetParent(terrain.transform);

            // Цикл создания деревьев на местности
            foreach (ListSpawnTrees spawnTree in listSpawnTrees)
            {
                List<GameObject> spawnedObjects = new List<GameObject>(); // Список для хранения созданных объектов

                if (spawnTree.CreationUnderTheRiver == true)
                {
                    Water.isTrigger = false;
                }

                for (int i = 0; i < spawnTree.NumberOfTrees; i++)
                {
                    GameObject prefab = spawnTree.Tree;

                    // Попытайтесь найти допустимую позицию появления (не сталкиваясь с другими объектами).
                    Vector3 randomPosition = GetRandomTerrainPosition(terrainPosX, terrainPosZ, terrainWidth, terrainLength);
                    int maxAttempts = 10;
                    int attempts = 0;
                    while (IsPositionColliding(randomPosition) && attempts < maxAttempts)
                    {
                        randomPosition = GetRandomTerrainPosition(terrainPosX, terrainPosZ, terrainWidth, terrainLength);
                        attempts++;
                    }

                    if (attempts < maxAttempts)
                    {
                        // Создайте экземпляр объекта в случайной позиции
                        GameObject instance = Instantiate(prefab, randomPosition, Quaternion.identity);

                        if (spawnTree.MinSize != 0 && spawnTree.MaxSize != 0)
                        {
                            float randomScale = Random.Range(spawnTree.MinSize, spawnTree.MaxSize);
                            instance.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
                        }
                        instance.transform.Rotate(0, Random.Range(0, 360), 0);

                        if (spawnTree.UseMeshCombiner == false)
                        {
                            instance.transform.SetParent(terrain.transform);
                        }
                        else
                        {
                            instance.transform.SetParent(trees.transform);
                        }
                        if (spawnTree.AddMarker)
                            Map.AddMarkerObjectStatic(instance);
                        spawnedObjects.Add(instance); // Добавляем объект в список
                    }
                }
                Water.isTrigger = true;
            }
            #endregion
            #region // Создание камней на местности и использование Mesh Combiner
            GameObject rocks = new GameObject("Rocks");
            rocks.AddComponent<MeshFilter>();
            rocks.AddComponent<MeshRenderer>();
            rocks.transform.SetParent(terrain.transform);

            // Цикл создания камней на местности
            foreach (ListSpawnRocks spawnRocks in listSpawnRocks)
            {
                List<GameObject> spawnedObjects = new List<GameObject>(); // Список для хранения созданных объектов
                if (spawnRocks.CreationUnderTheRiver == true)
                {
                    Water.isTrigger = false;
                }
                for (int i = 0; i < spawnRocks.NumberOfRocks; i++)
                {
                    GameObject prefab = spawnRocks.Rocks;

                    // Попытайтесь найти допустимую позицию появления (не сталкиваясь с другими объектами).
                    Vector3 randomPosition = GetRandomTerrainPosition(terrainPosX, terrainPosZ, terrainWidth, terrainLength);
                    int maxAttempts = 10;
                    int attempts = 0;
                    while (IsPositionColliding(randomPosition) && attempts < maxAttempts)
                    {
                        randomPosition = GetRandomTerrainPosition(terrainPosX, terrainPosZ, terrainWidth, terrainLength);
                        attempts++;
                    }

                    if (attempts < maxAttempts)
                    {
                        // Создайте экземпляр объекта в случайной позиции
                        GameObject instance = Instantiate(prefab, randomPosition, Quaternion.identity);

                        if (spawnRocks.MinSize != 0 && spawnRocks.MaxSize != 0)
                        {
                            float randomScale = Random.Range(spawnRocks.MinSize, spawnRocks.MaxSize);
                            instance.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
                        }
                        instance.transform.Rotate(0, Random.Range(0, 360), 0);

                        if (spawnRocks.UseMeshCombiner == false)
                        {
                            instance.transform.SetParent(terrain.transform);
                        }
                        else
                        {
                            instance.transform.SetParent(rocks.transform);
                        }
                        if (spawnRocks.AddMarker)
                            Map.AddMarkerObjectStatic(instance);
                        spawnedObjects.Add(instance); // Добавляем объект в список
                    }
                }
                Water.isTrigger = true;
            }
            #endregion
            #region // Создание травы на местности и использование Mesh Combiner
            GameObject grass = new GameObject("Grass");
            grass.AddComponent<MeshFilter>();
            grass.AddComponent<MeshRenderer>();
            grass.transform.SetParent(terrain.transform);

            // Цикл создания травы на местности
            foreach (ListSpawnGrass spawnGrass in listSpawnGrass)
            {
                List<GameObject> spawnedObjects = new List<GameObject>(); // Список для хранения созданных объектов

                if (spawnGrass.CreationUnderTheRiver == true)
                {
                    Water.isTrigger = false;
                }

                for (int i = 0; i < spawnGrass.NumberOfGrass; i++)
                {
                    GameObject prefab = spawnGrass.Grass;

                    // Попытка найти допустимую позицию появления (не сталкиваясь с другими объектами)
                    Vector3 randomPosition = GetRandomTerrainPosition(terrainPosX, terrainPosZ, terrainWidth, terrainLength);
                    int maxAttempts = 10;
                    int attempts = 0;
                    while (IsPositionColliding(randomPosition) && attempts < maxAttempts)
                    {
                        randomPosition = GetRandomTerrainPosition(terrainPosX, terrainPosZ, terrainWidth, terrainLength);
                        attempts++;
                    }

                    if (attempts < maxAttempts)
                    {
                        // Создаём экземпляр объекта в случайной позиции
                        GameObject instance = Instantiate(prefab, randomPosition, Quaternion.identity);

                        if (spawnGrass.MinSize != 0 && spawnGrass.MaxSize != 0)
                        {
                            float randomScale = Random.Range(spawnGrass.MinSize, spawnGrass.MaxSize);
                            instance.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
                        }

                        instance.transform.Rotate(0, Random.Range(0, 360), 0);

                        if (spawnGrass.UseMeshCombiner == false)
                        {
                            instance.transform.SetParent(terrain.transform);
                        }
                        else
                        {
                            instance.transform.SetParent(grass.transform);
                        }
                        if (spawnGrass.AddMarker)
                            Map.AddMarkerObjectStatic(instance);
                        spawnedObjects.Add(instance); // Добавляем объект в список
                    }
                }
                Water.isTrigger = true;
            }
            #endregion
            Map.AddMarkerStaticObject();
            MeshCombiner(grass);
            MeshCombiner(rocks);
            MeshCombiner(rocks);
        }
        void MeshCombiner(GameObject MeshGameObject)
        {
            MeshCombiner meshCombiner = MeshGameObject.AddComponent<MeshCombiner>();
            meshCombiner.CreateMultiMaterialMesh = true;
            meshCombiner.DestroyCombinedChildren = true;
            meshCombiner.CombineMeshes(false);
        }
        private Vector3 GetRandomTerrainPosition(float terrainPosX, float terrainPosZ, float terrainWidth, float terrainLength)
        {
            float randomX = Random.Range(0f, terrainWidth);
            float randomZ = Random.Range(0f, terrainLength);
            float y = terrain.SampleHeight(new Vector3(randomX + terrainPosX, 0, randomZ + terrainPosZ));
            y += terrain.transform.position.y;
            return new Vector3(randomX + terrainPosX, y, randomZ + terrainPosZ);
        }

        private bool IsPositionColliding(Vector3 position)
        {
            float capsuleRadius = 0.5f; // Радиус капсулы
            float capsuleHeight = 1.0f; // Высота капсулы, регулируйте ее по необходимости

            // Определяем вертикальную капсулу вокруг заданной позиции
            Collider[] colliders = Physics.OverlapCapsule(position, position + Vector3.up * capsuleHeight, capsuleRadius);

            foreach (Collider collider in colliders)
            {
                // Проверяем, принадлежит ли коллайдер какому-либо созданному объекту (кроме ландшафта)
                if (collider.gameObject != terrain.gameObject && collider.isTrigger == false)
                {
                    return true;
                }
            }
            return false;
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
        #endregion 
    }
}