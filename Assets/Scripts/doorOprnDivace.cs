using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorOprnDivace : MonoBehaviour
{
    [SerializeField] private Vector3 dpos;
    private bool dopen;

    public void Activate()
    {
        if (!dopen)
        {
            Vector3 pos = transform.position + dpos;
            transform.position = pos;
            dopen = true;
        }
    }
    public void Deactivate()
    {
        if(dopen)
        {
            Vector3 pos = transform.position - dpos;
            transform.position = pos;
            dopen = false;
        }
    }
    public void Operate()
    {
        if (dopen)
        {
            Vector3 pos = transform.position - dpos;
            transform.position = pos;
        }
        else
        {
            Vector3 pos = transform.position + dpos;
            transform.position = pos;
        }
        dopen = !dopen;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
