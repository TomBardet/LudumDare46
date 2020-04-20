using System.Collections;
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
    public GameObject manivelleImg;
    private FMOD.Studio.PLAYBACK_STATE playbackState;
    public override void Interact()
    {
        used = true;
        if(progress<duration)
        {
            progress += InteractSpeed * Time.deltaTime;

            //animation de tourner lentement:
            manivelleImg.transform.Rotate(new Vector3(0, 0, 1), -1);
        }
        MusicController.instance.manivelle.getPlaybackState(out playbackState);
        
        if (playbackState != FMOD.Studio.PLAYBACK_STATE.PLAYING) {
            MusicController.instance.manivelle.setParameterByName("IsManivelle", 1);
            MusicController.instance.manivelle.start();
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
        MusicController.instance.manivelle.setParameterByName("IsManivelle", 0);
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
            manivelleImg.transform.Rotate(new Vector3(0, 0, 1), 2.5f);
        }
        PlayTimeline();
    }
}
