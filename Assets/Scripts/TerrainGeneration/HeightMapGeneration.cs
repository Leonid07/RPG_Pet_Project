using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;
using AQUAS_Lite;
using Leopotam.Ecs;

namespace GenerateTerrain
{
    public class HeightMapGeneration : MonoBehaviour
    {
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
        [HideInInspector]
        public MeshCollider Water;
        List<Vector2> pathPoints;
        Color[] pixels;

        [SerializeField] Material[] mat;
        [SerializeField] Vector3 size = new Vector3(100, 0, 100);
        [Range(0,1)]
        [SerializeField] float quality = 0.001f;

        void Start()
        {
            pathPoints = new List<Vector2>();
            pixels = new Color[textureWidth * textureHeight];
            texture = new Texture2D(textureWidth, textureHeight);
            GeneratePathWithSinusoid();
            Generate(texture);

        }

        void Generate(Texture2D textureWater)
        {
            GameObject go = new GameObject("Water");
            go.transform.position = Vector3.zero;
            go.layer = LayerMask.NameToLayer("Water");
            MeshFilter meshFilter = go.AddComponent<MeshFilter>();

            go.AddComponent<MeshRenderer>().materials = mat;
            go.AddComponent<AQUAS_Lite_Reflection>();

            int width = textureWater.width;
            int height = textureWater.height;

            Mesh mesh = new Mesh();
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            Vector3[] vertices = new Vector3[height * width];
            Vector2[] UVs = new Vector2[height * width];
            Vector4[] tangs = new Vector4[height * width];

            Vector2 uvScale = new Vector2(1 / (width - 1), 1 / (height - 1));
            Vector3 sizeScale = new Vector3(size.x / (width - 1), size.y, size.z / (height - 1));

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

            SimplifyMeshFilter(meshFilter);

            go.transform.Rotate(0,90,0);
            go.transform.position = new Vector2(0,2.6f);

            MeshCollider meshCollider = go.AddComponent<MeshCollider>();
            meshCollider.convex = true;

            Water = meshCollider;
        }
        void SimplifyMeshFilter(MeshFilter meshFilter)
        {
            Mesh sourceMesh = meshFilter.sharedMesh;
            if (sourceMesh == null) // verify that the mesh filter actually has a mesh
                return;

            // Create our mesh simplifier and setup our entire mesh in it
            var meshSimplifier = new UnityMeshSimplifier.MeshSimplifier();
            meshSimplifier.Initialize(sourceMesh);

            // This is where the magic happens, lets simplify!
            meshSimplifier.SimplifyMesh(quality);

            // Create our final mesh and apply it back to our mesh filter
            meshFilter.sharedMesh = meshSimplifier.ToMesh();
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
        void GeneratePathWithSinusoid()
        {
            bool isValidPath = false;
            bool isFirstPointOnTopOrBottomEdge;
            int edge;

            float x1, y1, x2, y2;
            float step, t, x, y;
            float angle, offsetY;

            // Залейте текстуру белым цветом.
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = Color.white;
            }
            int n = 25;
            while (!isValidPath)
            {
                // Сгенерируйте случайные координаты для первой точки на левом, правом, верхнем или нижнем краю.
                edge = Random.Range(0, 4);
                isFirstPointOnTopOrBottomEdge = edge < 2;

                if (isFirstPointOnTopOrBottomEdge)
                {
                    //Первая точка на верхнем или нижнем краю
                    x1 = Random.Range(n, textureWidth - n);
                    y1 = (edge == 0) ? n : (textureHeight - n - 1);
                    x2 = Random.Range(n, textureWidth - n);
                    y2 = (edge == 0) ? (textureHeight - n - 1) : n;
                }
                else
                {
                    // Первая точка на левом или правом краю
                    x1 = (edge == 2) ? n : (textureWidth - n - 1);
                    y1 = Random.Range(n, textureHeight - n);
                    x2 = (edge == 2) ? (textureWidth - n - 1) : n;
                    y2 = Random.Range(n, textureHeight - n);
                }

                // Проверьте, находятся ли первая и вторая точки в пределах границ текстуры.
                if (x1 >= 0 && x1 < textureWidth && y1 >= 0 && y1 < textureHeight &&
                    x2 >= 0 && x2 < textureWidth && y2 >= 0 && y2 < textureHeight)
                {
                    isValidPath = true;

                    // Рассчитайте шаг синусоиды на основе частоты и разрешения.
                    step = 1f / resolution;

                    // Сгенерируйте координаты для каждой точки на пути
                    for (int i = 0; i < resolution; i++)
                    {
                        t = i * step;

                        // Рассчитать координаты x и y на основе синусоиды
                        x = Mathf.Lerp(x1, x2, t);
                        y = Mathf.Lerp(y1, y2, t);

                        angle = 2f * Mathf.PI * frequency * t;
                        offsetY = amplitude * Mathf.Sin(angle);

                        // Примените синусоидальное смещение к координате y
                        y += offsetY;

                        // Зафиксируйте координаты внутри границ текстуры
                        x = Mathf.Clamp(x, 0, textureWidth - 1);
                        y = Mathf.Clamp(y, 0, textureHeight - 1);

                        pathPoints.Add(new Vector2(x, y));
                    }
                }
            }
            // Рисуем путь с указанной толщиной линии
            foreach (Vector2 point in pathPoints)
            {
                DrawLine(point, thickness, Color.black);
            }
            ApplyBlurToTexture(blurIntensity);
            // Устанавливаем значения пикселей в текстуру и применяем изменения
            texture.SetPixels(pixels);
            texture.Apply();
            // Применяем эффект размытия к текстуре с указанной интенсивностью
        }
        void DrawLine(Vector2 point, int thickness, Color lineColor)
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
                    if (x >= 0 && x < textureWidth && y >= 0 && y < textureHeight)  // Исправленная строка
                    {
                        index = y * textureWidth + x;
                        if (pixels[index] == Color.white)
                        {
                            pixels[index] = lineColor;
                        }
                    }
                }
            }
        }
        void ApplyBlurToTexture(float intensity)
        {
            int radius = Mathf.RoundToInt(intensity * 5f);
            int index;
            // Применить эффект размытия ко всей текстуре
            for (int x = 0; x < textureWidth; x++)
            {
                for (int y = 0; y < textureHeight; y++)
                {
                    index = y * textureWidth + x;
                    pixels[index] = GetBlurredColor(x, y, radius);
                }
            }
        }
        Color GetBlurredColor(int x, int y, int radius)
        {
            Color blurredColor = Color.black;
            int count = 0;
            int neighborX, index;
            // Вычислить средний цвет соседних пикселей в пределах радиуса
            for (int i = -radius; i <= radius; i++)
            {
                int neighborY = y + i;

                if (neighborY >= 0 && neighborY < textureHeight)
                {
                    for (int j = -radius; j <= radius; j++)
                    {
                        neighborX = x + j;

                        if (neighborX >= 0 && neighborX < textureWidth)
                        {
                            index = neighborY * textureWidth + neighborX;
                            blurredColor += pixels[index];
                            count++;
                        }
                    }
                }
            }

            blurredColor /= count;

            return blurredColor;
        }
    }
}