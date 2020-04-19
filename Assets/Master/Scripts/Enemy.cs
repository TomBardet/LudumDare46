using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : WarriorInteractable
{
    public int hp;
    public float aggroRange;
    public List<Enemy> pack;
    public float chargeSpeed;

    public Transform hpLayout;
    public GameObject heartPrefab;

    bool aggro = false;
    Animator animator;
    Rigidbody2D body;
    
    void Awake()
    {
        animator = GetComponent<Animator>();
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
        while (Vector2.Distance(transform.position, warrior.transform.position) > 2f)
        {
            Vector2 dir = (warrior.transform.position - transform.position).normalized;
            body.MovePosition((Vector2)transform.position + dir * chargeSpeed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        Attack();
    }

    public void TakeDamage()
    {
        if (!aggro)
            foreach(Enemy enemy in pack) 
                enemy.Aggro();
        hp -= 1;
        if (hpLayout.childCount > 0)
            Destroy(hpLayout.GetChild(0).gameObject);
        if (hp <= 0)
        {
            StopAllCoroutines();    
            Invoke("Death", 1f);
        }
        else
            Attack();
    }

    void Attack()
    {
        Warrior.instance.TakeDamage(10, this);
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
