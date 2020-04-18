using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : MonoBehaviour
{
    public static Warrior instance;

    public float speed;
    public int maxHp;
    public float sightRange;

    float hp;
    Rigidbody2D body;
    Animator animator;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        
        hp = maxHp;
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        DetectTargets();
    }

    public void DetectTargets()
    {
        Collider2D[] inRange = Physics2D.OverlapCircleAll(transform.position, sightRange, LayerMask.GetMask("WarriorInteractable"));    

        if (inRange.Length > 0)
        {
            //Priorite = Le plus pres, ou un type priorise ?
            if (inRange[0].GetComponent<WarriorInteractable>() == null)
                Debug.Log("Problem no component attached");
            WarriorInteractable target = inRange[0].GetComponent<WarriorInteractable>();
            StartCoroutine(WalkToTarget(target));
        }
        else //Nothing found;
        {
            Debug.Log("Nothing to interact with");
        }
    }

    IEnumerator WalkToTarget(WarriorInteractable target)
    {
        while (Vector2.Distance(transform.position, target.transform.position) > 0.1f)
        {
            Vector2 dir = (target.transform.position - transform.position).normalized;
            body.MovePosition((Vector2)transform.position + dir * speed * Time.deltaTime);
            yield return null;
        }


        target.Interact();
    }

    public void GoAfk()
    {
        animator.SetBool("Afk", true);
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
            GameManager.Defeat();
        }
    }

    public void ReceiveHeal(int heal)
    {
        hp += heal;
        if (hp > maxHp)
            hp = maxHp;
    }
}
