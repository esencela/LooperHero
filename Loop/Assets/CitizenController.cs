using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenController : MonoBehaviour, IRagdoll
{
    public float walkSpeed;
    public float lowTurnSpeed;
    public float highTurnSpeed;

    float turnSpeed;

    public Rigidbody rb;
    ConfigurableJoint mainJoint;
    float startXPositionSpring = 0.0f;
    float startYZPositionSpring = 0.0f;
    bool isActiveRagdoll = true;
    bool dead = false;

    public bool isGrounded = false;
    float maxMoveAngle = 80f;

    bool addWeight = false;

    public Animator animator;

    public CustomPath path;
    float pathRange = 1f;

    RaycastHit[] groundHits = new RaycastHit[10];

    CopyMotion[] syncedObjects;

    Vector3 movementDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        syncedObjects = GetComponentsInChildren<CopyMotion>();
        mainJoint = GetComponent<ConfigurableJoint>();

        turnSpeed = lowTurnSpeed;
        startXPositionSpring = mainJoint.angularXDrive.positionSpring;
        startYZPositionSpring = mainJoint.angularYZDrive.positionSpring;
    }

    void Update()
    {
        CheckGround();        

        if (path)
        {
            CheckPath();
            movementDirection = path.GetCurrentPoint().position - transform.position;
        }        

        if (isActiveRagdoll)
        {
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
    }

    void FixedUpdate()
    {
        animator.SetBool("isMoving", false);

        if (!isActiveRagdoll)
            return;

        if (!isGrounded)
            return;

        if (!path)
            return;

        if (path.IsLastPoint())
            return;

        if (Vector3.Distance(transform.position, path.GetCurrentPoint().position) > pathRange)
        {
            Turn();
            Move();
        }
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

    void CheckPath()
    {
        if (Vector3.Distance(transform.position, path.GetCurrentPoint().position) < pathRange)
        {
            path.NextPoint();
        }
    }

    void Move()
    {
        if (!path)
        {
            return;
        }

        if (Vector3.Angle(movementDirection.normalized, rb.transform.forward) > maxMoveAngle)
        {
            turnSpeed = highTurnSpeed;
            return;
        }

        rb.AddForce(transform.forward * walkSpeed, ForceMode.Force);

        animator.SetBool("isMoving", true);
    }

    void Turn()
    {
        if (!path)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(-movementDirection.x, 0.0f, movementDirection.z), Vector3.up);

        mainJoint.targetRotation = Quaternion.RotateTowards(mainJoint.targetRotation, targetRotation, Time.deltaTime * turnSpeed);
    }

    public void Ragdoll(bool kill)
    {
        if (kill)
        {
            GameManager.instance.CitizenDeath();
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
        float time = 3f;

        yield return new WaitForSeconds(time);


        if (!dead)
        {
            ActiveRagdoll();
        }
    }
}
