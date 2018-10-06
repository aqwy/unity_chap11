using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(playerManager))]
[RequireComponent(typeof(inventoryManager))]
[RequireComponent(typeof(missionManager))]
[RequireComponent(typeof(dataManagers))]
[RequireComponent(typeof(audioManager))]
public class Managers : MonoBehaviour
{
    public static playerManager player { get; private set; }
    public static inventoryManager inventory { get; private set; }
    public static missionManager mission { get; private set; }
    public static dataManagers data { get; private set; }
    public static audioManager audio { get; private set; }
    private List<IGameManager> startSequnce;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        player = GetComponent<playerManager>();
        inventory = GetComponent<inventoryManager>();
        mission = GetComponent<missionManager>();
        audio = GetComponent<audioManager>();
        data = GetComponent<dataManagers>();

        startSequnce = new List<IGameManager>();

        startSequnce.Add(player);
        startSequnce.Add(inventory);
        startSequnce.Add(mission);
        startSequnce.Add(audio);
        startSequnce.Add(data);

        StartCoroutine(startupManagers());
    }
    private IEnumerator startupManagers()
    {
        NetworkService network = new NetworkService();
        foreach (IGameManager mang in startSequnce)
        {
            mang.Startup(network);
        }
        yield return null;

        int numModuls = startSequnce.Count;
        int numReady = 0;

        while (numReady < numModuls)
        {
            int lastReady = numReady;
            numReady = 0;

            foreach (IGameManager managers in startSequnce)
            {
                if (managers.status == managerStatus.Started)
                    numReady++;
            }
            if (numReady > lastReady)
            {
                Debug.Log("progers " + numReady + "/" + numModuls);
                Messenger<int, int>.Broadcast(startUpEvents.MANAGERS_PROGRESS, numReady, numModuls);
            }
            yield return null;
        }
        Debug.Log("All managers started up");
        Messenger.Broadcast(startUpEvents.MANAGERS_STARTED);
    }
}
