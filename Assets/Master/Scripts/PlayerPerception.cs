using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPerception : MonoBehaviour
{
    Interactable _interactable;
    Healer healer;

    private void Awake()
    {
        healer = GetComponent<Healer>();
    }
    private void Update()
    {
        Interacting();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (healer.isGrabbing) return;

        _interactable = collision.GetComponent<Interactable>();
        if (_interactable != null)
        {
            _interactable.isInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (healer.isGrabbing) return;

        _interactable = collision.GetComponent<Interactable>();

        if (_interactable != null)
        {
            _interactable.isInRange = false;
        }
    }

    bool InteractInput()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    void Interacting()
    {
        if(_interactable != null)
        {
            if (_interactable.isInRange && InteractInput())
            {
                _interactable.Interact();
                healer.Interact(_interactable.tag, _interactable);
            }
        }
    }
}


