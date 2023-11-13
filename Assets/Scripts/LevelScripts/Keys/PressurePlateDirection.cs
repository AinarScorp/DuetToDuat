using System.Collections;
using System.Collections.Generic;
using MainGame.Keys;
using UnityEngine;

// Made by Max Ekberg.

//TODO Delete this.
public class PressurePlateDirection : MonoBehaviour
{
    [SerializeField] private Vector2Int direction = Vector2Int.zero;
    
    public Vector2Int Direction { get => direction; }
}
