using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Warrior : MonoBehaviour
{
    public static Warrior instance;

    public float speedOnSight;//Not basic speed, its the speed when he see enemy/chest (Interactable)
    public int maxHp;

    float hp;
    Rigidbody2D body;
    Animator animator;
    NavMeshAgent agent;

    E_WarriorInterests currentInterests;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        
        hp = maxHp;
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        currentInterests = E_WarriorInterests.None;
    }

    void Start()
    {
        StartRoom();
    }

    void StartRoom()
    {
        agent.isStopped = false;
        agent.SetDestination((Vector2)transform.position + Vector2.up * 10);  //TMP destination
    }

    public void See(WarriorInteractable interest)
    {
        if (interest.interestType > currentInterests)
        {
            currentInterests = interest.interestType;
            agent.isStopped = true;
            StopAllCoroutines();
            StartCoroutine(WalkToTarget(interest));
        }
    }

    IEnumerator WalkToTarget(WarriorInteractable target)
    {
        while (Vector2.Distance(transform.position, target.transform.position) > 0.1f)
        {
            Vector2 dir = (target.transform.position - transform.position).normalized;
            body.MovePosition((Vector2)transform.position + dir * speedOnSight * Time.deltaTime);
            yield return null;
        }
        target.Interact();
        yield return new WaitForSeconds(3); //3 sec after(Animation etc), the warrior start to walk to the door again
        agent.isStopped = false;
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
