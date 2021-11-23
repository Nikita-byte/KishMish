using System;
using UnityEngine;


namespace UnityStandardAssets.Characters.FirstPerson
{
    class Thrown : BaseInteractable
    {
        [SerializeField] private float _forceVelocity;

        public override void Interact(GameObject position)
        {
            _rigidBody.AddForce(position.transform.forward * _forceVelocity, ForceMode.Force);

            Debug.Log("Kick");
        }
    }
}
