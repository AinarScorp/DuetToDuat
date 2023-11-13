using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = Unity.Mathematics.Random;

//TODO Delete this.
public class FallingRockSpawner : TimerBase
{

    [SerializeField] private GameObject rockPrefab;

    [SerializeField] private Vector2 minPosDelta;
    [SerializeField] private Vector2 maxPosDelta;
    
    [SerializeField] private Vector2 spawnTimeRange = new Vector2(2, 5);
    [SerializeField] private Vector2Int spawnAmountRange = new Vector2Int(2, 5);
    
    [SerializeField] private Vector2 sizeRange = new Vector2(1, 5);

    private Vector3 minPos;
    private Vector3 maxPos;

    private void Awake()
    {
        minPos = new Vector3(transform.position.x - minPosDelta.x, transform.position.y,
            transform.position.z - minPosDelta.y);
        
        maxPos = new Vector3(transform.position.x + maxPosDelta.x, transform.position.y,
            transform.position.z - maxPosDelta.y);
    }


    private void StartTimer()
    {
        if (timerCoroutine != null) return;

        float newSpawnTime = UnityEngine.Random.Range(spawnTimeRange.x, spawnTimeRange.y);
        timerCoroutine = StartCoroutine(Timer(newSpawnTime));
    }
    
    private void Update()
    {
        
        if(timerDone) OnTimerDone();
        
        if (timerCoroutine != null) return;
        StartTimer();
    }


    protected override void OnTimerDone()
    {
        base.OnTimerDone();
        SpawnRocks();
    }

    private void SpawnRocks()
    {
        Vector3 spawnPos = GetSpawnPos();
        
        Instantiate(rockPrefab, spawnPos, quaternion.identity);

        timerCoroutine = null;
    }

    private Vector3 GetSpawnPos()
    {
        float spawnX = UnityEngine.Random.Range(minPos.x, maxPos.x);
        float spawnZ = UnityEngine.Random.Range(minPos.z, maxPos.z);

        Vector3 finalPos = new Vector3(spawnX, transform.position.y, spawnZ);

        return finalPos;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;

        Vector3 minPos = new Vector3(transform.position.x - minPosDelta.x, transform.position.y,
            transform.position.z - minPosDelta.y);
        
        Vector3 maxPos = new Vector3(transform.position.x + maxPosDelta.x, transform.position.y,
            transform.position.z - maxPosDelta.y);
        
        Gizmos.DrawLine(minPos, maxPos);

    }
}
