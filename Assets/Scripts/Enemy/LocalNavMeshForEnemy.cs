using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;
using Unity.AI.Navigation;

public class LocalNavMeshForEnemy : MonoBehaviour
{
    // Размер границ сборки
    [Tooltip("Изменение границ для NavMesh")]
    public Vector3 m_Size = new Vector3(80.0f, 20.0f, 80.0f);

    NavMeshData m_NavMesh;
    Transform enemyTransform;
    AsyncOperation m_Operation;
    NavMeshDataInstance m_Instance;
    List<NavMeshBuildSource> m_Sources = new List<NavMeshBuildSource>();

    void Awake()
    {
        enemyTransform = GetComponent<Transform>();
    }

    void OnEnable()
    {
        // Подписываемся на событие
        NavMeshSourceTag.OnNavMeshSourceTagRemoved += UpdateNavMeshOnObjectRemoval;

        m_NavMesh = new NavMeshData();
        m_Instance = NavMesh.AddNavMeshData(m_NavMesh);
        if (enemyTransform == null)
            enemyTransform = transform;
    }
    void OnDisable()
    {
        // Отписываемся от события
        NavMeshSourceTag.OnNavMeshSourceTagRemoved -= UpdateNavMeshOnObjectRemoval;

        m_Instance.Remove();
    }

    public void UpdateNavMesh()
    {
        NavMeshSourceTag.Collect(ref m_Sources);
        var defaultBuildSettings = NavMesh.GetSettingsByID(0);
        var bounds = QuantizedBounds();
        m_Operation = NavMeshBuilder.UpdateNavMeshDataAsync(m_NavMesh, defaultBuildSettings, m_Sources, bounds);
    }

    static Vector3 Quantize(Vector3 v, Vector3 quant)
    {
        float x = quant.x * Mathf.Floor(v.x / quant.x);
        float y = quant.y * Mathf.Floor(v.y / quant.y);
        float z = quant.z * Mathf.Floor(v.z / quant.z);
        return new Vector3(x, y, z);
    }

    Bounds QuantizedBounds()
    {
        var center = enemyTransform ? enemyTransform.position : transform.position;
        return new Bounds(Quantize(center, 0.1f * m_Size), m_Size);
    }

    void OnDrawGizmosSelected()
    {
        if (m_NavMesh)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(m_NavMesh.sourceBounds.center, m_NavMesh.sourceBounds.size);
        }

        Gizmos.color = Color.yellow;
        var bounds = QuantizedBounds();
        Gizmos.DrawWireCube(bounds.center, bounds.size);

        Gizmos.color = Color.green;
        var center = enemyTransform ? enemyTransform.position : transform.position;
        Gizmos.DrawWireCube(center, m_Size);
    }

    // Добавьте этот метод для обновления NavMesh при удалении объекта
    public void UpdateNavMeshOnObjectRemoval()
    {
        UpdateNavMesh();
    }
}