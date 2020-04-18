using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : WarriorInteractable
{
    public bool trapped;

    public override void Interact()
    {
        Debug.Log("Opening loot chest !");
        if (trapped)
            StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(1f);
        Warrior.instance.TakeDamage(10, null);
    }
}