using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPerception : MonoBehaviour
{
    Interactable _interactable;
    private void Update()
    {
        Interacting();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _interactable = collision.GetComponent<Interactable>();
        if (_interactable != null)
        {
            _interactable.isInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
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
            }
        }
    }
}


