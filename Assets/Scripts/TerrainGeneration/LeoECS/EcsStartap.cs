using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;
using Enemy;

public class EcsStartap : MonoBehaviour
{
    private EcsWorld ecsWorld;
    private EcsSystems startSystems;
    public LocalNavMeshForEnemy LocalNavMeshForEnemy;

    //
    public GameObject enemy;
    //
    private void Start()
    {
        ecsWorld = new EcsWorld();
        startSystems = new EcsSystems(ecsWorld);

        startSystems.Add(new TerrainGenerationSystem()).Init();
        LocalNavMeshForEnemy.UpdateNavMesh();

        StartCoroutine(Delay());

    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(3);
        enemy.GetComponent<EnemyCharacter>().enabled = true;
        enemy.SetActive(true);
    }
    private void OnDestroy()
    {
        ecsWorld.Destroy();
        ecsWorld = null;
        startSystems.Destroy();
        startSystems = null;
    }
}
