using UnityEngine;

public enum MovementState
{
    Left,
    Idle,
    Right,
    Jump
}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class CharacterController2D : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed;
    public float jumpForce;
    public Transform groundChecker;
    public Vector2 checkSize;
    public LayerMask whatIsGround;
    [Range(0, 1)] public float smoothOfStartEndMoving;

    [Header("Debug")]
    [SerializeField] private MovementState _movementState;
    [SerializeField] private bool _isGrounded = false;
    [SerializeField] private float _movementDirection;

    private Rigidbody2D _rb2d;
    private Animator _anim;

    private void Start()
    {
        _rb2d = gameObject.GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        _isGrounded = Physics2D.OverlapCapsule(groundChecker.position, checkSize, CapsuleDirection2D.Horizontal, 0, whatIsGround);
        _anim.SetBool("Ground", _isGrounded);
        _anim.SetFloat("Speed", Mathf.Abs(_movementDirection));

#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.Space)) Jump();
        if (Input.GetKey(KeyCode.A)) Left();
        if (Input.GetKey(KeyCode.D)) Right();
        if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) Idle();
#endif

        if (_movementDirection < 0)
        {
            Flip(true);
            if (_isGrounded) _movementState = MovementState.Left;
        }
        else if (_movementDirection > 0)
        {
            Flip(false);
            if (_isGrounded) _movementState = MovementState.Right;
        }
        else
        {
            if (_isGrounded) _movementState = MovementState.Idle;
        }

        _rb2d.velocity = new Vector2(_movementDirection * moveSpeed, Mathf.Clamp(_rb2d.velocity.y, -jumpForce, jumpForce));
    }

    public void Jump()
    {
        if(_isGrounded)
        {
            _movementState = MovementState.Jump;
            _rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public void Left()
    {
        _movementDirection = Mathf.MoveTowards(_movementDirection, -1, smoothOfStartEndMoving);
    }

    public void Right()
    {
        _movementDirection = Mathf.MoveTowards(_movementDirection, 1, smoothOfStartEndMoving);
    }

    public void Idle()
    {
        _movementDirection = Mathf.MoveTowards(_movementDirection, 0, smoothOfStartEndMoving);
    }

    private void Flip(bool isFlip)
    {
        GetComponent<SpriteRenderer>().flipX = isFlip;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "MovingPlatform")
        {
            transform.SetParent(col.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "MovingPlatform")
        {
            transform.SetParent(null);
        }
    }
}
