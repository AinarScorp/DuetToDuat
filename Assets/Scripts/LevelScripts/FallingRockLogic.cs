using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO Delete this.
public class FallingRockLogic : MonoBehaviour
{

    public void SetScale(Vector3 passedScale) => transform.localScale = passedScale;
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}
