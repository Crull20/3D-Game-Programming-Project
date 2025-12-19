using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager I;

    [Header("Clips")]
    public AudioClip forestAmbience;
    public AudioClip playerWalk;
    public AudioClip playerRoll;
    public AudioClip playerSlash;
    public AudioClip playerHit;
    public AudioClip enemyWalk;
    public AudioClip enemyInjured;

    [Header("Sources")]
    public AudioSource sfx2D;     // one-shot UI/player global sounds
    public AudioSource ambience;  // looping ambience

    private void Start()
    {
        AudioManager.I.PlayAmbience();
    }
    void Awake()
    {
        if (I != null) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);

        if (!sfx2D) sfx2D = gameObject.AddComponent<AudioSource>();
        sfx2D.spatialBlend = 0f;

        if (!ambience) ambience = gameObject.AddComponent<AudioSource>();
        ambience.spatialBlend = 0f;
        ambience.loop = true;
    }

    public void PlayAmbience()
    {
        if (!forestAmbience) return;
        ambience.clip = forestAmbience;
        ambience.Play();
    }

    public void Play2D(AudioClip clip, float volume = 1f)
    {
        if (!clip) return;
        sfx2D.PlayOneShot(clip, volume);
    }

    public void Play3D(AudioClip clip, Vector3 pos, float volume = 1f)
    {
        if (!clip) return;
        AudioSource.PlayClipAtPoint(clip, pos, volume);
    }
}
