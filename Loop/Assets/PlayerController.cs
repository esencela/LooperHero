using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IRagdoll
{
    public float walkSpeed;
    public float runSpeed;
    public float lowTurnSpeed;
    public float highTurnSpeed;
    public float jumpForce;

    float turnSpeed;

    public Rigidbody rb;
    ConfigurableJoint mainJoint;
    float startXPositionSpring = 0.0f;
    float startYZPositionSpring = 0.0f;
    bool isActiveRagdoll = true;
    bool dead = false;

    Vector2 movementInput;
    public bool isSprinting = false;
    public bool isGrounded = false;
    float maxMoveAngle = 80f;

    bool canJump = true;
    public float jumpCooldown;
    float jumpTimer;

    bool addWeight = false;

    public Animator animator;

    RaycastHit[] groundHits = new RaycastHit[10];

    CopyMotion[] syncedObjects;

    Vector3 movementDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        syncedObjects = GetComponentsInChildren<CopyMotion>();
        mainJoint = GetComponent<ConfigurableJoint>();

        jumpTimer = jumpCooldown;
        turnSpeed = lowTurnSpeed;
        startXPositionSpring = mainJoint.angularXDrive.positionSpring;
        startYZPositionSpring = mainJoint.angularYZDrive.positionSpring;
    }

    void Update()
    {
        CheckGround();
        movementInput.x = Input.GetAxis("Horizontal");
        movementInput.y = Input.GetAxis("Vertical");

        isSprinting = Input.GetButton("Sprint");

        animator.SetBool("isSprinting", isSprinting);

        JumpCooldown();

        animator.SetBool("handUp", Input.GetButton("Fire1") || Input.GetButton("Fire2"));

        if (isActiveRagdoll)
        {
            if (isGrounded)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    Jump();
                }
            }

            //Sync to animated joints
            for (int i = 0; i < syncedObjects.Length; i++)
            {
                syncedObjects[i].SyncJointAnimation();
            }
        }

        if (addWeight)
        {
            AddWeight();
        }

        movementDirection = Quaternion.Euler(0.0f, Camera.main.transform.eulerAngles.y, 0.0f) * new Vector3(movementInput.x, 0.0f, movementInput.y);
    }

    void FixedUpdate()
    {
        if (!isActiveRagdoll)
            return;

        if (!isGrounded)
            return;

        Turn();
        Move();
    }

    void CheckGround()
    {
        int numberOfHits = Physics.SphereCastNonAlloc(rb.position, 0.1f, transform.up * -1, groundHits, 0.8f);
        
        for (int i = 0; i < numberOfHits; i++)
        {
            //ignore self hits
            if (groundHits[i].transform.root == transform)
                continue;

            isGrounded = true;

            return;
        }

        isGrounded = false;
    }

    void Move()
    {
        float inputMagnitude = movementInput.magnitude;

        animator.SetBool("isMoving", false);

        if (Vector3.Angle(movementDirection, rb.transform.forward) > maxMoveAngle)
        {
            turnSpeed = highTurnSpeed;
            return;
        }

        turnSpeed = lowTurnSpeed;

        if (isSprinting)
        {
            rb.AddForce(transform.forward * inputMagnitude * runSpeed, ForceMode.Force);
        }
        else
        {
            rb.AddForce(transform.forward * inputMagnitude * walkSpeed, ForceMode.Force);
        }

        if (inputMagnitude > 0)
        {
            animator.SetBool("isMoving", true);
        }        
    }

    void Jump()
    {
        if (canJump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJump = false;
        }
    }

    void Turn()
    {
        float inputMagnitude = movementInput.magnitude;

        if (inputMagnitude > 0)
        {
            Vector3 targetDirection = Quaternion.Euler(0.0f, -Camera.main.transform.eulerAngles.y, 0.0f) * new Vector3(-movementInput.x, 0.0f, movementInput.y);
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

            mainJoint.targetRotation = Quaternion.RotateTowards(mainJoint.targetRotation, targetRotation, Time.deltaTime * turnSpeed);
        }        
    }

    void JumpCooldown()
    {
        if (canJump)
            return;

        if (jumpTimer > 0)
        {
            jumpTimer -= Time.deltaTime;
        }
        else
        {
            jumpTimer = jumpCooldown;
            canJump = true;
        }
    }

    public void Ragdoll(bool kill)
    {
        if (kill)
        {
            GameManager.instance.PlayerDeath();
            dead = true;
        }

        if (!isActiveRagdoll)
            return;

        mainJoint.angularXMotion = ConfigurableJointMotion.Free;
        mainJoint.angularZMotion = ConfigurableJointMotion.Free;

        JointDrive xDrive = mainJoint.angularXDrive;
        JointDrive yzDrive = mainJoint.angularYZDrive;
        xDrive.positionSpring = 0.0f;
        yzDrive.positionSpring = 0.0f;
        mainJoint.angularXDrive = xDrive;
        mainJoint.angularYZDrive = yzDrive;

        for (int i = 0; i < syncedObjects.Length; i++)
        {
            syncedObjects[i].Ragdoll();
        }

        isActiveRagdoll = false;

        StartCoroutine(RagdollTimer());
    }

    public void ActiveRagdoll()
    {
        if (isActiveRagdoll)
            return;

        addWeight = true;
        StartCoroutine(StopAddingWeight());

        mainJoint.angularXMotion = ConfigurableJointMotion.Locked;
        mainJoint.angularZMotion = ConfigurableJointMotion.Locked;

        JointDrive xDrive = mainJoint.angularXDrive;
        JointDrive yzDrive = mainJoint.angularYZDrive;
        xDrive.positionSpring = startXPositionSpring;
        yzDrive.positionSpring = startYZPositionSpring;
        mainJoint.angularXDrive = xDrive;
        mainJoint.angularYZDrive = yzDrive;

        for (int i = 0; i < syncedObjects.Length; i++)
        {
            syncedObjects[i].ActiveRagdoll();
        }
        isActiveRagdoll = true;
    }

    void AddWeight()
    {
        float force = 300f;
        rb.AddForce(Vector3.down * force);
    }

    IEnumerator StopAddingWeight()
    {
        yield return new WaitForSeconds(0.6f);
        addWeight = false;
    }

    IEnumerator RagdollTimer()
    {
        float time = 2.5f;

        yield return new WaitForSeconds(time);

        if (!dead)
        {
            ActiveRagdoll();
        }
    }
}
