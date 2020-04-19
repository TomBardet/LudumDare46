﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class ManivelleBehavior : Interactable
{
    public double duration;
    public float progress;
    public float InteractSpeed;
    public float revertSpeed;
    public PlayableDirector TimelineManivelle;
    bool used;
    public override void Interact()
    {
        used = true;
        if(progress<duration)
        {
            progress += InteractSpeed * Time.deltaTime;
            //progress = progress / (float)duration;

        }
    }

    void PlayTimeline()
    {
        TimelineManivelle.Play();
        TimelineManivelle.time = progress;
    }

    public override void InteractibleFdbck()
    {
        if (isInRange)
            transform.localScale = new Vector3(startsize.x + .17f, startsize.x + .17f, startsize.x + .17f);
        else
            transform.localScale = startsize;
    }

    public override void StopInteract()
    {
        used = false;
    }

    void Start()
    {
        if (TimelineManivelle == null) Debug.LogError("Error pas de timeline");
        duration = TimelineManivelle.duration;
    }

    void Update()
    {
        if (progress > 0 && !used)
        {
            progress -= revertSpeed * Time.deltaTime;
        }
        PlayTimeline();


    }
}