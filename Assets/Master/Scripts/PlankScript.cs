using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankScript : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Warrior")
        {
            Warrior.instance.isOnPlank = true;
        }
        else if (collision.gameObject.tag == "Player")
        {
            Healer.instance.isOnPlank = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Warrior")
        {
            Warrior.instance.isOnPlank = false;
        }
        else if (collision.gameObject.tag == "Player")
        {
            Healer.instance.isOnPlank = false;
        }
    }
}
