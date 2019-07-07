using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    public LayerMask whatIsMovingPlatform;
    [Range(0, 1)] public float smoothOfStartEndMoving;
    public GameObject dustAfterJump;

    public Image[] movementButtons;
    public Sprite activeButtonSprite;
    public Sprite inactiveButtonSprite;

    [Header("Debug")]
    [SerializeField] private MovementState _movementState;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private float _movementDirection;

    private Rigidbody2D _rb2d;
    private Animator _anim;
    private bool _lastIsGrounded;
    private Vector2 _spawnPoint;

    private void Start()
    {
        _rb2d = gameObject.GetComponent<Rigidbody2D>();
        _anim = gameObject.GetComponent<Animator>();

        _spawnPoint = gameObject.transform.position;
    }

    private void Update()
    {
        // Check if there is ground below the player.
        _isGrounded = Physics2D.OverlapCapsule(groundChecker.position, checkSize, CapsuleDirection2D.Horizontal, 0, whatIsGround);
        if(!_isGrounded) _isGrounded = Physics2D.OverlapCapsule(groundChecker.position, checkSize, CapsuleDirection2D.Horizontal, 0, whatIsMovingPlatform);

        // Manage animation.
        _anim.SetBool("Ground", _isGrounded);
        _anim.SetFloat("Speed", Mathf.Abs(_movementDirection));

        // Spawn dust if necessary.
        if (_isGrounded && !_lastIsGrounded) SpawnDust();

#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.Space)) Jump();
        if (Input.GetKey(KeyCode.A)) Left();
        if (Input.GetKey(KeyCode.D)) Right();
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) Idle();
#endif

        // Calculate which way the player moves.
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

        // Interact with moving platforms if necessary.
        Collider2D col2d = Physics2D.OverlapCapsule(groundChecker.position, checkSize, CapsuleDirection2D.Horizontal, 0, whatIsMovingPlatform);
        if (col2d != null && _movementState == MovementState.Idle) transform.SetParent(col2d.transform);
        else transform.SetParent(null);

        // Move player.
        _rb2d.velocity = new Vector2(_movementDirection * moveSpeed, Mathf.Clamp(_rb2d.velocity.y, -jumpForce, jumpForce));

        // Remember _isGrounded variable.
        _lastIsGrounded = _isGrounded;
    }

    public void Jump()
    {
        if (_isGrounded)
        {
            _movementState = MovementState.Jump;
            _rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public void Left()
    {
        _movementDirection = -1;
    }

    public void Right()
    {
        _movementDirection = 1;
    }

    public void Idle()
    {
        _movementDirection = 0;
    }

    public void ChangeButtonSprite(int index)
    {
        if (movementButtons[index].sprite == activeButtonSprite) movementButtons[index].sprite = inactiveButtonSprite;
        else movementButtons[index].sprite = activeButtonSprite;
    }

    private void Flip(bool isFlip)
    {
        GetComponent<SpriteRenderer>().flipX = isFlip;
    }

    private void SpawnDust()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, float.MaxValue, whatIsGround.value | whatIsMovingPlatform.value);
        if (hit) Destroy(Instantiate(dustAfterJump, hit.point, dustAfterJump.transform.rotation), 2f);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Saw" || col.gameObject.tag == "Spike" || col.gameObject.tag == "Abyss")
        {
            gameObject.transform.position = _spawnPoint;
            ImageEffectsHandler.instance.StartCoroutine(ImageEffectsHandler.instance.ActivateGlitch());
        }
    }
}
