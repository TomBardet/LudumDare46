using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : WarriorInteractable
{
    public int hp;
    public float aggroRange;
    public int damage;
    public List<Enemy> pack;
    public float chargeSpeed;
    public float timeBetweenAttacks;

    public Transform hpLayout;
    public GameObject heartPrefab;

    bool aggro = false;
    Animator animator;
    Rigidbody2D body;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        body = GetComponent<Rigidbody2D>();
        interestType = E_WarriorInterests.Enemy;
        if (!pack.Contains(this))
            pack.Add(this);
        for (int i = 0; i < hp; i++)
            Instantiate(heartPrefab, hpLayout);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange - 1);
    }

    public override void Interact()
    {
        Warrior.instance.enemy = this;
        Warrior.instance.AI = Warrior.WarriorAI.fight;
    }

    void Update()
    {
        if (Warrior.instance && !aggro && Vector2.Distance(transform.position, Warrior.instance.transform.position) < aggroRange)
        {
            foreach (Enemy enemy in pack)
                enemy.Aggro();
        }
    }

    public void Aggro()
    {
        aggro = true;
        StartCoroutine(WalkToTarget(Warrior.instance.transform));
        MusicController.instance.PlayAnSFX(MusicController.instance.GoblinTrigger);
    }

    IEnumerator WalkToTarget(Transform warrior)
    {
        animator.SetInteger("Movement", 1);
        while (Vector2.Distance(transform.position, warrior.transform.position) > 2f)
        {
            Vector2 dir = (warrior.transform.position - transform.position).normalized;
            body.MovePosition((Vector2)transform.position + dir * chargeSpeed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        animator.SetInteger("Movement", 0);
        Attack();
    }

    public void TakeDamage(int dmg)
    {
        if (!aggro)
            foreach (Enemy enemy in pack)
                enemy.Aggro();
        hp -= dmg;
        if (hpLayout.childCount > 0)
            Destroy(hpLayout.GetChild(0).gameObject);
        if (hp <= 0)
        {
            StopAllCoroutines();
            animator.SetBool("Dead", true);
            Invoke("Death", 1f);
        }
        else
            Attack();
    }

    void Attack()
    {
        animator.SetBool("Attacking", true);
        Warrior.instance.TakeDamage(damage, this);
        StartCoroutine(AttackDelay());
        MusicController.instance.PlayAnSFX(MusicController.instance.GoblinHit);
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        StartCoroutine(WalkToTarget(Warrior.instance.transform)); // If Warrior is going for loot and ignore mob, they have to walk again to catch him
    }

    void Death()
    {
        MusicController.instance.PlayAnSFX(MusicController.instance.GoblinDeath);
        Destroy(gameObject);
    }
}

