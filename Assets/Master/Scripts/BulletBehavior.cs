using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float speed;
    Rigidbody2D rb;
    public float dmg;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Invoke("KillMe", 5f);

        rb.velocity = transform.up * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Warrior")
            Warrior.instance.TakeDamage(500, null);
        else if (collision.gameObject.tag == "Player")
            Healer.instance.Dead();
        Destroy(gameObject);
    }

    void KillMe()
    {
        Destroy(gameObject);
    }
}
