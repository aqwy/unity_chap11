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
    public void UpDateData(int hp, int maxhp)
    {
        health = hp;
        maxHealth = maxhp;
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
        if (health == 0)
        {
            Messenger.Broadcast(gameEvent.LEVEL_FAILED);
        }
        /*Debug.Log("Health:" + health + "/" + maxHealth);*/
        Messenger.Broadcast(gameEvent.HEALTH_UPDATED);
    }
    public void respawn()
    {
        UpDateData(50, 100);
    }
}
