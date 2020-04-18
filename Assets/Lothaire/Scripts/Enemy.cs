using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : WarriorInteractable
{
    public int hp;
    public float aggroRange;

    bool aggro = false;
    Animator animator;
    
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void Interact()
    {
        
    }

    // void Update()
    // {
    //     if (!aggro &&  Vector2.Distance(transform.position, Warrior.instance.transform.position) < aggroRange)
    //     {
    //         aggro = true;
    //         S
    //     }
    // }

    public void TakeDamage()
    {
        hp -= 1;
        if (hp <= 0)
            Death();
        else
            Attack();
    }

    void Attack()
    {
        Warrior.instance.TakeDamage(1);
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
