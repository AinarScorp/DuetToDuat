using System;
using System.Collections;
using System.Collections.Generic;
using MainGame.Keys;
using MainGame.Locks;
using UnityEngine;

// Made by Max Ekberg.

// TODO Currently not used, delete if not used later. 

namespace MainGame.Locks
{
    public class MovingGridLock : TransformLockBase
    {
        GridCreator gridHolder;

        Coroutine moveCoroutine;

        List<PressurePlateDirection> directionInputs;
        Vector2Int currentDir;

        Vector2Int currentIndex = Vector2Int.zero;

        protected override void Awake()
        {
            base.Awake();
            
            gridHolder = GetComponent<GridCreator>();

            directionInputs = new List<PressurePlateDirection>();

            foreach (var key in keys)
            {
                directionInputs.Add(key.GetComponent<PressurePlateDirection>());
            }
        }

        protected override void KeyCheck()
        {
            for (int i = 0; i < keys.Length; i++)
            {
                if (!keys[i].IsActivated) continue;

                currentDir = directionInputs[i].Direction;
                keys[i].SetActive(false);
                return;
            }

            return;
        }

        protected override void HandleOnState()
        {
            if (transformCoroutine != null) return;
            
            if(MoveCheck(currentDir, out Vector3 newPos))
            {
                transformCoroutine = StartCoroutine(TransformLock(transform.localPosition, newPos));
            }
        }

        bool MoveCheck(Vector2Int dir, out Vector3 newPos)
        {
            newPos = Vector3.zero;

            Vector2Int newIndex = currentIndex + dir;

            int newX = newIndex.x;
            int newY = newIndex.y;
            
            bool xInBounds = newX >= 0 && newX < gridHolder.PositionLut.GetLength(0);
            bool yInBounds = newY >= 0 && newY < gridHolder.PositionLut.GetLength(1);
            
            if (!xInBounds || !yInBounds) return false;

            currentIndex = newIndex;
            
            return true;
        }

    }
}