using System;
using UnityEngine;


namespace UnityStandardAssets.Characters.FirstPerson
{
    class Handpicked : BaseInteractable
    {
        public override void Interact(GameObject position)
        {
            gameObject.SetActive(false);
            Debug.Log("Pick");
        }
    }
}
