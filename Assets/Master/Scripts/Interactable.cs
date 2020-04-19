using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool isInRange;
    public float triggerSize;
    public bool isInteractingWith;

    public abstract void Interact();
    public abstract void StopInteract();

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
