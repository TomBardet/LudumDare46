using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour
{
    SpringJoint2D joints;
    LineRenderer line;

    public bool isGrabbing;
    public GrabableItem Tg_grab;
    public Vector2 offsetRope;

    public float manaMax;
    public float manaRegenPerSec;
    public float manaRegenDelay;
    public float healCost;
    float currentMana;
    bool isRegenerating;

    void Awake()
    {
        joints = GetComponent<SpringJoint2D>();
        line = GetComponent<LineRenderer>();
        joints.enabled = false;
        line.enabled = false;
        currentMana = manaMax;
    }

    private void Update()
    {
        if (isGrabbing && Tg_grab != null)
        {
            UpdateRope();
        }
        if (isRegenerating)
            currentMana += Time.deltaTime * manaRegenPerSec;
    }

    public void Interact(string tag, Interactable _obj)
    {
        switch(tag)
        {
            case "Moveable":
                AttachToObject(_obj);
                break;
            default:
                break;
        }
    }

    public void StopInteract(string tag, Interactable _obj)
    public void Heal()
    {
        if (currentMana >= healCost)
        {
            currentMana -= healCost;
            Warrior.instance.ReceiveHeal(1);
            StartCoroutine(RegenDelay());
        }
    }

    IEnumerator RegenDelay()
    {
        float delay = manaRegenDelay;
        isRegenerating = false;
        while(delay > 0)
        {
            delay -= 1 * Time.deltaTime;
            yield return null;
        }
        isRegenerating = true;
    }

    void AttachToObject(Interactable _obj)
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

        joints.connectedBody = Tg_grab.rb;
        joints.connectedAnchor = Tg_grab.FindClosestPoint(transform.position);
        joints.distance = Tg_grab.GrabDistance;
        joints.enabled = true;

        line.enabled = true;

        isGrabbing = true;

        _obj.isInteractingWith = true;


    }

    void ReleaseObj(Interactable _obj)
    {
        joints.enabled = false;

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
