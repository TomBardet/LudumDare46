using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTrigger : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger + " + collision.gameObject.tag + " its = " + collision.gameObject.name);
        if (collision.gameObject.tag == "Warrior")
            Warrior.instance.TakeDamage(500, null);
        else if (collision.gameObject.tag == "Player")
            Healer.instance.Dead();
        else if (collision.gameObject.GetComponent<Enemy>())
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(50);
        }
    }
}
