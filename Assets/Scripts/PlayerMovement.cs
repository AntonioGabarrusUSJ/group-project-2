using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")]
    public float walkSpeed;
    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplyier;
    bool readyToJump;

    [Header("KeyBinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode runningKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask groundLayer;
    bool grounded;

    [Header("Sprint Variables")]
    public float runningSpeed;
    bool isRunning = false;

    public Transform orientation;


    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    [SerializeField] Rigidbody rb;


    [Header("Camera Parameters")]
    public float runningCameraFOV = 90;
    float originalCameraFOV;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        originalCameraFOV = Camera.main.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);

        StoreInput();
        SpeedControl();

        //handle ground
        rb.linearDamping = (grounded) ? groundDrag : 0;

        ManageCameraFOV();

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void StoreInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        isRunning = Input.GetKey(runningKey) && (Mathf.Abs(horizontalInput) > 0 || Mathf.Abs(verticalInput) > 0);


    }

    void MovePlayer()
    {
        //calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        
        Vector3 force = (grounded) ? //on ground
            (isRunning)? 
                moveDirection.normalized * runningSpeed * 10f : 
                moveDirection.normalized * walkSpeed * 10f : 
            moveDirection.normalized * walkSpeed * 10f * airMultiplyier; // on air
        rb.AddForce(force, ForceMode.Force);
    }

    void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x,0,rb.linearVelocity.z);
        if (isRunning && flatVel.magnitude > runningSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * runningSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
        else if(!isRunning && flatVel.magnitude > walkSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * walkSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x,rb.linearVelocity.y,limitedVel.z);
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector3 (rb.linearVelocity.x,0f,rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void ResetJump()
    {
        readyToJump = true;
    }

    void ManageCameraFOV()
    {
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, (isRunning)? runningCameraFOV : originalCameraFOV, Time.deltaTime * 5);
    }
}
