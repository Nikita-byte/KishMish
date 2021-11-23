using UnityEngine;


namespace UnityStandardAssets.Characters.FirstPerson
{
    public abstract class BaseInteractable : MonoBehaviour
    {
        protected Rigidbody _rigidBody;

        private void Awake()
        {
            if (_rigidBody == null)
            {
                _rigidBody = GetComponent<Rigidbody>();
            }
        }

        public abstract void Interact(GameObject position);
    }
}
