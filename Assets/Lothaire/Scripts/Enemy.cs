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

    void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, aggroRange);
    }

    public override void Interact()
    {
        Warrior.instance.StartCoroutine(Warrior.instance.StartBattle(pack));
    }

    void Update()
    {
        if (!aggro && Vector2.Distance(transform.position, Warrior.instance.transform.position) < aggroRange)
        {
            foreach(Enemy enemy in pack)
                enemy.Aggro();
        }
    }

    public void Aggro()
    {
        aggro = true;
        StartCoroutine(WalkToTarget(Warrior.instance.transform));
    }

    IEnumerator WalkToTarget(Transform warrior)
    {
        while (Vector2.Distance(transform.position, warrior.transform.position) > 1f)
        {
            Vector2 dir = (warrior.transform.position - transform.position).normalized;
            body.MovePosition((Vector2)transform.position + dir * Warrior.instance.speed * 3 * Time.deltaTime);// Warrior run at double speed when see something, so ennemy at triple so it can catch up
            yield return null;
        }
        yield return new WaitForSeconds(0.3f);
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
        animator.SetBool("Attacking", true);
        StartCoroutine(AttackDelay());
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(WalkToTarget(Warrior.instance.transform)); // If Warrior is going for loot and ignore mob, they have to walk again to catch him
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
