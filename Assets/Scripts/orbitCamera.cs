using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orbitCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    public float rotSpeed = 1.5f;
    private float rotY;
    private Vector3 offset;

    void LateUpdate()
    {
        /*float horInput = Input.GetAxis("Horizontal");
        if(horInput != 0)
        {
            rotY += horInput * rotSpeed;
        }
        else
        {
            rotY += Input.GetAxis("Mouse X") * rotSpeed * 3;
        }*/
        rotY -= Input.GetAxis("Horizontal") * rotSpeed;
        Quaternion rotation = Quaternion.Euler(0, rotY, 0);
        transform.position = target.position - (rotation * offset);
        transform.LookAt(target);
    }

    // Use this for initialization
    void Start()
    {
        rotY = transform.eulerAngles.y;
        offset = target.position - transform.position;
    }

    // Update is called once per frame
    /*void Update () {
		
	}*/
}
