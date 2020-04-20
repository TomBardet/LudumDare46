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

    public GameObject On;
    public GameObject Off;

    private void Start()
    {
        On.SetActive(false);
        Off.SetActive(true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var c = collision.GetComponent<GrabableItem>();
        if (collision.CompareTag("Warrior") ||c!= null && !c.isplank)
        {
            Power = true;
            On.SetActive(true);
            Off.SetActive(false);

            if (TimelinePowerOn == null) Debug.Log("Pas de timeline power on !");

            TimelinePowerOn.time = 0;
            TimelinePowerOff.Stop();
            TimelinePowerOn.Play();
            Debug.Log("playd");

        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var c = collision.GetComponent<GrabableItem>();
        if (collision.CompareTag("Warrior") || c != null && !c.isplank)
        {
            Power = false;
            On.SetActive(false);
            Off.SetActive(true);

            if (TimelinePowerOff == null) Debug.Log("Pas de timeline power off !");

            TimelinePowerOff.time = 0;
            TimelinePowerOn.Stop();

            TimelinePowerOff.Play();
            Debug.Log("play");
        }
    }

}
