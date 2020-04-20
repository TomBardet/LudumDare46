using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZOrderRendererSorter : MonoBehaviour
{
    public int sortingOrderBase = 5000;
    public int offset;
    public bool  isUnmovingObject;

    float refreshTime;
    float refreshMaxTime;
    Renderer[] r;


    void Awake()
    {
        r = GetComponentsInChildren<Renderer>();
    }

    //void LateUpdate()
    //{
    //    foreach (Renderer render in r)
    //    {
    //        render.sortingOrder = (int)(sortingOrderBase - transform.position.y - offset);
    //    }
    //    if (isUnmovingObject) Destroy(this);
    //}
}
