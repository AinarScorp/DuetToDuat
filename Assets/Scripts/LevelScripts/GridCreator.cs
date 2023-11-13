using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Made by Max Ekberg.
//TODO Delete this.
public class GridCreator : MonoBehaviour
{

    [SerializeField] private Transform gridBlockTransform;
    
    private Vector2[,] positionLut = new Vector2[3,3];

    public Vector2[,] PositionLut { get => positionLut; }
    
    // Start is called before the first frame update
    void Awake()
    {
        CalculateGridPositions();
    }

    private void CalculateGridPositions()
    {
        float gridSizeX = gridBlockTransform.localScale.x;
        float gridSizeY = gridBlockTransform.localScale.z;

        float cubeSizeX = transform.localScale.x;
        float cubeSizeY = transform.localScale.z;

        for (int y = 0; y < (int)gridSizeY / (int)cubeSizeY; y++)
        {
            for (int x = 0; x < (int)gridSizeX / (int)cubeSizeX; x++)
            {
                positionLut[x, y] = new Vector2(transform.localPosition.x, transform.localPosition.z) + new Vector2(x * cubeSizeX, y * cubeSizeY);
            }
        }

    }
}
