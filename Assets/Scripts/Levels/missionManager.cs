using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        UpdateData(0, 3);
        /*currLevel = 0;
        maxLevel = 1;*/
        status = managerStatus.Started;
    }
    public void GoToNext()
    {
        if (currLevel < maxLevel)
        {
            currLevel++;
            string level = "Level" + currLevel;
            Debug.Log("Loading..." + level);
            SceneManager.LoadScene(level);
        }
        else
        {
            Debug.Log("Last level");
            Messenger.Broadcast(gameEvent.GAME_COMPLETE);
        }
    }
    public void ReachObjective()
    {
        Messenger.Broadcast(gameEvent.LEVEL_COMPLETE);
    }
    public void restartCurrent()
    {
        string name = "Level" + currLevel;
        Debug.Log("Loading " + name);
        SceneManager.LoadScene(name);
    }
    public void UpdateData(int currlvl, int maxlvl)
    {
        currLevel = currlvl;
        maxLevel = maxlvl;
    }
}
