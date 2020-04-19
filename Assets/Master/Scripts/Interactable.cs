using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool isInRange;
    public float triggerSize;

    public abstract void Interact();

    public Vector3 startsize;
    private void Awake()
    {
        startsize = transform.localScale;
    }

    private void Update()
    {
        InteractibleFdbck();
    }

    public abstract void InteractibleFdbck();


   
}
