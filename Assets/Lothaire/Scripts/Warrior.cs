using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : MonoBehaviour
{
    public static Warrior instance;

    public float speed;
    public int maxHp;

    float hp;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        
        hp = maxHp;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    IEnumerator WalkToTarget(Transform target)
    {
        while (Vector2.Distance(transform.position, target.position) < 0.1f)
        yield return null;
    }
}
