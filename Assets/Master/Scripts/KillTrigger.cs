using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTrigger : MonoBehaviour
{
    public bool isActive;


    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Warrior")
        {
            if (!Warrior.instance.isOnPlank) Warrior.instance.TakeDamage(500, null);
        }
        else if (collision.gameObject.tag == "Player")
        {
            if (!Healer.instance.isOnPlank)
                Healer.instance.Dead();
        }
        else if (collision.gameObject.GetComponent<Enemy>())
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(50);
        }
    }


}
