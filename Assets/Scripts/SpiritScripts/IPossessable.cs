using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Made by Einar Hallik
public interface IPossessable
{
    Transform GetShoulder();
    void VisualisePossession();
    
    void Possess();
    void UnPossess();
}
