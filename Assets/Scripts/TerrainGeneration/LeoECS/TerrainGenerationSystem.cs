using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;
using System.Linq;
using AQUAS_Lite;
using System.Threading.Tasks;

public class TerrainGenerationSystem : IEcsInitSystem
{
    private EcsWorld ecsWorld;
    private EcsEntity entities;

    [System.Obsolete]
    public void Init()
    {
        ecsWorld = new EcsWorld();
        foreach (var terrainGenerationView in Object.FindObjectsOfType<TerrainGenerationView>())
        {
            #region TerrainStruct 0
            var generateTerrain = ecsWorld.NewEntity();
            ref var terrainStruct = ref generateTerrain.Get<TerrainStruct>();
            terrainStruct.textureHeight = terrainGenerationView.textureHeight;
            terrainStruct.textureWidth = terrainGenerationView.textureWidth;
            terrainStruct.thickness = terrainGenerationView.thickness;
            terrainStruct.resolution = terrainGenerationView.resolution;
            terrainStruct.amplitude = terrainGenerationView.amplitude;
            terrainStruct.frequency = terrainGenerationView.frequency;
            terrainStruct.blurIntensity = terrainGenerationView.blurIntensity;
            terrainStruct.texture = terrainGenerationView.texture;
            terrainStruct.Water = terrainGenerationView.Water;
            terrainStruct.pathPoints = terrainGenerationView.pathPoints;
            terrainStruct.pixels = terrainGenerationView.pixels;
            terrainStruct.mat = terrainGenerationView.mat;
            terrainStruct.size = terrainGenerationView.size;
            terrainStruct.quality = terrainGenerationView.quality;
            terrainStruct.HillFrequency = terrainGenerationView.HillFrequency;
            terrainStruct.LowestHillHeight = terrainGenerationView.LowestHillHeight;
            terrainStruct.HighestHillHeight = terrainGenerationView.HighestHillHeight;
            terrainStruct.textures = terrainGenerationView.textures;
            terrainStruct.textureHeights = terrainGenerationView.textureHeights;
            terrainStruct.deeps = terrainGenerationView.deeps;
            terrainStruct.perlinWeight = terrainGenerationView.perlinWeight;
            terrainStruct.textureWeight = terrainGenerationView.textureWeight;
            terrainStruct.Map = terrainGenerationView.Map;
            terrainStruct.terrain = terrainGenerationView.terrain;
            terrainStruct.terrainData = terrainGenerationView.terrainData;
            #endregion
            #region ListSpawnTrees 1
            terrainStruct.listSpawnTrees = new ListSpawnTrees[terrainGenerationView.listSpawnTrees.Length];
            for (int i = 0; i < terrainGenerationView.listSpawnTrees.Length; i++)
            {
                var listSpawnTreesEntity = ecsWorld.NewEntity();
                ref var listSpawnTreesStruct = ref listSpawnTreesEntity.Get<ListSpawnTrees>();
                listSpawnTreesStruct.Tree = terrainGenerationView.listSpawnTrees[i].Tree;
                listSpawnTreesStruct.MinSize = terrainGenerationView.listSpawnTrees[i].MinSize;
                listSpawnTreesStruct.MaxSize = terrainGenerationView.listSpawnTrees[i].MaxSize;
                listSpawnTreesStruct.NumberOfTrees = terrainGenerationView.listSpawnTrees[i].NumberOfTrees;
                listSpawnTreesStruct.CreationUnderTheRiver = terrainGenerationView.listSpawnTrees[i].CreationUnderTheRiver;
                listSpawnTreesStruct.UseMeshCombiner = terrainGenerationView.listSpawnTrees[i].UseMeshCombiner;
                listSpawnTreesStruct.AddMarker = terrainGenerationView.listSpawnTrees[i].AddMarker;

                terrainStruct.listSpawnTrees[i] = listSpawnTreesStruct;
            }
            #endregion
            #region ListSpawnGrass 2
            terrainStruct.listSpawnGrass = new ListSpawnGrass[terrainGenerationView.listSpawnGrass.Length];
            for (int i = 0; i < terrainGenerationView.listSpawnGrass.Length; i++)
            {
                var listSpawnGrassEntity = ecsWorld.NewEntity();
                ref var listSpawnGrassStruct = ref listSpawnGrassEntity.Get<ListSpawnGrass>();
                listSpawnGrassStruct.Grass = terrainGenerationView.listSpawnGrass[i].Grass;
                listSpawnGrassStruct.MinSize = terrainGenerationView.listSpawnGrass[i].MinSize;
                listSpawnGrassStruct.MaxSize = terrainGenerationView.listSpawnGrass[i].MaxSize;
                listSpawnGrassStruct.NumberOfGrass = terrainGenerationView.listSpawnGrass[i].NumberOfGrass;
                listSpawnGrassStruct.CreationUnderTheRiver = terrainGenerationView.listSpawnGrass[i].CreationUnderTheRiver;
                listSpawnGrassStruct.UseMeshCombiner = terrainGenerationView.listSpawnGrass[i].UseMeshCombiner;
                listSpawnGrassStruct.AddMarker = terrainGenerationView.listSpawnGrass[i].AddMarker;

                terrainStruct.listSpawnGrass[i] = listSpawnGrassStruct;
            }

            #endregion
            #region ListSpawnRocks 3
            terrainStruct.listSpawnRocks = new ListSpawnRocks[terrainGenerationView.listSpawnRocks.Length];
            for (int i = 0; i < terrainGenerationView.listSpawnRocks.Length; i++)
            {
                var listSpawnRocksEntity = ecsWorld.NewEntity();
                ref var listSpawnRocksStruct = ref listSpawnRocksEntity.Get<ListSpawnRocks>();
                listSpawnRocksStruct.Rocks = terrainGenerationView.listSpawnRocks[i].Rocks;
                listSpawnRocksStruct.MinSize = terrainGenerationView.listSpawnRocks[i].MinSize;
                listSpawnRocksStruct.MaxSize = terrainGenerationView.listSpawnRocks[i].MaxSize;
                listSpawnRocksStruct.NumberOfRocks = terrainGenerationView.listSpawnRocks[i].NumberOfRocks;
                listSpawnRocksStruct.CreationUnderTheRiver = terrainGenerationView.listSpawnRocks[i].CreationUnderTheRiver;
                listSpawnRocksStruct.UseMeshCombiner = terrainGenerationView.listSpawnRocks[i].UseMeshCombiner;
                listSpawnRocksStruct.AddMarker = terrainGenerationView.listSpawnRocks[i].AddMarker;

                terrainStruct.listSpawnRocks[i] = listSpawnRocksStruct;
            }

            #endregion
            #region AlphaMap & AlphaMapLayer 4
            terrainStruct.alphaMap = new AlphaMap[terrainGenerationView.alphaMap.Length];
            for (int i = 0; i < terrainGenerationView.alphaMap.Length; i++)
            {
                var alphaMapEntity = ecsWorld.NewEntity();
                ref var alphaMapStruct = ref alphaMapEntity.Get<AlphaMap>();
                alphaMapStruct.minHeight = terrainGenerationView.alphaMap[i].minHeight;
                alphaMapStruct.maxHeight = terrainGenerationView.alphaMap[i].maxHeight;

                alphaMapStruct.layers = new AlphaMapLayer[terrainGenerationView.alphaMap[i].layers.Length];
                for (int j = 0; j < terrainGenerationView.alphaMap[i].layers.Length; j++)
                {
                    var layerEntity = ecsWorld.NewEntity();
                    ref var layerComponent = ref layerEntity.Get<AlphaMapLayer>();
                    layerComponent.texture = terrainGenerationView.alphaMap[i].layers[j].texture;
                    layerComponent.transparency = terrainGenerationView.alphaMap[i].layers[j].transparency;

                    alphaMapStruct.layers[j] = layerComponent;
                }

                terrainStruct.alphaMap[i] = alphaMapStruct;
            }
            entities = generateTerrain;
            #endregion
            TerrainGenerate();
        }
    }

