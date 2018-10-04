using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public interface IGameManager
{
    managerStatus status { get; }
    void Startup(NetworkService service);

}

