using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : WarriorInteractable
{
    public bool trapped;

    public void Awake()
    {
        interestType = E_WarriorInterests.Chest;
    }

    public override void Interact()
    {
        Debug.Log("Opening loot chest !");
        interestType = E_WarriorInterests.EmptyChest;
        if (Healer.instance.Tg_grab == GetComponent<GrabableItem>())
            Healer.instance.ReleaseObj(Healer.instance.Tg_grab);
        if (trapped)
            StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(1f);
        Warrior.instance.TakeDamage(10, null);
    }
}