    [System.Obsolete]
    void TerrainGenerate()
    {
        var entitiyTerrain = entities;
        var terrainComponent = entitiyTerrain.Get<TerrainStruct>();

        terrainComponent.pathPoints = new List<Vector2>();
        terrainComponent.pixels = new Color[terrainComponent.textureWidth * terrainComponent.textureHeight];
        terrainComponent.texture = new Texture2D(terrainComponent.textureWidth, terrainComponent.textureHeight);
        GeneratePathWithSinusoid(terrainComponent);
        Generate(terrainComponent.texture, terrainComponent);

        int width = terrainComponent.terrainData.heightmapWidth;
        int height = terrainComponent.terrainData.heightmapHeight;
        float[,] heights = new float[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                heights[x, y] = 0f;
            }
        }

        terrainComponent.terrainData.SetHeights(0, 0, heights);
        terrainComponent.terrainData.SetHeights(0, 0, PerlinNoise(terrainComponent.terrain, terrainComponent.HillFrequency, terrainComponent));
        terrainComponent.terrainData.SetHeights(0, 0, CombineHeights(PerlinNoise(terrainComponent.terrain, terrainComponent.HillFrequency, terrainComponent),
            GetHeightsFromTexture(terrainComponent.texture, terrainComponent.terrainData.heightmapWidth, terrainComponent.terrainData.heightmapHeight, terrainComponent),
            terrainComponent.terrainData, terrainComponent));

