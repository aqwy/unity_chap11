using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missionManager : MonoBehaviour, IGameManager
{
    public managerStatus status { get; private set; }
    private NetworkService network;
    public int currLevel { get; private set; }
    public int maxLevel { get; private set; }
    public void Startup(NetworkService service)
    {
        Debug.Log("Mission manager starting...");
        network = service;

        status = managerStatus.Started;
    }
}
