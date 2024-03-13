using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    [SerializeField] private CharacterController CC;
    [SerializeField] private Transform PlayerBody;
    [SerializeField] private Transform GroundCheck;

     float X;
     float Z;

    [Header("Movement")]
    public bool IsWalking;
    public bool IsRunning;
    [SerializeField] private float WalkSpeed;
    [SerializeField] private float RunSpeed;
    [SerializeField] private float JumpForce;


    [Header("Gravity")]

    Vector3 GravityVector;
    [SerializeField] private float GravityAc = -9.81f;
    [SerializeField] private bool IsGrounded;
    [SerializeField] private LayerMask GroundLayer;


    private void FixedUpdate()
    {
        Movement();
        Gravity();

    }

    private void Update()
    {
        Jump();
        CheckMovement();
    }
    private void Movement()
    {
        X=Input.GetAxis("Horizontal");
        Z=Input.GetAxis("Vertical");

        Vector3 Move = PlayerBody.right* X+ PlayerBody.forward* Z;
        CC.Move(Move*TotalSpeed()*Time.deltaTime);
    }
    private void CheckMovement()
    {
        if (X!=0f||Z!=0f)
        {
            if (TotalSpeed()==RunSpeed)
            {
                IsRunning = true;
                IsWalking = false;
            }
            if (TotalSpeed() == WalkSpeed)
            {
                IsRunning = false;
                IsWalking = true;
            }
        }
        else
        {
            IsRunning = false;
            IsWalking = false;
        }
    }
    private void Jump()
    {
        if (IsGrounded&&Input.GetButtonDown("Jump"))
        {
            GravityVector.y=Mathf.Sqrt(JumpForce*-2f*GravityAc/1000f);
        }
    }
    private void Gravity()
    {
        IsGrounded = Physics.CheckSphere(GroundCheck.position, 0.4f, GroundLayer);

        if (!IsGrounded)
            GravityVector.y += GravityAc * Mathf.Pow(Time.deltaTime, 2);
        else if (GravityVector.y<0f)
            GravityVector.y = -0.15f;

        CC.Move(GravityVector);
    }

  public float TotalSpeed()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            return RunSpeed;
        }
        else
        {
            return WalkSpeed;
        }
    }
}
