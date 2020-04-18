using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : WarriorInteractable
{
    public int hp;
    public float aggroRange;
    public Enemy[] pack;

    bool aggro = false;
    Animator animator;
    Rigidbody2D body;
    
    void Awake()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
    }

    void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, aggroRange);
    }

    public override void Interact()
    {
        StartCoroutine(Warrior.instance.StartBattle(pack));
    }

    void Update()
    {
        if (!aggro &&  Vector2.Distance(transform.position, Warrior.instance.transform.position) < aggroRange)
        {
            aggro = true;
            StartCoroutine(WalkToTarget(Warrior.instance.transform));
        }
    }

    IEnumerator WalkToTarget(Transform warrior)
    {
        while (Vector2.Distance(transform.position, warrior.transform.position) > 0.1f) // range must be smaller than warrior
        {
            Vector2 dir = (warrior.transform.position - transform.position).normalized;
            body.MovePosition((Vector2)transform.position + dir * Warrior.instance.speedOnSight * Time.deltaTime);
            yield return null;
        }
        Attack();
    }

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
        Warrior.instance.TakeDamage(1, this);
        StartCoroutine(AttackDelay());
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(WalkToTarget(Warrior.instance.transform));
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
