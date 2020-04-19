﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Warrior : MonoBehaviour
{
    public static Warrior instance;

    [Header("General")]
    public int maxHp;
    public Transform viewCone;

    [Header("Scan")]
    public float scanSpeed = 1f;
    public float scanDuration = 5f;
    public float pauseAfterscan = 1f;

    [Header("MoveToTargets")]
    public Transform Tg_WalkTo;
    public float chargeSpeed = 1f;
    public float pauseAfterMoveTo = 2f;

    [Header("MoveToDoor")]
    public float Walkspeed = .5f;

    [Header("Fighting")]
    public Enemy enemy;
    public float timeBetweenEachShot = 1f;
    //public int AttackDamage = 10;//On devrait laisser 1 vis a vis des gobelins non ? Sans variable, en dur

    float hp;
    Rigidbody2D body;
    Animator animator;
    
    E_WarriorInterests currentInterests;
    bool busy = false;
    Vector2 exit;
    WarriorInteractable destination;
    Coroutine walking;
    float TgAngle = 0f;

    public Slider healthBar;
    public enum WarriorAI {scanning, moveToDoor, moveToTarget, fight, die};
    public WarriorAI AI;

    Barks barks;

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
        walking = null;
        barks = GetComponentInChildren<Barks>();
    }

    void Start()
    {
        exit = GameObject.FindObjectOfType<DoorExit>().transform.position;
        StartRoom();
    }

    void StartRoom()
    {
        AI = WarriorAI.scanning;
    }

    void Update()
    {
        if (!busy)
        {
            switch(AI)
            {
                case WarriorAI.scanning:
                    Scan();
                    break;
                case WarriorAI.moveToDoor:
                    MoveToDoor();
                    break;
                case WarriorAI.moveToTarget:
                    MoveToTg();
                    break;
                case WarriorAI.fight:
                    if (enemy != null)
                        Fight();
                    break;
                case WarriorAI.die: // peut on l'enelver et simplement call die dans take damage comme un event ?
                    break;
            }
        }
    }

    //appelé si un obj dans le champ de vision
    public void See(WarriorInteractable interest)
    {
        if (interest.interestType > currentInterests)
        {
            currentInterests = interest.interestType;
            Tg_WalkTo = interest.transform;
        }
    }

    public void MoveToDoor()
    {
        //on vérifie si il n'as pas de tg walk to
        if(Tg_WalkTo == null)
        {
            Vector2 dir;
            if (transform.position.x < exit.x - 4)
                dir = Vector2.right;
            else if (transform.position.y > exit.y + 0.1f)
                dir = Vector2.down;
            else if (transform.position.y < exit.y - 0.1f)
                dir = Vector2.up;
            else
                dir = (exit - (Vector2)transform.position).normalized;
            if (Vector2.Distance(transform.position, exit) > 1f)
            {
                body.MovePosition((Vector2)transform.position + dir * Walkspeed * Time.deltaTime);
                viewCone.right = (Vector3)exit - viewCone.position;
            }
        }
        else
        {
            AI = WarriorAI.moveToTarget;
        }
        
    }
    public void MoveToTg()
    {
        busy = true;
        WarriorInteractable newDestination = Tg_WalkTo.GetComponent<WarriorInteractable>();
        
        if (newDestination.interestType == E_WarriorInterests.Sandwitch)
            barks.ScreamBark(E_Barks.Sandswitch);
        else if (newDestination.interestType == E_WarriorInterests.Chest)
            barks.ScreamBark(E_Barks.Chest);
        else if (newDestination.interestType == E_WarriorInterests.Enemy)
            barks.ScreamBark(E_Barks.Enemies);
        Debug.Log(walking);
        destination = newDestination;
        if (walking == null)
            walking = StartCoroutine(WalkToTarget());
    }

    IEnumerator WalkToTarget()
    {
        AngleSight(destination.transform.position);
        while (Vector2.Distance(transform.position, destination.transform.position) > 2f)
        {
            Vector2 dir = (destination.transform.position - transform.position).normalized;
            body.MovePosition((Vector2)transform.position + dir * chargeSpeed * Time.deltaTime); // *2 because when he see something he run
            yield return null;
        }
        destination.Interact();
        busy = false;
        Tg_WalkTo = null;
        //Après le walkTo, on fait un scan sauf s'il est en combat:
        if(AI != WarriorAI.fight)
        {
            AI = WarriorAI.scanning;
            currentInterests = 0;
            yield return new WaitForSeconds(pauseAfterMoveTo); //3 sec after(Animation etc), the warrior start to walk to the door again
        }
        walking = null;
    }

    public void GoAfk()
    {
        animator.SetBool("Eating", true);
    }

    public void TakeDamage(int damage, Enemy enemy)
    {
        hp -= damage;
        healthBar.value = hp / maxHp;
        if (hp <= 0)
        {
            hp = 0;
            GameManager.Defeat();
        }
        if (currentInterests == E_WarriorInterests.None && enemy != null)
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

    public void Fight()
    {
        busy = true;
        StopAllCoroutines();
        StartCoroutine(StartBattle(enemy.pack));
    }

    public IEnumerator StartBattle(List<Enemy> pack)
    {
        busy = true;
        for (int i = 0; i< pack.Count; i++)
        {
            Enemy target = pack[i];
            //Debug.Log("Fighting" + target);
            while(target.hp > 0)
            {
                target.TakeDamage();
                //Debug.Log("Inflict Dmg");
                yield return new WaitForSeconds(timeBetweenEachShot);
            }
            Debug.Log("Kill");
        }
        //Debug.Log("End of Pack");
        busy = false;

        //à la fin d'un combat, retour au scan:
        AI = WarriorAI.scanning;
    }

    void AngleSight(Vector2 target)
    {
        float AngleRad = Mathf.Atan2(target.y - viewCone.position.y, target.x - viewCone.position.x);
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        viewCone.rotation = Quaternion.Euler(0, 0, AngleDeg);
    }

    public void Scan()
    {
        busy = true;
        StopAllCoroutines();
        StartCoroutine(ScanCoroutine());
    }

    public IEnumerator ScanCoroutine()
    {
        Vector2 tg1 = (Vector2)transform.position + Vector2.up;
        Vector2 tg2 = (Vector2)transform.position - Vector2.up;
        
        barks.ScreamBark(E_Barks.Scanning);
        var timer = scanDuration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;

            RotateToward(tg1);
            yield return new WaitForEndOfFrame();
        }

        timer = scanDuration * 2;
        while (timer > 0)
        {
            timer -= Time.deltaTime;

            RotateToward(tg2);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(pauseAfterscan);

        busy = false;
        //si il a trouvé une target
        if(Tg_WalkTo != null)
        {
            AI = WarriorAI.moveToTarget;
        }
        else //si il n'a rien trouvé il se dirige vers la porte
        {
            AI = WarriorAI.moveToDoor;
        }
    }

    void RotateToward(Vector2 lookAt)
    {
        float AngleRad = Mathf.Atan2(lookAt.y - viewCone.position.y, lookAt.x - viewCone.position.x);
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        TgAngle = Mathf.MoveTowardsAngle(TgAngle, AngleDeg, scanSpeed);
        viewCone.rotation = Quaternion.Euler(0, 0, TgAngle);
    }
}
