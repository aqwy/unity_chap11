using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deviceOperator : MonoBehaviour
{
    public float radius = 1.5f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            Collider[] hitCollider = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider hitColl in hitCollider)
            {
                Vector3 direction = hitColl.transform.position - transform.position;
                if (Vector3.Dot(transform.forward, direction) > 0.5f)
                {
                    hitColl.SendMessage("Operate", SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }
}
