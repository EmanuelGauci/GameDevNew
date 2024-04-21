using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerMovement : MonoBehaviour {
    [SerializeField]private RendererFeatureToggle rendererFeatureToggle;
    public float baseMoveSpeed = 5f;
    public float jumpForce = 7f;
    public bool isGrounded, isOnLadder;
    [System.NonSerialized] public float turnSpeed = 20f;
    public Rigidbody rb;
    [SerializeField] public float raycastDistance = 0.2f;
    public LayerMask groundLayer;
    public LayerMask ladderLayer;
    public float climbSpeed = 5f, horizontalClimbSpeed = 7f;
    public bool isParalyzed = false;
    public bool RMBToggle = false;
    [SerializeField] private UIHandler uihandler;
    public bool selectwithmouse = true, turnwithmouse = false;

    void Start() {
        RigidBody();
    }
    void Update() {
        if (isParalyzed == false) {
            MovementInput();
            TurnPlayer();
            Jump();
        }
        CheckGround();
        CheckLadder();
        Climbing();
    }
    void RigidBody() { //anything that has to do with rigidbody is to be put here
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevents the player from flipping over
    }
    void MovementInput() {//basic movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput);
        MovePlayer(moveDirection, baseMoveSpeed);//call moveplayer with the regular movement speed
        
    }
    void MovePlayer(Vector3 moveDirection, float moveSpeed) {
        Vector3 targetVelocity = transform.TransformDirection(moveDirection) * moveSpeed;
        rb.velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);
    }
    void TurnPlayer() {//mouse behaviour
        if (Input.GetMouseButtonDown(1)) {
            RMBToggle = !RMBToggle; // Toggle the state
        }
        if (RMBToggle || uihandler.isPaused) {
            if (!uihandler.isPaused) {
                rendererFeatureToggle.activateFeature = true;
                float mouseX = Input.GetAxis("Mouse X");
                Vector3 rotation = new Vector3(0f, mouseX * turnSpeed, 0f);
                Quaternion deltaRotation = Quaternion.Euler(rotation * Time.fixedDeltaTime);
                rb.MoveRotation(rb.rotation * deltaRotation);
            }
        } else {
            TurnWithMouse();
            float mouseX = Input.GetAxis("Mouse X");
            Vector3 rotation = new Vector3(0f, mouseX * turnSpeed, 0f);
            Quaternion deltaRotation = Quaternion.Euler(rotation * Time.fixedDeltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }
    public void TurnWithMouse() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        turnwithmouse = true;
        if (!isParalyzed && rendererFeatureToggle.activateFeature == true) {
            rendererFeatureToggle.activateFeature = false;
        }
    }
    public void SelectWithMouse() {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        selectwithmouse = true;
        
    }
    //Jump and climb
    void CheckGround() {//raycast downwards to check for the ground
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance, groundLayer)) {
            isGrounded = true;
        } else {
            isGrounded = false;
        }
    }
    void Jump() {
        if (Input.GetButtonDown("Jump") && isGrounded) {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    void CheckLadder() {//raycast forwards to check for ladder
        RaycastHit hit;
        Vector3 raycastDirection = transform.forward;
        if (Physics.Raycast(transform.position, raycastDirection, out hit, raycastDistance, ladderLayer)) {
            isOnLadder = true;
        } else {
            isOnLadder = false;
        }
    }
    void Climbing() {
        if (isOnLadder && !isGrounded) {
            float verticalInput = Input.GetAxis("Vertical");
            float horizontalInput = Input.GetAxis("Horizontal");
            Vector3 climbVelocity = new Vector3(horizontalInput * -horizontalClimbSpeed, verticalInput * climbSpeed, rb.velocity.z);//preserve both horizontal and vertical movement with adjusted speeds

            // Only update the vertical and horizontal components of the velocity
            rb.velocity = new Vector3(climbVelocity.x, climbVelocity.y, 0f);

        }
    }
    
}