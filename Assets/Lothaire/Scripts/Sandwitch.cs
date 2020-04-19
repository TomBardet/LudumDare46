using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sandwitch : WarriorInteractable
{
    public void Awake()
    {
        interestType = E_WarriorInterests.Sandwitch;
    }

    public override void Interact()
    {
        Debug.Log("Eating sandswitch !");
        Warrior.instance.GetComponent<Animator>().SetBool("Eating", true);
        Destroy(gameObject);
    }
}