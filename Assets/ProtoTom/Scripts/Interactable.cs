using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool isInRange;
    public float triggerSize;

    public abstract void Interact();

    private void Update()
    {
        InteractibleFdbck();
    }

    void InteractibleFdbck()
    {
        if (isInRange)
            transform.localScale = new Vector3(1.02f, 1.02f, 1.02f);
        else
            transform.localScale = new Vector3(1f, 1f, 1f);
    }

   
}