        CreateTextureLayers(terrainComponent);
        ApplyTextures(terrainComponent);

        SpawnObjectsOnTerrain(terrainComponent);
    }

    #region создание местности
    [System.Obsolete]
    float[,] PerlinNoise(Terrain terrain, float tileSize, TerrainStruct terrainComponent)
    {
        //Debug.Log($"Минимальная высота холмов {terrainComponent.LowestHillHeight} Максимальная высота холмов {terrainComponent.HighestHillHeight}");
        float RandomHeightOfHills = Random.Range(terrainComponent.LowestHillHeight, terrainComponent.HighestHillHeight);
        //Debug.Log($"Рандомная высота холмов {RandomHeightOfHills}");
        float hillHeight = (float)RandomHeightOfHills / ((float)terrain.terrainData.heightmapHeight / 2);
        float baseHeight = terrainComponent.LowestHillHeight / ((float)terrain.terrainData.heightmapHeight / 2);
        float[,] heights = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];

        for (int i = 0; i < terrain.terrainData.heightmapWidth; i++)
        {
            for (int k = 0; k < terrain.terrainData.heightmapHeight; k++)
            {
                heights[i, k] = baseHeight + (Mathf.PerlinNoise((float)i / terrain.terrainData.heightmapWidth * tileSize, (float)k / terrain.terrainData.heightmapHeight * tileSize) * (float)hillHeight);
            }
        }

        return heights;
    }
    float[,] GetHeightsFromTexture(Texture2D texture, int width, int height, TerrainStruct terrainComponent)
    {
        float deep = terrainComponent.deeps;
        float depthDifference, grayscaleValue, heightValue;
        float[,] heightsForTexture = terrainComponent.terrain.terrainData.GetHeights(0, 0, width, height);

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
    float[,] CombineHeights(float[,] perlinHeights, float[,] textureHeights, TerrainData terrainData, TerrainStruct terrainComponent)
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
                combinedHeight = (perlinHeights[x, y] * terrainComponent.perlinWeight + textureHeights[x, y] * terrainComponent.textureWeight) / (terrainComponent.perlinWeight + terrainComponent.textureWeight);

                combinedHeights[x, y] = combinedHeight;
            }
        }
        return combinedHeights;
    }
    void CreateTextureLayers(TerrainStruct terrainComponent)
    {
        int totalTextureCount = 0;
        for (int i = 0; i < terrainComponent.alphaMap.Length; i++)
        {
            totalTextureCount += terrainComponent.alphaMap[i].layers.Length;
        }

        TerrainLayer[] terrainLayers = new TerrainLayer[totalTextureCount];
        int layerIndex = 0;

        for (int j = 0; j < terrainComponent.alphaMap.Length; j++)
        {
            for (int i = 0; i < terrainComponent.alphaMap[j].layers.Length; i++)
            {
                TerrainLayer terrainLayer = new TerrainLayer();
                terrainLayer.diffuseTexture = terrainComponent.alphaMap[j].layers[i].texture;
                terrainLayer.tileSize = new Vector2(10, 10);
                terrainLayer.diffuseRemapMin = new Vector4(0, 0, 0, 0);
                terrainLayer.diffuseRemapMax = new Vector4(1, 1, 1, 1);

                terrainLayers[layerIndex] = terrainLayer;
                layerIndex++;
            }
        }

        terrainComponent.terrainData.terrainLayers = terrainLayers;
    }
    void ApplyTextures(TerrainStruct terrainComponent)
    {
        int textureCount = 0;
        foreach (AlphaMap alphaMapInfo in terrainComponent.alphaMap)
        {
            textureCount += alphaMapInfo.layers.Length;
        }

        float[,,] splatmapData = new float[terrainComponent.terrainData.alphamapWidth, terrainComponent.terrainData.alphamapHeight, textureCount];

        for (int x = 0; x < terrainComponent.terrainData.alphamapWidth; x++)
        {
            for (int y = 0; y < terrainComponent.terrainData.alphamapHeight; y++)
            {
                float height = terrainComponent.terrainData.GetHeight(y, x);

                int layerIndex = 0;
                for (int i = 0; i < terrainComponent.alphaMap.Length; i++)
                {
                    AlphaMap alphaMapInfo = terrainComponent.alphaMap[i];

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
                        layerIndex += alphaMapInfo.layers.Length;
                    }
                }
            }
        }
        terrainComponent.terrainData.SetAlphamaps(0, 0, splatmapData);
    }
    void SpawnObjectsOnTerrain(TerrainStruct terrainComponent)
    {
        TerrainData terrainData = terrainComponent.terrain.terrainData;
        float terrainWidth = terrainData.size.x;
        float terrainLength = terrainData.size.z;
        float terrainPosX = terrainComponent.terrain.transform.position.x;
        float terrainPosZ = terrainComponent.terrain.transform.position.z;

        #region // Создание деревьев на местности и использование Mesh Combiner
        GameObject trees = new GameObject("Trees");
        trees.AddComponent<MeshFilter>();
        trees.AddComponent<MeshRenderer>();
        trees.transform.SetParent(terrainComponent.terrain.transform);

        // Цикл создания деревьев на местности
        foreach (ListSpawnTrees spawnTree in terrainComponent.listSpawnTrees)
        {
            List<GameObject> spawnedObjects = new List<GameObject>(); // Список для хранения созданных объектов

            if (spawnTree.CreationUnderTheRiver == true)
            {
                terrainComponent.Water.isTrigger = false;
            }

            for (int i = 0; i < spawnTree.NumberOfTrees; i++)
            {
                GameObject prefab = spawnTree.Tree;

                Vector3 randomPosition = GetRandomTerrainPosition(terrainPosX, terrainPosZ, terrainWidth, terrainLength, terrainComponent);
                int maxAttempts = 10;
                int attempts = 0;
                while (IsPositionColliding(randomPosition, terrainComponent) && attempts < maxAttempts)
                {
                    randomPosition = GetRandomTerrainPosition(terrainPosX, terrainPosZ, terrainWidth, terrainLength, terrainComponent);
                    attempts++;
                }

                if (attempts < maxAttempts)
                {
                    GameObject instance = Object.Instantiate(prefab, randomPosition, Quaternion.identity);

                    if (spawnTree.MinSize != 0 && spawnTree.MaxSize != 0)
                    {
                        float randomScale = Random.Range(spawnTree.MinSize, spawnTree.MaxSize);
                        instance.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
                    }
                    instance.transform.Rotate(0, Random.Range(0, 360), 0);

                    if (spawnTree.UseMeshCombiner == false)
                    {
                        instance.transform.SetParent(terrainComponent.terrain.transform);
                    }
                    else
                    {
                        instance.transform.SetParent(trees.transform);
                    }
                    if (spawnTree.AddMarker)
                        terrainComponent.Map.AddMarkerObjectStatic(instance);
                    spawnedObjects.Add(instance); // Добавляем объект в список
                }
            }
            terrainComponent.Water.isTrigger = true;
        }
        #endregion
        #region // Создание камней на местности и использование Mesh Combiner
        GameObject rocks = new GameObject("Rocks");
        rocks.AddComponent<MeshFilter>();
        rocks.AddComponent<MeshRenderer>();
        rocks.transform.SetParent(terrainComponent.terrain.transform);

        // Цикл создания камней на местности
        foreach (ListSpawnRocks spawnRocks in terrainComponent.listSpawnRocks)
        {
            List<GameObject> spawnedObjects = new List<GameObject>(); // Список для хранения созданных объектов
            if (spawnRocks.CreationUnderTheRiver == true)
            {
                terrainComponent.Water.isTrigger = false;
            }
            for (int i = 0; i < spawnRocks.NumberOfRocks; i++)
            {
                GameObject prefab = spawnRocks.Rocks;

                Vector3 randomPosition = GetRandomTerrainPosition(terrainPosX, terrainPosZ, terrainWidth, terrainLength, terrainComponent);
                int maxAttempts = 10;
                int attempts = 0;
                while (IsPositionColliding(randomPosition, terrainComponent) && attempts < maxAttempts)
                {
                    randomPosition = GetRandomTerrainPosition(terrainPosX, terrainPosZ, terrainWidth, terrainLength, terrainComponent);
                    attempts++;
                }

                if (attempts < maxAttempts)
                {
                    GameObject instance = Object.Instantiate(prefab, randomPosition, Quaternion.identity);

                    if (spawnRocks.MinSize != 0 && spawnRocks.MaxSize != 0)
                    {
                        float randomScale = Random.Range(spawnRocks.MinSize, spawnRocks.MaxSize);
                        instance.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
                    }
                    instance.transform.Rotate(0, Random.Range(0, 360), 0);

                    if (spawnRocks.UseMeshCombiner == false)
                    {
                        instance.transform.SetParent(terrainComponent.terrain.transform);
                    }
                    else
                    {
                        instance.transform.SetParent(rocks.transform);
                    }
                    if (spawnRocks.AddMarker)
                        terrainComponent.Map.AddMarkerObjectStatic(instance);
                    spawnedObjects.Add(instance); // Добавляем объект в список
                }
            }
            terrainComponent.Water.isTrigger = true;
        }
        #endregion
        #region // Создание травы на местности и использование Mesh Combiner
        GameObject grass = new GameObject("Grass");
        grass.AddComponent<MeshFilter>();
        grass.AddComponent<MeshRenderer>();
        grass.transform.SetParent(terrainComponent.terrain.transform);

        // Цикл создания травы на местности
        foreach (ListSpawnGrass spawnGrass in terrainComponent.listSpawnGrass)
        {
            List<GameObject> spawnedObjects = new List<GameObject>(); // Список для хранения созданных объектов

            if (spawnGrass.CreationUnderTheRiver == true)
            {
                terrainComponent.Water.isTrigger = false;
            }

            for (int i = 0; i < spawnGrass.NumberOfGrass; i++)
            {
                GameObject prefab = spawnGrass.Grass;

                // Попытка найти допустимую позицию появления (не сталкиваясь с другими объектами)
                Vector3 randomPosition = GetRandomTerrainPosition(terrainPosX, terrainPosZ, terrainWidth, terrainLength, terrainComponent);
                int maxAttempts = 10;
                int attempts = 0;
                while (IsPositionColliding(randomPosition, terrainComponent) && attempts < maxAttempts)
                {
                    randomPosition = GetRandomTerrainPosition(terrainPosX, terrainPosZ, terrainWidth, terrainLength, terrainComponent);
                    attempts++;
                }

                if (attempts < maxAttempts)
                {
                    // Создаём экземпляр объекта в случайной позиции
                    GameObject instance = Object.Instantiate(prefab, randomPosition, Quaternion.identity);

                    if (spawnGrass.MinSize != 0 && spawnGrass.MaxSize != 0)
                    {
                        float randomScale = Random.Range(spawnGrass.MinSize, spawnGrass.MaxSize);
                        instance.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
                    }

                    instance.transform.Rotate(0, Random.Range(0, 360), 0);

                    if (spawnGrass.UseMeshCombiner == false)
                    {
                        instance.transform.SetParent(terrainComponent.terrain.transform);
                    }
                    else
                    {
                        instance.transform.SetParent(grass.transform);
                    }
                    if (spawnGrass.AddMarker)
                        terrainComponent.Map.AddMarkerObjectStatic(instance);
                    spawnedObjects.Add(instance); // Добавляем объект в список
                }
            }
            terrainComponent.Water.isTrigger = true;
        }
        #endregion
        terrainComponent.Map.AddMarkerStaticObject();
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
    Vector3 GetRandomTerrainPosition(float terrainPosX, float terrainPosZ, float terrainWidth, float terrainLength, TerrainStruct terrainComponent)
    {
        float randomX = Random.Range(0f, terrainWidth);
        float randomZ = Random.Range(0f, terrainLength);
        float y = terrainComponent.terrain.SampleHeight(new Vector3(randomX + terrainPosX, 0, randomZ + terrainPosZ));
        y += terrainComponent.terrain.transform.position.y;
        return new Vector3(randomX + terrainPosX, y, randomZ + terrainPosZ);
    }
    bool IsPositionColliding(Vector3 position, TerrainStruct terrainComponent)
    {
        float capsuleRadius = 0.5f; // Радиус капсулы
        float capsuleHeight = 1.0f; // Высота капсулы, регулируйте ее по необходимости

        // Определяем вертикальную капсулу вокруг заданной позиции
        Collider[] colliders = Physics.OverlapCapsule(position, position + Vector3.up * capsuleHeight, capsuleRadius);

        foreach (Collider collider in colliders)
        {
            // Проверяем, принадлежит ли коллайдер какому-либо созданному объекту (кроме ландшафта)
            if (collider.gameObject != terrainComponent.terrain.gameObject && collider.isTrigger == false)
            {
                return true;
            }
        }
        return false;
    }
    #endregion

    #region Создание реки на местности
    void Generate(Texture2D textureWater, TerrainStruct terrainComponent)
    {
        terrainComponent.Water.transform.position = Vector3.zero;
        terrainComponent.Water.gameObject.layer = LayerMask.NameToLayer("Water");
        MeshFilter meshFilter = terrainComponent.Water.gameObject.AddComponent<MeshFilter>();

        terrainComponent.Water.gameObject.AddComponent<MeshRenderer>().materials = terrainComponent.mat;
        terrainComponent.Water.gameObject.AddComponent<AQUAS_Lite_Reflection>();

        int width = textureWater.width;
        int height = textureWater.height;

        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        Vector3[] vertices = new Vector3[height * width];
        Vector2[] UVs = new Vector2[height * width];
        Vector4[] tangs = new Vector4[height * width];

        Vector2 uvScale = new Vector2(1 / (width - 1), 1 / (height - 1));
        Vector3 sizeScale = new Vector3(terrainComponent.size.x / (width - 1), terrainComponent.size.y, terrainComponent.size.z / (height - 1));

        int index;
        float pixelHeight;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                index = y * width + x;
                pixelHeight = textureWater.GetPixel(x, y).grayscale;
                Vector3 vertex = new Vector3(x, pixelHeight, y);
                vertices[index] = Vector3.Scale(sizeScale, vertex);
                Vector2 cur_uv = new Vector2(x, y);
                UVs[index] = Vector2.Scale(cur_uv, uvScale);

                Vector3 leftV = new Vector3(x - 1, textureWater.GetPixel(x - 1, y).grayscale, y);
                Vector3 rightV = new Vector3(x + 1, textureWater.GetPixel(x + 1, y).grayscale, y);
                Vector3 tang = Vector3.Scale(sizeScale, rightV - leftV).normalized;
                tangs[index] = new Vector4(tang.x, tang.y, tang.z, 1);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = UVs;

        index = 0;
        int[] triangles = new int[(height - 1) * (width - 1) * 6];

        for (int y = 0; y < height - 1; y++)
        {
            for (int x = 0; x < width - 1; x++)
            {
                if (Mathf.Approximately(textureWater.GetPixel(x, y).grayscale, 1.0f))
                {
                    continue;
                }
                triangles[index++] = (y * width) + x;
                triangles[index++] = ((y + 1) * width) + x;
                triangles[index++] = (y * width) + x + 1;

                triangles[index++] = ((y + 1) * width) + x;
                triangles[index++] = ((y + 1) * width) + x + 1;
                triangles[index++] = (y * width) + x + 1;
            }
        }

        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.tangents = tangs;

        MirrorMeshX(mesh);

        meshFilter.sharedMesh = mesh;

        SimplifyMeshFilter(meshFilter, terrainComponent);

        terrainComponent.Water.transform.Rotate(0, 90, 0);
        terrainComponent.Water.transform.position = new Vector2(0, 2.6f);

        terrainComponent.Water.convex = true;
    }

    void SimplifyMeshFilter(MeshFilter meshFilter, TerrainStruct terrainComponent)
    {
        Mesh sourceMesh = meshFilter.sharedMesh;
        if (sourceMesh == null)
            return;

        var meshSimplifier = new UnityMeshSimplifier.MeshSimplifier();
        meshSimplifier.Initialize(sourceMesh);

        meshSimplifier.SimplifyMesh(terrainComponent.quality);

        meshFilter.sharedMesh = meshSimplifier.ToMesh();
        terrainComponent.Water.sharedMesh = meshSimplifier.ToMesh();
    }
    void MirrorMeshX(Mesh mesh)
    {
        // Отзеркаливание вершин по оси X
        mesh.vertices = mesh.vertices.Select(v => new Vector3(-v.x, v.y, v.z)).ToArray();

        // Обновление треугольников для сохранения правильного направления поверхности
        mesh.triangles = mesh.triangles.Reverse().ToArray();

        // Пересчет нормалей для корректного отображения освещения
        mesh.RecalculateNormals();
    }
    void GeneratePathWithSinusoid(TerrainStruct terrainComponent)
    {
        bool isValidPath = false;
        bool isFirstPointOnTopOrBottomEdge;
        int edge;

        float x1, y1, x2, y2;
        float step, t, x, y;
        float angle, offsetY;
        for (int i = 0; i < terrainComponent.pixels.Length; i++)
        {
            terrainComponent.pixels[i] = Color.white;
        }
        int n = 25;
        while (!isValidPath)
        {
            edge = Random.Range(0, 4);
            isFirstPointOnTopOrBottomEdge = edge < 2;

            if (isFirstPointOnTopOrBottomEdge)
            {
                x1 = Random.Range(n, terrainComponent.textureWidth - n);
                y1 = (edge == 0) ? n : (terrainComponent.textureHeight - n - 1);
                x2 = Random.Range(n, terrainComponent.textureWidth - n);
                y2 = (edge == 0) ? (terrainComponent.textureHeight - n - 1) : n;
            }
            else
            {
                x1 = (edge == 2) ? n : (terrainComponent.textureWidth - n - 1);
                y1 = Random.Range(n, terrainComponent.textureHeight - n);
                x2 = (edge == 2) ? (terrainComponent.textureWidth - n - 1) : n;
                y2 = Random.Range(n, terrainComponent.textureHeight - n);
            }

            if (x1 >= 0 && x1 < terrainComponent.textureWidth && y1 >= 0 && y1 < terrainComponent.textureHeight &&
                x2 >= 0 && x2 < terrainComponent.textureWidth && y2 >= 0 && y2 < terrainComponent.textureHeight)
            {
                isValidPath = true;

                step = 1f / terrainComponent.resolution;

                for (int i = 0; i < terrainComponent.resolution; i++)
                {
                    t = i * step;

                    x = Mathf.Lerp(x1, x2, t);
                    y = Mathf.Lerp(y1, y2, t);

                    angle = 2f * Mathf.PI * terrainComponent.frequency * t;
                    offsetY = terrainComponent.amplitude * Mathf.Sin(angle);

                    y += offsetY;

                    x = Mathf.Clamp(x, 0, terrainComponent.textureWidth - 1);
                    y = Mathf.Clamp(y, 0, terrainComponent.textureHeight - 1);

                    terrainComponent.pathPoints.Add(new Vector2(x, y));
                }
            }
        }
        // Рисуем путь с указанной толщиной линии
        foreach (Vector2 point in terrainComponent.pathPoints)
        {
            DrawLine(point, terrainComponent.thickness, Color.black, terrainComponent);
        }
        ApplyBlurToTexture(terrainComponent.blurIntensity, terrainComponent);
        // Устанавливаем значения пикселей в текстуру и применяем изменения
        terrainComponent.texture.SetPixels(terrainComponent.pixels);
        terrainComponent.texture.Apply();
        // Применяем эффект размытия к текстуре с указанной интенсивностью
    }
    void DrawLine(Vector2 point, int thickness, Color lineColor, TerrainStruct terrainComponent)
    {
        int index;
        int halfThickness = thickness / 2;

        int startX = Mathf.RoundToInt(point.x) - halfThickness;
        int endX = Mathf.RoundToInt(point.x) + halfThickness;
        int startY = Mathf.RoundToInt(point.y) - halfThickness;
        int endY = Mathf.RoundToInt(point.y) + halfThickness;

        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                if (x >= 0 && x < terrainComponent.textureWidth && y >= 0 && y < terrainComponent.textureHeight)
                {
                    index = y * terrainComponent.textureWidth + x;
                    if (terrainComponent.pixels[index] == Color.white)
                    {
                        terrainComponent.pixels[index] = lineColor;
                    }
                }
            }
        }
    }
    void ApplyBlurToTexture(float intensity, TerrainStruct terrainComponent)
    {
        int radius = Mathf.RoundToInt(intensity * 5f);
        int index;
        for (int x = 0; x < terrainComponent.textureWidth; x++)
        {
            for (int y = 0; y < terrainComponent.textureHeight; y++)
            {
                index = y * terrainComponent.textureWidth + x;
                terrainComponent.pixels[index] = GetBlurredColor(x, y, radius, terrainComponent);
            }
        }
    }

    Color GetBlurredColor(int x, int y, int radius, TerrainStruct terrainComponent)
    {
        Color blurredColor = Color.black;
        int count = 0;
        int neighborX, index;
        for (int i = -radius; i <= radius; i++)
        {
            int neighborY = y + i;

            if (neighborY >= 0 && neighborY < terrainComponent.textureHeight)
            {
                for (int j = -radius; j <= radius; j++)
                {
                    neighborX = x + j;

                    if (neighborX >= 0 && neighborX < terrainComponent.textureWidth)
                    {
                        index = neighborY * terrainComponent.textureWidth + neighborX;
                        blurredColor += terrainComponent.pixels[index];
                        count++;
                    }
                }
            }
        }

        blurredColor /= count;

        return blurredColor;
    }
    #endregion
}