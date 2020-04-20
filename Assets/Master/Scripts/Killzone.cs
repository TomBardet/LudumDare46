using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    public bool isActive;

    public Animator anim;
    public Collider2D trigger;

    // Start is called before the first frame update
    void Start()
    {

        if(isActive)
        {
            setActive();
        }
        else
        {
            setInactive();
        }
    }

    [ContextMenu("SetActive")]

    public void setActive()
    {
        trigger.enabled = true;
        isActive = true;

        anim.SetBool("isInactive", false);
    }

    [ContextMenu("SetInactive")]

    public void setInactive()
    {
        trigger.enabled = false;
        isActive = false;

        anim.SetBool("isInactive", true);

    }
}
