using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class dataManagers : MonoBehaviour, IGameManager
{
    public managerStatus status { get; private set; }
    private NetworkService network;
    private string filename;
    public void Startup(NetworkService service)
    {
        Debug.Log("Data managers starting...");
        network = service;
        filename = Path.Combine(Application.persistentDataPath, "game.dat");
        status = managerStatus.Started;
    }
    public void SaveGameState()
    {
        Dictionary<string, object> gameState = new Dictionary<string, object>();
        gameState.Add("inventory", Managers.inventory.GetData());
        gameState.Add("health", Managers.player.health);
        gameState.Add("maxHealth", Managers.player.maxHealth);
        gameState.Add("currLevel", Managers.mission.currLevel);
        gameState.Add("maxLevel", Managers.mission.maxLevel);

        FileStream fileStream = File.Create(filename);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(fileStream, gameState);
        fileStream.Close();
    }
    public void LoadGameState()
    {
        if (!File.Exists(filename))
        {
            Debug.Log("No save files");
            return;
        }
        Dictionary<string, object> gameState = new Dictionary<string, object>();
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Open(filename, FileMode.Open);
        gameState = formatter.Deserialize(stream) as Dictionary<string, object>;
        stream.Close();

        Managers.inventory.UpdateData((Dictionary<string, int>)gameState["inventory"]);
        Managers.player.UpDateData((int)gameState["health"], (int)gameState["maxHealth"]);
        Managers.mission.UpdateData((int)gameState["currLevel"], (int)gameState["maxLevel"]);
        Managers.mission.restartCurrent();
    }
}
