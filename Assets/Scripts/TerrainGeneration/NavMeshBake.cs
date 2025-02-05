using System.Collections;
using System.Collections.Generic;
using Unity.AI;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshBake : MonoBehaviour
{
    public NavMeshSurface navMeshSurface;

    private void Start()
    {
        GenerateNavMesh();
    }

    private void GenerateNavMesh()
    {
        if (navMeshSurface == null)
        {
            Debug.LogError("NavMeshSurface is not assigned!");
            return;
        }

        navMeshSurface.BuildNavMesh();
    }
    /*
         public void AddObjectToNavMesh(GameObject obj)
    {
        // ���������� ������� �� NavMesh
        NavMeshBuilder.BuildNavMesh();
    }

    public void RemoveObjectFromNavMesh(GameObject obj)
    {
        // �������� ������� � NavMesh
        NavMesh.RemoveNavMeshData(NavMesh.navMeshData);
        NavMeshBuilder.BuildNavMesh();
    }
     */
}
/*
     void Start()
    {
        // ��������� NavMesh ����� ������ ����
        BakeNavMesh();
    }

    void BakeNavMesh()
    {
        NavMeshBuilder.BuildNavMesh();
    }
*/