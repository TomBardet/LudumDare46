using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class LevierBehavior : Interactable
{
    public bool Power;
    public PlayableDirector TimelinePowerOn;
    public PlayableDirector TimelinePowerOff;

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public override void Interact()
    {
        if (isInteractingWith) return;

        Power = !Power;

        if (Power)
        {
            isInteractingWith = true;
            if (TimelinePowerOn == null) Debug.Log("Pas de timeline power on !");

            TimelinePowerOn.Play();
        }
        else
        {
            isInteractingWith = true;
            if (TimelinePowerOff == null) Debug.Log("Pas de timeline power off !");

            TimelinePowerOff.Play();
        }

        anim.SetTrigger("interact");
        MusicController.instance.PlayAnSFX(MusicController.instance.Lever);
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
        isInteractingWith = false ;
    }
}
