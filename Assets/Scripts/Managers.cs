using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(playerManager))]
[RequireComponent(typeof(inventoryManager))]
public class Managers : MonoBehaviour
{
    public static playerManager player { get; private set; }
    public static inventoryManager inventory { get; private set; }
    private List<IGameManager> startSequnce;
    void Awake()
    {
        player = GetComponent<playerManager>();
        inventory = GetComponent<inventoryManager>();
        startSequnce = new List<IGameManager>();
        startSequnce.Add(player);
        startSequnce.Add(inventory);
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
            }
            yield return null;
        }
        Debug.Log("All managers started up");
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
