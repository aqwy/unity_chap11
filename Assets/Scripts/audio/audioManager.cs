using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour, IGameManager
{
    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioSource music1source;
    [SerializeField] private AudioSource music2source;
    [SerializeField] private string introBGMusic;
    [SerializeField] private string levelBGMusic;
    private AudioSource activeMusic;
    private AudioSource inactiveMusic;
    public float crossFadeRate = 1.5f;
    private bool crossFadnig;
    public managerStatus status { get; private set; }
    private NetworkService network;
    private float theMusicVolume;
    public float musicVolume
    {
        get { return theMusicVolume; }
        set
        {
            theMusicVolume = value;
            if (music1source != null && !crossFadnig)
            {
                music1source.volume = theMusicVolume;
                music2source.volume = theMusicVolume;
            }
        }
    }
    public bool musicMute
    {
        get
        {
            if (music1source != null)
            {
                return music1source.mute;
            }
            return false;
        }
        set
        {
            if (music1source != null)
            {
                music1source.mute = value;
                music2source.mute = value;
            }
        }
    }
    public void playIntroMusic()
    {
        playMusic(Resources.Load("Music/" + introBGMusic) as AudioClip);
    }
    public void playLevelMusic()
    {
        playMusic(Resources.Load("Music/" + levelBGMusic) as AudioClip);
    }
    private void playMusic(AudioClip clip)
    {
        /* music1source.clip = clip;
         music1source.Play();*/
        if (crossFadnig)
            return;
        StartCoroutine(crossFadeMusic(clip));
    }
    public void stopMusic()
    {
        /*music1source.Stop();*/
        activeMusic.Stop();
        inactiveMusic.Stop();
    }
    public void playSound(AudioClip clip)
    {
        soundSource.PlayOneShot(clip);
    }
    public float soundVolume
    {
        get { return AudioListener.volume; }
        set { AudioListener.volume = value; }
    }
    public bool soundMute
    {
        get { return AudioListener.pause; }
        set { AudioListener.pause = value; }
    }
    public void Startup(NetworkService service)
    {
        Debug.Log("audio manager starting...");
        network = service;

        music1source.ignoreListenerVolume = true;
        music1source.ignoreListenerPause = true;
        music2source.ignoreListenerVolume = true;
        music2source.ignoreListenerPause = true;

        soundVolume = 1f;
        musicVolume = 1f;

        activeMusic = music1source;
        inactiveMusic = music2source;

        status = managerStatus.Started;
    }
    /*private void PlayMusic(AudioClip clip)
    {
        if (crossFadnig)
            return;
        StartCoroutine(crossFadeMusic(clip));
    }*/
    private IEnumerator crossFadeMusic(AudioClip clip)
    {
        crossFadnig = true;
        inactiveMusic.clip = clip;
        inactiveMusic.volume = 0;
        inactiveMusic.Play();

        float scaledRate = crossFadeRate * theMusicVolume;
        while (activeMusic.volume > 0)
        {
            activeMusic.volume -= scaledRate * Time.deltaTime;
            inactiveMusic.volume += scaledRate * Time.deltaTime;
            yield return null;
        }

        AudioSource temp = activeMusic;
        activeMusic = inactiveMusic;
        activeMusic.volume = musicVolume;

        inactiveMusic = temp;
        inactiveMusic.Stop();

        crossFadnig = false;
    }
}
