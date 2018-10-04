using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Text healthLabe;
    [SerializeField] private InventoryPopur popup;

    void Awake()
    {
        Messenger.AddListener(gameEvent.HEALTH_UPDATED, OnHealthUpdated);
    }
    void OnDestroy()
    {
        Messenger.RemoveListener(gameEvent.HEALTH_UPDATED, OnHealthUpdated);
    }
    // Use this for initialization
    void Start()
    {
        OnHealthUpdated();
        popup.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            bool isShowing = popup.gameObject.activeSelf;
            popup.gameObject.SetActive(!isShowing);
            popup.refresh();
        }
    }
    private void OnHealthUpdated()
    {
        string message = "health: " + Managers.player.health + "/" + Managers.player.maxHealth;
        healthLabe.text = message;
    }
}
