using UnityEngine;
using UnityStandardAssets.Utility;


public class Player : BasePlayer
{
    [SerializeField] protected float _stepInterval;          // the sound played when character touches back on ground.
    [SerializeField] [Range(0f, 1f)] protected float _runstepLenghten;
    [SerializeField] protected float _stickToGroundForce;
    [SerializeField] protected float _gravityMultiplier;
    [SerializeField] protected bool _useFovKick;
    [SerializeField] protected FOVKick _fovKick = new FOVKick();
    [SerializeField] protected bool _useHeadBob;
    [SerializeField] protected CurveControlledBob _headBob = new CurveControlledBob();
    [SerializeField] protected LerpControlledBob _jumpBob = new LerpControlledBob();

    protected bool _jump;
    protected float _Yrotation;
    protected Vector2 _input;
    protected Vector3 _moveDir = Vector3.zero;
    protected bool _previouslyGrounded;
    protected Vector3 _originalCameraPosition;
    protected float _stepCycle;
    protected float _nextStep;
    protected bool _jumping;
    protected PlayerSoundComponent _playerSoundComponent;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _playerSoundComponent = GetComponent<PlayerSoundComponent>();
        _camera = Camera.main;
        _originalCameraPosition = _camera.transform.localPosition;
        _fovKick.Setup(_camera);
        _headBob.Setup(_camera, _stepInterval);
        _stepCycle = 0f;
        _nextStep = _stepCycle / 2f;
        _jumping = false;
        _mouseLook.Init(transform, _camera.transform);
    }

    public override void Jump()
    {
        if (!_jump)
        {
            _jump = true;
        }

        if (!_previouslyGrounded && _characterController.isGrounded)
        {
            StartCoroutine(_jumpBob.DoBobCycle());
            _moveDir.y = 0f;
            _jumping = false;

            if (_playerSoundComponent != null)
            {
                _playerSoundComponent.PlayLandingSound();
            }
        }

        if (!_characterController.isGrounded && !_jumping && _previouslyGrounded)
        {
            _moveDir.y = 0f;
        }

        _previouslyGrounded = _characterController.isGrounded;
    }

    public override void Move(Vector2 axisInput)
    {
        float speed;
        bool waswalking = _isWalking;
        _input = axisInput;

        // set the desired speed to be walking or running
        speed = _isWalking ? _walkSpeed : _runSpeed;

        // handle speed change to give an fov kick
        // only if the player is going to a run, is running and the fovkick is to be used
        if (_isWalking != waswalking && _useFovKick && _characterController.velocity.sqrMagnitude > 0)
        {
            StopAllCoroutines();
            StartCoroutine(!_isWalking ? _fovKick.FOVKickUp() : _fovKick.FOVKickDown());
        }
        // always move along the camera forward as it is the direction that it being aimed at
        Vector3 desiredMove = transform.forward * _input.y + transform.right * _input.x;

        // get a normal for the surface that is being touched to move along it
        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, _characterController.radius, Vector3.down, out hitInfo,
                           _characterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        _moveDir.x = desiredMove.x * speed;
        _moveDir.z = desiredMove.z * speed;


        if (_characterController.isGrounded)
        {
            _moveDir.y = -_stickToGroundForce;

            if (_jump)
            {
                _moveDir.y = _jumpSpeed;
                _jump = false;
                _jumping = true;

                if (_playerSoundComponent != null)
                {
                    _playerSoundComponent.PlayJumpSound();
                }
            }
        }
        else
        {
            _moveDir += Physics.gravity * _gravityMultiplier * Time.fixedDeltaTime;
        }
        _collisionFlags = _characterController.Move(_moveDir * Time.fixedDeltaTime);

        ProgressStepCycle(speed);
        UpdateCameraPosition(speed);

        _mouseLook.UpdateCursorLock();
    }

    private void ProgressStepCycle(float speed)
    {
        if (_characterController.velocity.sqrMagnitude > 0 && (_input.x != 0 || _input.y != 0))
        {
            _stepCycle += (_characterController.velocity.magnitude + (speed * (_isWalking ? 1f : _runstepLenghten))) *
                         Time.fixedDeltaTime;
        }

        if (!(_stepCycle > _nextStep))
        {
            return;
        }

        _nextStep = _stepCycle + _stepInterval;

        if (_playerSoundComponent != null && _characterController.isGrounded)
        {
            _playerSoundComponent.PlayFootStepAudio();
        }
    }

    private void UpdateCameraPosition(float speed)
    {
        Vector3 newCameraPosition;
        if (!_useHeadBob)
        {
            return;
        }
        if (_characterController.velocity.magnitude > 0 && _characterController.isGrounded)
        {
            _camera.transform.localPosition =
                _headBob.DoHeadBob(_characterController.velocity.magnitude +
                                  (speed * (_isWalking ? 1f : _runstepLenghten)));
            newCameraPosition = _camera.transform.localPosition;
            newCameraPosition.y = _camera.transform.localPosition.y - _jumpBob.Offset();
        }
        else
        {
            newCameraPosition = _camera.transform.localPosition;
            newCameraPosition.y = _originalCameraPosition.y - _jumpBob.Offset();
        }
        _camera.transform.localPosition = newCameraPosition;
    }
}
