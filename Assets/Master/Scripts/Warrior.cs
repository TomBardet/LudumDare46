using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public int AttackDamage = 10;

    float hp;
    Rigidbody2D body;
    Animator animator;
    
    E_WarriorInterests currentInterests;
    bool busy = false;
    Vector2 exit;
    float TgAngle = 0f;

    public enum WarriorAI {scanning, moveToDoor, moveToTarget, fight, die};
    public WarriorAI AI;

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
        exit = GameObject.FindObjectOfType<DoorExit>().transform.position;

        StartRoom();
    }

    void StartRoom()
    {
        AI = WarriorAI.scanning;
    }

    void Update()
    {
        switch(AI)
        {
            case WarriorAI.scanning:
                if(!busy)
                Scan();
                break;
            case WarriorAI.moveToDoor:
                if (!busy)
                MoveToDoor();
                break;
            case WarriorAI.moveToTarget:
                if (!busy)
                MoveToTg();
                break;
            case WarriorAI.fight:
                if (!busy && enemy != null)
                Fight();
                break;
            case WarriorAI.die:
                break;
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
            if (Vector2.Distance(transform.position, exit) > 1f)
            {
                Vector2 dir = (exit - (Vector2)transform.position).normalized;
                body.MovePosition((Vector2)transform.position + dir * Walkspeed * Time.deltaTime);
                AngleSight(exit);
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

        StopAllCoroutines();
        StartCoroutine(WalkToTarget(Tg_WalkTo.GetComponent<WarriorInteractable>()));
    }

    IEnumerator WalkToTarget(WarriorInteractable target)
    {
        AngleSight(target.transform.position);
        while (Vector2.Distance(transform.position, target.transform.position) > 0.7f)
        {
            Vector2 dir = (target.transform.position - transform.position).normalized;
            body.MovePosition((Vector2)transform.position + dir * chargeSpeed * Time.deltaTime); // *2 because when he see something he run
            yield return null;
        }
        target.Interact();
        yield return new WaitForSeconds(pauseAfterMoveTo); //3 sec after(Animation etc), the warrior start to walk to the door again
        busy = false;
        Tg_WalkTo = null;
        //Après le walkTo, on fait un scan sauf s'il est en combat:
        if(AI != WarriorAI.fight)
        {
            AI = WarriorAI.scanning;
            currentInterests = 0;
        }
    }

    public void GoAfk()
    {
        animator.SetBool("Eating", true);
    }

    public void TakeDamage(int damage, Enemy enemy)
    {
        hp -= damage;
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

    public IEnumerator StartBattle(Enemy[] pack)
    {
        busy = true;
        for (int i = 0; i< pack.Length; i++)
        {
            Enemy target = pack[i];
            Debug.Log("Fighting" + target);
            while(target.hp > 0)
            {
                target.TakeDamage(AttackDamage);
                Debug.Log("Inflict Dmg");
                yield return new WaitForSeconds(timeBetweenEachShot);
            }
            Debug.Log("Kill");
        }
        Debug.Log("End of Pack");
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

        Debug.Log(tg1);

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
        Debug.Log("PAUSE");
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
        Debug.Log("Turning" +TgAngle);
        viewCone.rotation = Quaternion.Euler(0, 0, TgAngle);
    }

    public IEnumerator RotateTowardTarget(Vector2 target, float speed)
    {
        var timer = speed;
        while(timer > 0)
        {
            timer -= Time.deltaTime;

            AngleSight(target);
            yield return new WaitForSeconds(2f);
        }
    }

    public IEnumerator Pause(float time)
    {
        Debug.Log("startpause");
        yield return new WaitForSeconds(time);
    }
}
