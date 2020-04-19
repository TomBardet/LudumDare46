using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class ManivelleBehavior : Interactable
{
    public float duration;
    public float progress;

    public PlayableDirector TimelineManivelle;

    public override void Interact()
    {
        Debug.Log("test");
    }

    public override void InteractibleFdbck()
    {
        if (isInRange)
            transform.localScale = new Vector3(startsize.x + .07f, startsize.x + .07f, startsize.x + .07f);
        else
            transform.localScale = startsize;
    }

    public override void StopInteract()
    {
        throw new System.NotImplementedException();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
