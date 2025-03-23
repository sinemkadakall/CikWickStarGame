using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _orientationTransform;

    [Header("Movement Settings ")]
    [SerializeField] private KeyCode _movementKey;
    [SerializeField] private float _movementSpeed;


    [Header("Jump Settings")]
    [SerializeField] private KeyCode _jumpKey;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpCoolDown;
    [SerializeField] private bool _canJump;


    [Header("Ground Check Settings")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundDrag;


    [Header("Slider Settings")]
    [SerializeField] private KeyCode _slideKey;
    [SerializeField] private float _slideMultiplier;
    [SerializeField] private float _slideDrag;
    private bool _isSliding;




    private Rigidbody _playerRigidbody;

    private float _horizontalInput,_verticalInput;
    private Vector3 _movementDirection;

   
   


    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerRigidbody.freezeRotation = true;
    }

    private void Update()
    {
        SetInputs();
        SetPlayerDrag();
        LimitPlayerSpeed();
    }

    private void FixedUpdate()
    {
        SetPlayerMovement();
    }

    private void SetInputs()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(_slideKey))
        {
            _isSliding = true;
        }
        else if(Input.GetKeyDown(_movementKey))
        {
            _isSliding=false;
        }
        else if (Input.GetKey(_jumpKey) && _canJump && IsGrounded())
        {
            //Zýplama olacak
            _canJump = false;
            SetPlayerJumping();
            Invoke(nameof(ResetJumping),_jumpCoolDown);
        }
    }

    private void SetPlayerMovement()
    {
        _movementDirection = _orientationTransform.forward * _verticalInput
            + _orientationTransform.right * _horizontalInput;
        if (_isSliding)
        {
            _playerRigidbody.AddForce(_movementDirection.normalized * _movementSpeed * _slideMultiplier, ForceMode.Force);

        }
        else
        {
            _playerRigidbody.AddForce(_movementDirection.normalized * _movementSpeed, ForceMode.Force);

        }
    }

    private void SetPlayerDrag()
    {
        if(_isSliding)
        {
            _playerRigidbody.drag = _slideDrag;

        }
        else
        {
            _playerRigidbody.drag = _groundDrag;
        }
    }

    private void LimitPlayerSpeed()
    {
        Vector3 flatVelocity = new Vector3(_playerRigidbody.velocity.x,0f,_playerRigidbody.velocity.z);

        if(flatVelocity.magnitude > _movementSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * _movementSpeed;
            _playerRigidbody.velocity = new Vector3(limitedVelocity.x, _playerRigidbody.velocity.y, limitedVelocity.z);
        }
    }
    private void SetPlayerJumping()
    {
        // _playerRigidbody.linearVelocity = new Vector3(_playerRigidbody.maxLinearVelocity.x,0f,_playerRigidbody.maxLinearVelocity.z);
      
        Vector3 velocity = _playerRigidbody.velocity;
        velocity.y = 0f;
        _playerRigidbody.velocity = velocity;
        _playerRigidbody.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }


    private void ResetJumping()
    {
        _canJump = true;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f,_groundLayer);
    }

}
