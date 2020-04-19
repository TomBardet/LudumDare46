using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabableItem : Interactable
{

    public float WeightDefault;
    public float WeightCarry;
    public float GrabDistance;

    public Transform[] grabpoints;

    public Rigidbody2D rb;
    public bool isCarried;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void CarryBox()
    {
        isCarried = true;
        rb.mass = WeightCarry;
    }

    void ReleaseBox()
    {
        isCarried = false;
        rb.mass = WeightDefault;
    }

    public override void Interact()
    {
        if (isCarried) ReleaseBox();
        else CarryBox();
    }

    public Vector3 FindClosestPoint(Vector3 pos)
    {
        var distance = 0f;
        var memoryDistance = 0f;
        Transform pt = null;

       foreach(Transform go in grabpoints)
        {
            distance = Vector3.Distance(go.position, pos);

            if(distance < memoryDistance || memoryDistance == 0f)
            {
                memoryDistance = distance;
                pt = go;
            }
        }

        if (pt == null)
        {
            Debug.Log("No points found");
            return new Vector3(0,0,0);
        }
        Debug.Log( pt.position);
        return  pt.localPosition;
    }
}
