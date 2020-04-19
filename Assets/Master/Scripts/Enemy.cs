using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : WarriorInteractable
{
    public int hp;
    public float aggroRange;
    public List<Enemy> pack;
    public float chargeSpeed;
    public float timeBetweenEachAttack;
    public GameObject heartPrefab;
    public Transform layout;
    bool aggro = false;
    Animator animator;
    Rigidbody2D body;
    
    void Awake()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        interestType = E_WarriorInterests.Enemy;
        for(int i = 0; i < hp; i++)
            Instantiate(heartPrefab, layout);
        if (!pack.Contains(this))
            pack.Add(this);
        if (chargeSpeed < Warrior.instance.chargeSpeed)
            Debug.Log("Attention, ennemi plus lent que le warrior, il ne pourra pas le chase si le warrior va vers un coffre");
    }

    void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, aggroRange);
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
        Debug.Log("Enemy is aggro!");
        aggro = true;
        StopAllCoroutines();
        StartCoroutine(WalkToTarget(Warrior.instance.transform)); 
    }

    IEnumerator WalkToTarget(Transform warrior)
    {
        while (Vector2.Distance(transform.position, warrior.transform.position) > 2f)
        {
            Vector2 dir = (warrior.transform.position - transform.position).normalized;
            body.MovePosition((Vector2)transform.position + dir * chargeSpeed * Time.deltaTime);// Warrior run at double speed when see something, so ennemy at triple so it can catch up
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);// So that warrior attack first
        Attack();
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        for(int i = 0; i < dmg; i++)
            if(layout.transform.childCount > 0)
                Destroy(layout.GetChild(0).gameObject);
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
        Debug.Log("Goblin attack");
        Warrior.instance.TakeDamage(10, this);
        StartCoroutine(AttackDelay());
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(timeBetweenEachAttack);
        StartCoroutine(WalkToTarget(Warrior.instance.transform)); // If Warrior is going for loot and ignore mob, they have to walk again to catch him
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
