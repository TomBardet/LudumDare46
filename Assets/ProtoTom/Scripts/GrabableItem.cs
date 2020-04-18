using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabableItem : Interactable
{

    public float WeightDefault;
    public float WeightCarry;
    public float GrabDistance;

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
}
