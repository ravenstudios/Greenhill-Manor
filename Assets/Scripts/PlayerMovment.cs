using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovment : MonoBehaviour
{
    [SerializeField]
    float _walkSpeed = 5f;

    [SerializeField]
    
    bool _isJumping = false;
    [SerializeField]
    float _jumpSpeed = 5f;


    [SerializeField]
    Rigidbody2D rb;

    InputActions _inputActions;

    private void Awake()
    {
        _inputActions = new InputActions();
        _inputActions.Player.Jump.performed += ctx => StartJump();
        _inputActions.Player.Jump.canceled += ctx => EndJump();
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    

    void StartJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        _isJumping = true;

    }

    void EndJump()
    {
        _isJumping = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Jump() => rb.AddForce(Vector2.up * _jumpSpeed * Time.deltaTime, ForceMode2D.Impulse);

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 direction = _inputActions.Player.Move.ReadValue<Vector2>();
        float walking = direction.x * _walkSpeed;
        
        if (_isJumping)
        {
            Jump();
        }

        
        rb.AddForce(new Vector2(direction.x * _walkSpeed, rb.position.y));
        
    }
}
