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
        // ��������� ������������ � ��������
        if (collision.gameObject.tag == "Player")
        {
            DeformSurface(collision.contacts[0].point);
        }
    }

    void DeformSurface(Vector3 point)
    {
        // �������� ������� �����
        Vector3[] vertices = mesh.vertices;

        // ������� ��������� ������� � ����� ������������
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

        // �������� ��������� ��������� �������
        vertices[closestVertexIndex] += Vector3.up * deformationAmount;

        // ��������� ��������� � �����
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }
}
