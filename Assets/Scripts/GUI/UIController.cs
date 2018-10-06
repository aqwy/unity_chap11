using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Text healthLabe;
    [SerializeField] private InventoryPopur popup;
    [SerializeField] private Text levelEnding;
    [SerializeField] private settingsPopur stngPopur;
    void Awake()
    {
        Messenger.AddListener(gameEvent.HEALTH_UPDATED, OnHealthUpdated);
        Messenger.AddListener(gameEvent.LEVEL_COMPLETE, OnLevelComplete);
        Messenger.AddListener(gameEvent.LEVEL_FAILED, OnLevelFailed);
        Messenger.AddListener(gameEvent.GAME_COMPLETE, OnGameComplete);
    }
    void OnDestroy()
    {
        Messenger.RemoveListener(gameEvent.HEALTH_UPDATED, OnHealthUpdated);
        Messenger.RemoveListener(gameEvent.LEVEL_COMPLETE, OnLevelComplete);
        Messenger.RemoveListener(gameEvent.LEVEL_FAILED, OnLevelFailed);
        Messenger.RemoveListener(gameEvent.GAME_COMPLETE, OnGameComplete);
    }
    // Use this for initialization
    void Start()
    {
        OnHealthUpdated();

        levelEnding.gameObject.SetActive(false);
        popup.gameObject.SetActive(false);
        stngPopur.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            bool isShowing = popup.gameObject.activeSelf;
            popup.gameObject.SetActive(!isShowing);
            popup.refresh();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            bool isShowing = stngPopur.gameObject.activeSelf;
            stngPopur.gameObject.SetActive(!isShowing);
            /*if (isShowing)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }*/
        }
    }
    private void OnHealthUpdated()
    {
        string message = "health: " + Managers.player.health + "/" + Managers.player.maxHealth;
        healthLabe.text = message;
    }
    private void OnGameComplete()
    {
        levelEnding.gameObject.SetActive(true);
        levelEnding.text = "You Fineshed the Game!";
    }
    private void OnLevelComplete()
    {
        StartCoroutine(CompliteLevel());
    }
    private IEnumerator CompliteLevel()
    {
        levelEnding.gameObject.SetActive(true);
        levelEnding.text = "Level Ending!";
        yield return new WaitForSeconds(2);
        Managers.mission.GoToNext();
    }
    private IEnumerator FailedLevel()
    {
        levelEnding.gameObject.SetActive(true);
        levelEnding.text = "Failed";
        yield return new WaitForSeconds(2);
        Managers.player.respawn();
        Managers.mission.restartCurrent();
    }
    private void OnLevelFailed()
    {
        StartCoroutine(FailedLevel());
    }
    public void SaveGame()
    {
        Managers.data.SaveGameState();
    }
    public void LoadGame()
    {
        Managers.data.LoadGameState();
    }
}
