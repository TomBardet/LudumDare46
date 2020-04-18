using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour
{
    SpringJoint2D joints;

    public bool isGrabbing;

    void Awake()
    {
        joints = GetComponent<SpringJoint2D>();
        joints.enabled = false;
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
            var grab = _obj.GetComponent<GrabableItem>();

            if (grab == null) Debug.Log("Error récupération target de grab");

            joints.connectedBody = grab.rb;
            joints.distance = grab.GrabDistance;
            joints.enabled = true;

            isGrabbing = true;
        }else
        {
            joints.enabled = false;
            
            isGrabbing = false;
            

        }

    }
}
