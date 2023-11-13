using System;
using System.Collections;
using System.Collections.Generic;
using MainGame.Keys;
using MainGame.Spirit;
using UnityEngine;

[RequireComponent(typeof(PossessibleItem))]
public class PossesableSpiritKey : Key
{
    protected override void Start()
    {
        base.Start();
        
        PossessibleItem possessibleItem = GetComponent<PossessibleItem>();
        possessibleItem.OnPossessed += () => {SetActive(true); };
        possessibleItem.OnUnPossessed += () =>
        {
            Destroy(this);
            possessibleItem.DestroyThis();
        };
    }
    
}
