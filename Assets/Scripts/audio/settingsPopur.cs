using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settingsPopur : MonoBehaviour
{
    [SerializeField] private AudioClip sound;
    public void OnPlayMusic(int selector)
    {
        Managers.audio.playSound(sound);
        switch (selector)
        {
            case 1:
                Managers.audio.playIntroMusic();
                break;
            case 2:
                Managers.audio.playLevelMusic();
                break;
            default:
                Managers.audio.stopMusic();
                break;
        }
    }
    public void OnSoundToggle()
    {
        Managers.audio.soundMute = !Managers.audio.soundMute;
        Managers.audio.playSound(sound);
    }
    public void OnMusicToggle()
    {
        Managers.audio.musicMute = !Managers.audio.musicMute;
        Managers.audio.playSound(sound);
    }
    public void OnSoundValue(float volume)
    {
        Managers.audio.soundVolume = volume;
    }
    public void OnMusicValue(float volume)
    {
        Managers.audio.musicVolume = volume;
    }
}
