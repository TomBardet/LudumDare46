using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool isInRange;
    public float triggerSize;

    public abstract void Interact();

    Vector3 startsize;
    private void Awake()
    {
        startsize = transform.localScale;
    }

    private void Update()
    {
        InteractibleFdbck();
    }

    void InteractibleFdbck()
    {
        if (isInRange)
            transform.localScale = new Vector3(startsize.x + .02f, startsize.x + .02f, startsize.x + .02f);
        else
            transform.localScale = startsize;
    }

   
}
