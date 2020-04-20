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
    EmptyChest = -1,
}

public abstract class WarriorInteractable : MonoBehaviour
{
    public abstract void Interact();
    [HideInInspector]public E_WarriorInterests interestType;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "WarriorSight" && interestType != E_WarriorInterests.Sandwitch)//Sandswitch work on contact not on sight
            Warrior.instance.See(this);
        else if (interestType == E_WarriorInterests.Sandwitch && other.tag == "Warrior")
        {
            Warrior.instance.See(this);
        }
    }
}