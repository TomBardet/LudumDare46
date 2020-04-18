using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Warrior : MonoBehaviour
{
    public static Warrior instance;

    public float speed;
    public int maxHp;
    public Transform viewCone;

    float hp;
    Rigidbody2D body;
    Animator animator;
    
    E_WarriorInterests currentInterests;
    bool busy = false;
    Vector2 exit;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        
        hp = maxHp;
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentInterests = E_WarriorInterests.None;
    }

    void Start()
    {
        StartRoom();
    }

    void StartRoom()
    {
        exit = GameManager.exit;
    }

    void Update()
    {
        Debug.Log(currentInterests);
        if (!busy && Vector2.Distance(transform.position, exit) > 0.2f)
        {
            Vector2 dir = (exit - (Vector2)transform.position).normalized;
            body.MovePosition((Vector2)transform.position + dir * speed * Time.deltaTime);
            AngleSight(exit);
        }
    }

    public void See(WarriorInteractable interest)
    {
        if (interest.interestType > currentInterests)
        {
            currentInterests = interest.interestType;
            StopAllCoroutines();
            StartCoroutine(WalkToTarget(interest));
        }
    }

    IEnumerator WalkToTarget(WarriorInteractable target)
    {
        AngleSight(target.transform.position);
        busy = true;
        while (Vector2.Distance(transform.position, target.transform.position) > 0.2f)
        {
            Vector2 dir = (target.transform.position - transform.position).normalized;
            body.MovePosition((Vector2)transform.position + dir * speed * 2 * Time.deltaTime); // *2 because when he see something he run
            yield return null;
        }
        target.Interact();
        yield return new WaitForSeconds(3); //3 sec after(Animation etc), the warrior start to walk to the door again
        busy = false;
    }

    public void GoAfk()
    {
        animator.SetBool("Afk", true);
    }

    public void TakeDamage(int damage, Enemy enemy)
    {
        hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
            GameManager.Defeat();
        }
        if (currentInterests == E_WarriorInterests.None)
        {
            See(enemy);
        }
    }

    public void ReceiveHeal(int heal)
    {
        hp += heal;
        if (hp > maxHp)
            hp = maxHp;
    }

    public IEnumerator StartBattle(Enemy[] pack)
    {
        busy = true;
        while (pack.Length > 0)
        {
            Enemy target = pack[0];
            while (target)
            {
                target.TakeDamage();
                yield return new WaitForSeconds(2f);
            }
        }
        busy = false;
    }

    void AngleSight(Vector2 target)
    {
        float AngleRad = Mathf.Atan2(target.y - viewCone.position.y, target.x - viewCone.position.x);
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        viewCone.rotation = Quaternion.Euler(0, 0, AngleDeg);
    }
}
