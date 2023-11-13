using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Made by Max Ekberg.
namespace MainGame.Locks
{
    public class ComponentLock : LockBase
    {
        [SerializeField] GameObject[] objectsToToggle;

        // Disables the objects to toggle.
        protected override void HandleOnState()
        {
            foreach (var obj in objectsToToggle)
            {
                obj.SetActive(!obj.activeSelf);
            }
        }

        // Enables the objects to toggle.
        protected override void HandleOffState()
        {
            foreach (var obj in objectsToToggle)
            {
                obj.SetActive(!obj.activeSelf);
            }
        }
    }
}
