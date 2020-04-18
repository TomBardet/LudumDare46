using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_WarriorInterests
{
    None = 0,
    Enemy = 1,
    Chest = 2,
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