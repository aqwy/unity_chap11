using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class pointClickMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    private CharacterController theCharControl;
    private float vertSpeed;
    private ControllerColliderHit _contact;
    private Animator theAnumator;

    public float rotSpeed = 15.0f;
    public float moveSpeed = 6.0f;
    public float jumpSpeed = 15.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10.0f;
    public float minFall = -1.5f;
    public float pushForce = 3.0f;
    public float decelartion = 20f;
    public float targetBuffer = 1.5f;

    private float currentSpeed = 0f;
    private Vector3 targetPos = Vector3.one;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _contact = hit;
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body != null && !body.isKinematic)
        {
            body.velocity = hit.moveDirection * pushForce;
        }
    }
    void Start()
    {
        theAnumator = GetComponent<Animator>();
        vertSpeed = minFall;
        theCharControl = GetComponent<CharacterController>();
    }
    void Update()
    {
        Vector3 movement = Vector3.zero;

        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;
            if (Physics.Raycast(ray, out mouseHit))
            {
                GameObject hitObj = mouseHit.transform.gameObject;
                if (hitObj.layer == LayerMask.NameToLayer("Ground"))
                {
                    targetPos = mouseHit.point;
                    currentSpeed = moveSpeed;
                }
            }
        }
        if (targetPos != Vector3.one)
        {
            Vector3 adjustedPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
            Quaternion targetRot = Quaternion.LookRotation(adjustedPos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotSpeed * Time.deltaTime);

            movement = currentSpeed * Vector3.forward;
            movement = transform.TransformDirection(movement);

            if (Vector3.Distance(targetPos, transform.position) < targetBuffer)
            {
                currentSpeed -= decelartion * Time.deltaTime;
                if (currentSpeed <= 0)
                {
                    targetPos = Vector3.one;
                }
            }
        }
        theAnumator.SetFloat("speed", movement.sqrMagnitude);
        // raycast down to address steep slopes and dropoff edge
        bool hitGround = false;
        RaycastHit hit;
        if (vertSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            float check = (theCharControl.height + theCharControl.radius) / 1.9f;
            hitGround = hit.distance <= check;  // to be sure check slightly beyond bottom of capsule
        }

        // y movement: possibly jump impulse up, always accel down
        // could _charController.isGrounded instead, but then cannot workaround dropoff edge
        if (hitGround)
        {
            // commented out lines remove jump control
            //if (Input.GetButtonDown("Jump")) {
            //	_vertSpeed = jumpSpeed;
            //} else {
            vertSpeed = minFall;
            theAnumator.SetBool("jumping", false);
            //}
        }
        else
        {
            vertSpeed += gravity * 5 * Time.deltaTime;
            if (vertSpeed < terminalVelocity)
            {
                vertSpeed = terminalVelocity;
            }
            if (_contact != null)
            {   // not right at level start
                theAnumator.SetBool("jumping", true);
            }

            // workaround for standing on dropoff edge
            if (theCharControl.isGrounded)
            {
                if (Vector3.Dot(movement, _contact.normal) < 0)
                {
                    movement = _contact.normal * moveSpeed;
                }
                else
                {
                    movement += _contact.normal * moveSpeed;
                }
            }
        }
        movement.y = vertSpeed;

        movement *= Time.deltaTime;
        theCharControl.Move(movement);
    }
}
