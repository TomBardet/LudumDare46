using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healer : MonoBehaviour
{
    public static Healer instance;
    SpringJoint2D joints;
    RelativeJoint2D relativJoints;
    LineRenderer line;

    public bool isGrabbing;
    public GrabableItem Tg_grab;
    public Vector2 offsetRope;


    public float manaMax;
    public float manaRegenPerSec;
    public float manaRegenDelay;
    public float healCost;
    public int healEffect;
    Slider manaBar;
    float currentMana;
    bool isRegenerating;
    public bool isOnPlank;
    public ParticleSystem particlesHeal;
    [HideInInspector]public bool dead;
    [HideInInspector]public Barks barks;

    void Awake()
    {
        instance = this;
        joints = GetComponent<SpringJoint2D>();
        relativJoints = GetComponent<RelativeJoint2D>();
        line = GetComponent<LineRenderer>();
        joints.enabled = false;
        line.enabled = false;
        currentMana = manaMax;
        barks = GetComponentInChildren<Barks>();
    }

    void Start()
    {
        manaBar = UIRef.instance.manaBar;
        currentMana = manaMax;
        manaBar.value = currentMana / manaMax;
    }

    private void Update()
    {
        if (isGrabbing && Tg_grab != null)
        {
            UpdateRope();
        }

        if (isRegenerating)
        {
            currentMana += Time.deltaTime * manaRegenPerSec;
            if (currentMana > manaMax)
                currentMana = manaMax;
        }
        manaBar.value = currentMana / manaMax;
        if (Input.GetKeyDown(KeyCode.Space))
            Heal();
    }
    public void Heal()
    {
        if (currentMana >= healCost)
        {
            currentMana -= healCost;
            Warrior.instance.ReceiveHeal(healEffect);
            particlesHeal.Play();
            StopAllCoroutines();
            StartCoroutine(RegenDelay());
            MusicController.instance.PlayAnSFX(MusicController.instance.Heal);
        }
    }

    public void Dead()
    {
        GetComponentInChildren<Animator>().SetBool("Dead", true);
        if(!dead)
        {
            dead = true;
            MusicController.instance.PlayAnSFX(MusicController.instance.HealerDeath);
            Invoke("Defeat", 2f);

        }
    }

    void Defeat()
    {
        GameManager.Defeat();
    }

    IEnumerator RegenDelay()
    {
        float delay = manaRegenDelay;
        isRegenerating = false;
        while (delay > 0)
        {
            delay -= 1 * Time.deltaTime;
            yield return null;
        }
        isRegenerating = true;
    }

    public void Interact(string tag, Interactable _obj)
    {
        switch(tag)
        {
            case "Moveable":
                Debug.Log("interact");
                AttachToObject(_obj);
                break;
            default:
                break;
        }
    }

    public void StopInteract(string tag, Interactable _obj)
    {
        switch (tag)
        {
            case "Moveable":
                ReleaseObj(_obj);
                break;
            default:
                break;
        }
    }

    void AttachToObject(Interactable _obj)
    {
        Tg_grab = _obj.GetComponent<GrabableItem>();
        if (Tg_grab == null) Debug.Log("Error récupération target de grab");

        if (Tg_grab.isplank) isOnPlank = true;

        if (!Tg_grab.directMove)
        {

            joints.connectedBody = Tg_grab.rb;
            joints.connectedAnchor = Tg_grab.FindClosestPoint(transform.position);
            joints.distance = Tg_grab.GrabDistance;
            joints.enabled = true;

            line.enabled = true;

            isGrabbing = true;

            _obj.isInteractingWith = true;
        }
        else
        {
            relativJoints.connectedBody = Tg_grab.rb;
            relativJoints.enabled = true;
            isGrabbing = true;
            _obj.isInteractingWith = true;

        }

    }

    public void ReleaseObj(Interactable _obj)
    {
        joints.enabled = false;
        relativJoints.enabled = false;

        isGrabbing = false;
        line.enabled = false;

        _obj.isInteractingWith = false;

    }

    void UpdateRope()
    {
        line.SetPosition(1, Tg_grab.transform.position);
        line.SetPosition(0, (Vector2)transform.position + offsetRope);
    }
}
