using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectiveTriggered : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        Managers.mission.ReachObjective();
    }
}
