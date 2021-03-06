using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovment : MonoBehaviour
{
    [SerializeField]
    float _walkSpeed, _maxWalkSpeed, _jumpSpeed, _maxJumpHeight, _rayCastLength, _gravityMultiplier;

    [SerializeField]
    bool _isJumping, _isFacingRight, _isOnGround;

    

    [SerializeField]
    Rigidbody2D rb;

    [SerializeField]
    Vector2 MovementVelocity;

    [SerializeField]
    Animator animator;

    [SerializeField]
    SpriteRenderer sr;

    [Header("Collision")]
    [SerializeField]
    Vector3 _rayCastOffset;

    [SerializeField]
    LayerMask _groundLayer;




   InputActions _inputActions;

    void Awake()
    {
        _inputActions = new InputActions();
        _inputActions.Player.Jump.performed += ctx => StartJump();
        _inputActions.Player.Jump.canceled += ctx => EndJump();

    }


    void StartJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        if (_isOnGround)
        {
            _isJumping = true;
            Jump();
        }
    }


    void EndJump() => _isJumping = false;
    

    //Resets the posistion of the player for testing
    void OnReset() {
        rb.velocity = new Vector2(0, 0);
        transform.position = new Vector3(0, 0, 0);
    }


    void FixedUpdate()
    {
        //for UI
        MovementVelocity = rb.velocity;

        //Shoots a line from the player to the groundlayer and checks for collision
        _isOnGround =
            Physics2D.Raycast(transform.position + _rayCastOffset, Vector2.down, _rayCastLength, _groundLayer)
            ||
            Physics2D.Raycast(transform.position - _rayCastOffset, Vector2.down, _rayCastLength, _groundLayer);

        Walk();

        //Fall multipler used to make fall after apex faster
        if (rb.velocity.y < 0) rb.velocity += Vector2.up * Physics2D.gravity.y * _gravityMultiplier * Time.deltaTime;
        

        //if (_isOnGround) rb.velocity = new Vector2(rb.velocity.x, 0);

        if (_isJumping) Jump();
        

        animator.SetBool("IsOnGround", _isOnGround);

        if (rb.velocity.y > _maxJumpHeight)
        {
            _isJumping = false;
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }


    void Walk()
    {
        //Read from input device
        Vector2 direction = _inputActions.Player.Move.ReadValue<Vector2>();

        _isFacingRight = (direction.x > 0) ? true: false;
        _isFacingRight = (direction.x < 0) ? false : true;

        //Move the Player
        float walking = direction.x * _walkSpeed * Time.deltaTime;
        rb.velocity += new Vector2(walking, 0);

        //Flip animation if change of direction
        sr.flipX = !_isFacingRight;

        //Clamps X velocity
        rb.velocity = (rb.velocity.x > _maxWalkSpeed) ?
            new Vector2(_maxWalkSpeed, rb.velocity.y):
            new Vector2(rb.velocity.x, rb.velocity.y);

        rb.velocity = (rb.velocity.x < -_maxWalkSpeed) ?
            new Vector2(-_maxWalkSpeed, rb.velocity.y) :
            new Vector2(rb.velocity.x, rb.velocity.y);

        //Changes animation
        animator.SetFloat("Walking_speed", Mathf.Abs(rb.velocity.x));
    }


    void Jump() => rb.velocity += Vector2.up * _jumpSpeed * Time.deltaTime;
    void OnEnable() => _inputActions.Enable();
    void OnDisable() => _inputActions.Disable();


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + _rayCastOffset, transform.position + _rayCastOffset + Vector3.down * _rayCastLength);
        Gizmos.DrawLine(transform.position - _rayCastOffset, transform.position - _rayCastOffset + Vector3.down * _rayCastLength);
    }
}
