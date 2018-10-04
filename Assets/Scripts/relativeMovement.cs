using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class relativeMovement : MonoBehaviour
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

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _contact = hit;
        Rigidbody body = hit.collider.attachedRigidbody;
        if(body != null && !body.isKinematic)
        {
            body.velocity = hit.moveDirection * pushForce;
        }
    }

    // Use this for initialization
    void Start()
    {
        theAnumator = GetComponent<Animator>();
        vertSpeed = minFall;
        theCharControl = GetComponent<CharacterController>();
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 movement = Vector3.zero;
        float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        if (horInput != 0 || vertInput != 0)
        {
            movement.x = horInput * moveSpeed;
            movement.z = vertInput * moveSpeed;
            movement = Vector3.ClampMagnitude(movement, moveSpeed);

            Quaternion temp = target.rotation;
            target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
            movement = target.TransformDirection(movement);
            target.rotation = temp;
            /*transform.rotation = Quaternion.LookRotation(movement);*/
            Quaternion direction = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime);
        }

        theAnumator.SetFloat("speed",movement.sqrMagnitude);
        bool hitGround = false;
        RaycastHit hit;
        if (vertSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            float check = (theCharControl.height + theCharControl.radius) / 1.9f;
            hitGround = hit.distance <= check;
        }

        if (hitGround)
        {
            if (Input.GetButtonDown("Jump"))
            {
                vertSpeed = jumpSpeed;
            }
            else
            {
                /*vertSpeed = minFall;*/
                vertSpeed = -0.1f;
                theAnumator.SetBool("jumping", false);
            }
        }
        else
        {
            vertSpeed += gravity * 5 * Time.deltaTime;
            if (vertSpeed < terminalVelocity)
            {
                vertSpeed = terminalVelocity;
            }
            if(_contact != null)
            {
                theAnumator.SetBool("jumping", true);
            }
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

        /*if (theCharControl.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                vertSpeed = jumpSpeed;
            }
            else
            {
                vertSpeed = minFall;
            }
        }
        else
        {
            vertSpeed += gravity * 5 * Time.deltaTime;
            if (vertSpeed < terminalVelocity)
            {
                vertSpeed = terminalVelocity;
            }
        }*/
            
        movement.y = vertSpeed;
        movement *= Time.deltaTime;
        theCharControl.Move(movement);
    }
}
