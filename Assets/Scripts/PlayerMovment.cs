using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovment : MonoBehaviour
{
    [SerializeField]
    float _walkSpeed, _jumpSpeed, _rayCastLength;

    [SerializeField]
    bool _isJumping, _isFacingRight, _isOnGround;

    

    [SerializeField]
    Rigidbody2D rb;


    [Header("Collision")]
    [SerializeField]
    Vector3 _rayCastOffset;






    InputActions _inputActions;

    void Awake()
    {
        _inputActions = new InputActions();
        _inputActions.Player.Jump.performed += ctx => StartJump();
        _inputActions.Player.Jump.canceled += ctx => EndJump();

    }

    void Start()
    {
        
    }

    void OnMove()
    {
        FlipAnimation();
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


    void FixedUpdate()
    {
        if(!_isOnGround)
        {

        }


        Walk();
        if (_isJumping)
        {
            Jump();
        }

    }


    void Walk()
    {
        Vector2 direction = _inputActions.Player.Move.ReadValue<Vector2>();
        float walking = direction.x * _walkSpeed;
        rb.AddForce(new Vector2(direction.x * _walkSpeed, rb.position.y));
    }


    void Jump() => rb.AddForce(Vector2.up * _jumpSpeed * Time.deltaTime, ForceMode2D.Impulse);
    void OnEnable() => _inputActions.Enable();
    void OnDisable() => _inputActions.Disable();


    void FlipAnimation()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + _rayCastOffset, transform.position + _rayCastOffset + Vector3.down * _rayCastLength);
        Gizmos.DrawLine(transform.position - _rayCastOffset, transform.position - _rayCastOffset + Vector3.down * _rayCastLength);

    }
}
