using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.Utility;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class BasePlayer : MonoBehaviour
{
    [SerializeField] protected bool _isWalking;
    [SerializeField] protected float _walkSpeed;
    [SerializeField] protected float _runSpeed;
    [SerializeField] protected float _jumpSpeed;
    [SerializeField] protected MouseLook _mouseLook;
    [SerializeField] protected LayerMask _interactableObjectsMask;
    [SerializeField] protected float _distanceForInteract = 2.0f;
    [SerializeField] protected GameObject _handPosition;

    protected Camera _camera;
    protected CharacterController _characterController;
    protected CollisionFlags _collisionFlags;

    public virtual void Jump() { }
    public virtual void Move(Vector2 axisInput) { }

    public virtual void RotateView()
    {
        _mouseLook.LookRotation(transform, _camera.transform);
    }

    public virtual bool CheckInteractableObjects() 
    {
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, _distanceForInteract, _interactableObjectsMask))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                BaseInteractable _activeObject = hit.transform.gameObject.GetComponent<BaseInteractable>();

                if (_activeObject != null)
                {
                    _activeObject.Interact(_handPosition);
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        //dont move the rigidbody if the character is on top of it
        if (_collisionFlags == CollisionFlags.Below)
        {
            return;
        }

        if (body == null || body.isKinematic)
        {
            return;
        }
        body.AddForceAtPosition(_characterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
    }
}
