using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSight : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<WarriorInteractable>() != null)
        {
            //Raycast to see if there is a wall / darkness
            RaycastHit2D hit = Physics2D.Raycast(Warrior.instance.transform.position, (other.transform.position - Warrior.instance.transform.position)
                                , Vector2.Distance(Warrior.instance.transform.position, other.transform.position), LayerMask.GetMask("Obstacle"));
            if (hit.collider != null)
                Debug.Log("Obstacle between target and warrior !");
            else
                Warrior.instance.See(other.GetComponent<WarriorInteractable>());
        }
    }
}
