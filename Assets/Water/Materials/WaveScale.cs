using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveScale : MonoBehaviour
{
    MeshFilter meshFilter;
    Mesh mesh;
    Vector3[] originalVertices;

    public float deformationAmount = 0.1f;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;
        originalVertices = mesh.vertices;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Проверяем столкновение с объектом
        if (collision.gameObject.tag == "Player")
        {
            DeformSurface(collision.contacts[0].point);
        }
    }

    void DeformSurface(Vector3 point)
    {
        // Получаем вершины сетки
        Vector3[] vertices = mesh.vertices;

        // Находим ближайшую вершину к точке столкновения
        int closestVertexIndex = 0;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < vertices.Length; i++)
        {
            float distance = Vector3.Distance(point, transform.TransformPoint(vertices[i]));
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestVertexIndex = i;
            }
        }

        // Изменяем положение выбранной вершины
        vertices[closestVertexIndex] += Vector3.up * deformationAmount;

        // Применяем изменения к сетке
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }
}
