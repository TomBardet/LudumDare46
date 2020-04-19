using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class PressurePlaque : MonoBehaviour
{
    public bool Power;
    public PlayableDirector TimelinePowerOn; 
    public PlayableDirector TimelinePowerOff;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Warrior"))
        {
            Power = true;

            if (TimelinePowerOn == null) Debug.Log("Pas de timeline power on !");

            TimelinePowerOn.Play();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Warrior"))
        {
            Power = false;

            if (TimelinePowerOff == null) Debug.Log("Pas de timeline power off !");

            TimelinePowerOff.Play();
        }
    }

}
