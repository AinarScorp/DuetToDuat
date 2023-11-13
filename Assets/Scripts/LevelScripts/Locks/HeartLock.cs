using System;
using System.Collections;
using System.Collections.Generic;
using MainGame.Locks;
using UnityEngine;

public class HeartLock : LockBase
{
    [SerializeField] GameObject heartPrefab;

    [SerializeField] Rigidbody currentSpawnedObjectRb;
    
    protected override void HandleOnState()
    {
        if (currentSpawnedObjectRb != null)
        {
            currentSpawnedObjectRb.GetComponent<Rigidbody>().isKinematic = false;
        }
        else
        {
            currentSpawnedObjectRb = Instantiate(heartPrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        }
        
    }
}
