using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBark : MonoBehaviour
{
    public enum E_Entity {Warrior, Healer}
    public string bark;
    public E_Entity triggeredEntity;
    bool on = true;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (on)
        {
            if (col.gameObject.tag == "Warrior" && triggeredEntity == E_Entity.Warrior)
            {
                Warrior.instance.barks.CustomBark(bark);
                on = false;
            }
            else if (col.gameObject.tag == "Player" && triggeredEntity == E_Entity.Healer)
            {
                on = false;
                Healer.instance.barks.CustomBark(bark);
            }
        }
    }
}
