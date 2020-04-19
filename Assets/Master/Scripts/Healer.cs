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

    void Awake()
    {
        joints = GetComponent<SpringJoint2D>();
        line = GetComponent<LineRenderer>();
        joints.enabled = false;
        line.enabled = false;

    }

    private void Update()
    {
        if (isGrabbing && Tg_grab != null)
        {
            UpdateRope();
        }
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

    void AttachToObject(Interactable _obj)
    {
        if(!isGrabbing)
        {
            Tg_grab = _obj.GetComponent<GrabableItem>();

            if (Tg_grab == null) Debug.Log("Error récupération target de grab");

            joints.connectedBody = Tg_grab.rb;
            joints.connectedAnchor = Tg_grab.FindClosestPoint(transform.position);
            joints.distance = Tg_grab.GrabDistance;
            joints.enabled = true;

            line.enabled = true;

            isGrabbing = true;
        }else
        {
            joints.enabled = false;
            
            isGrabbing = false;
           line.enabled = false;


        }

    }

    void UpdateRope()
    {
        line.SetPosition(1, Tg_grab.transform.position);
        line.SetPosition(0, (Vector2)transform.position + offsetRope);
    }
}
