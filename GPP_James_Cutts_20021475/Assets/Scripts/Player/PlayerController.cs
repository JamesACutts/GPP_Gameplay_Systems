using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController playerCon;
    public DoorButton button;
    public Lever lever;


    // Input system
    PlayerInput controls;

    public GameObject cam1;
    public GameObject cam2;
    public GameObject cam3;

    // Character movement
    private CharacterController controller;
    private Animator anim;
    private Animator enemyAnim;
    public float moveSpeed;
    [SerializeField]
    private float walkSpeed = 5f;
    [SerializeField]
    private float runSpeed = 8f;
    [SerializeField]
    private float jumpHeight = 1.5f;
    [SerializeField]
    private float gravity = -9.81f;
    [SerializeField]
    private float groundCheckDistance = 0.4f;

    // Turning
    [SerializeField]
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    // Ground check
    private bool isGrounded;
    private bool isJumping;

    [SerializeField]
    private LayerMask groundMask;

    // variables to store player input
    Vector2 move;
    Vector3 velocity;
    bool runPressed;
    bool jumpPressed;

    public bool canPress = false;
    public bool isUsed = false;
    public bool isLeverUsed = false;
    public bool isButton = false;
    public bool isLever = false;

    // Attack
    public bool attacking;
    public bool canAttack = false;
    public bool isAttacking;
    [SerializeField]
    private float sinceLastAttack = 0;
    private PlayerStats stats;

    // Getters and Setters
    public float GetSpeed()
    {
        return moveSpeed;
    }
    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }

    private void Start()
    {
        // Initialization
        anim = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        cam1.SetActive(true);
        cam2.SetActive(false);
        cam3.SetActive(false);
        stats = GetComponent<PlayerStats>();

    }
    void Awake()
    {
        // Input System Event Handlers
        controls = new PlayerInput();
        controls.Gameplay.Movement.performed += ctx => SendMessage(ctx.ReadValue<Vector2>());
        controls.Gameplay.Movement.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Movement.canceled += ctx => move = Vector2.zero;
        controls.Gameplay.Run.performed += ctx => runPressed = ctx.ReadValueAsButton();
        controls.Gameplay.Jump.performed += ctx => jumpPressed = ctx.ReadValueAsButton();
/*      controls.Gameplay.Attack.performed += ctx => Attack();*/

        controls.Gameplay.Interact.performed += ctx => Interact();
    }

    public void OnAttack()
    {
        // Attack
        attacking = true;
        anim.SetTrigger("Attack");
    }

    public void OnEnable()
    {
        controls.Gameplay.Enable();
    }
    public void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    void SendMessage(Vector2 coordinates)
    {
        /*Debug.Log("Thumb-stick coordinates = " + coordinates);*/
    }
    void FixedUpdate()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Grounded and jumping states
        if (isGrounded)
        {
            anim.SetBool("isGrounded", true);
            isGrounded = true;
            anim.SetBool("isJumping", false);
            isJumping = false;
            anim.SetBool("isFalling", false);
        }
        else
        {
            anim.SetBool("isGrounded", false); ;
            isGrounded = false;
        }

        if ((isJumping && velocity.y < 0) || velocity.y < -2)
        {
            anim.SetBool("isFalling", true);
        }

        //Movement
        Vector3 movement = new Vector3(move.x, 0.0f, move.y).normalized;
        movement = cam1.transform.forward * movement.z + cam1.transform.right * movement.x;
        movement.y = 0f;
        movement = movement.normalized;

        if (movement.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
/*          movement = cam1.transform.forward * movement.z + cam1.transform.right * movement.x;*/
            controller.Move(movement * moveSpeed * Time.deltaTime);
        }

        // Set the player's movement state for the animation
        if (isGrounded)
        {
            if (move != Vector2.zero && !runPressed && moveSpeed != 16)
            {
                // Walk
                Walk();
            }
            else if (move != Vector2.zero && runPressed && moveSpeed != 16)
            {
                // Run
                Run();
            }
            else if (move != Vector2.zero && moveSpeed == 16)
            {
                anim.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
            }
            else if (move == Vector2.zero)
            {
                // Idle
                Idle();
            }
            move *= moveSpeed;
            if (jumpPressed)
            {
                // Jump
                Jump();
            }
            if(attacking && !runPressed && canAttack)
            {
                if (!isAttacking)
                {
                    isAttacking = true;
                }
            }

        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        attacking = false;
    }
    private void Idle()
    {
        anim.SetFloat("Speed", 0f, 0.1f, Time.deltaTime);
    }
    private void Walk()
    {
        moveSpeed = walkSpeed;
        anim.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
    }
    private void Run()
    {
        moveSpeed = runSpeed;
        anim.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
    }
    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        anim.SetBool("isJumping", true);
        isJumping = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            canAttack = true;
            if (isAttacking)
            {
                if (Time.time >= sinceLastAttack + stats.attackSpeed)
                {
                    sinceLastAttack = Time.time;
                    CharacterStats enemyStats = other.GetComponent<CharacterStats>();
                    enemyAnim = other.GetComponent<Animator>();
                    enemyAnim.SetTrigger("Damage");
                    isAttacking = false;
                    Attack(enemyStats);
                }
            }
        }
    }

    private void Attack(CharacterStats statsToDamage)
    {
        stats.DealDamage(statsToDamage);
    }

    private void Interact()
    {
        if (canPress && !isUsed && isButton)
        {
            anim.SetTrigger("Attack");
            button.DoorOpen();
            isUsed = true;
        }
        else if (canPress && !isLeverUsed && isLever)
        {
            anim.SetTrigger("Attack");
            lever.DoorOpen();
            isLeverUsed = true;
        }
    }
}
