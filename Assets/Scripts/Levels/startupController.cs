using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class startupController : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    void Awake()
    {
        Messenger<int, int>.AddListener(startUpEvents.MANAGERS_PROGRESS, OnManagersProgress);
        Messenger.AddListener(startUpEvents.MANAGERS_STARTED, OnManagersStarted);
    }
    void OnDestroy()
    {
        Messenger<int, int>.RemoveListener(startUpEvents.MANAGERS_PROGRESS, OnManagersProgress);
        Messenger.RemoveListener(startUpEvents.MANAGERS_STARTED, OnManagersStarted);
    }
    private void OnManagersProgress(int numReady, int numModuls)
    {
        float progress = (float)numReady / numModuls;
        progressBar.value = progress;
    }
    private void OnManagersStarted()
    {
        Managers.mission.GoToNext();
    }
}
