using System;
using UnityEngine;


class Portable : BaseInteractable
{
    private bool _isPicked = false;
    private Transform _position;

    public override void Interact(GameObject position)
    {
        if (!_isPicked)
        {
            _position = position.transform;
            _rigidBody.useGravity = false;
            _isPicked = true;
            Debug.Log("Carry");
        }
        else
        {
            _rigidBody.useGravity = true;
            _isPicked = false;
            Debug.Log("Drop");
        }
    }

    private void FixedUpdate()
    {
        if (_isPicked)
        {
            gameObject.transform.position = _position.position;
        }
    }
}
