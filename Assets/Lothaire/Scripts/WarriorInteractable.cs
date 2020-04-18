using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_WarriorInterests
{
    //By priority
    None = 0,
    Sandwitch = 1,
    Enemy = 9,
    Chest = 10,
}

public abstract class WarriorInteractable : MonoBehaviour
{
    public abstract void Interact();
    public E_WarriorInterests interestType;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "WarriorSight")
            Warrior.instance.See(this);
    }
}