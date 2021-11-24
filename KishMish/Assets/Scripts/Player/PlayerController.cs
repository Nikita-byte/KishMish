using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerUI _playerUI;
    [SerializeField] private BasePlayer _player;

    private Vector2 _axisInput;

    private void Update()
    {
        CheckInputs();

        _player.RotateView();

        if (_player.CheckInteractableObjects())
        {
            _playerUI.CanInteract(true);
        }
        else
        {
            _playerUI.CanInteract(false);
        }
    }

    private void FixedUpdate()
    {
        _player.Move(_axisInput);
    }

    private void CheckInputs()
    {
        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float vertical = CrossPlatformInputManager.GetAxis("Vertical");

        _axisInput = new Vector2(horizontal, vertical);

        // normalize input if it exceeds 1 in combined length:
        if (_axisInput.sqrMagnitude > 1)
        {
            _axisInput.Normalize();
        }

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            _player.Jump();
        }
    }
}
