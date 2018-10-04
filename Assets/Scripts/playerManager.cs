using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerManager : MonoBehaviour, IGameManager
{
    public managerStatus status { get; private set; }
    public int health { get; private set; }
    public int maxHealth { get; private set; }
    private NetworkService theNetwork;
    public void Startup(NetworkService service)
    {
        Debug.Log("Player manager starting...");
        theNetwork = service;
        health = 50;
        maxHealth = 100;
        status = managerStatus.Started;
    }
    public void changeHealth(int val)
    {
        health += val;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        else if (health < 0)
        {
            health = 0;
        }
        /*Debug.Log("Health:" + health + "/" + maxHealth);*/
        Messenger.Broadcast(gameEvent.HEALTH_UPDATED);
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
