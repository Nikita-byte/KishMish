using System;
using UnityEngine;


class Handpicked : BaseInteractable
{
    public override void Interact(GameObject position)
    {
        gameObject.SetActive(false);
        Debug.Log("Pick");
    }
}
