using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioClip ShatterSound;
    public AudioClip MoveSound;

    const int AudioSourceCount = 5;

    List<AudioSource> m_audioSources = new List<AudioSource>();

    private int m_audioSourceIndex = 0;


    /// <summary>
    /// Create sources
    /// </summary>
    void Start()
    {
        for(int i = 0; i < AudioSourceCount; i++)
        {
            m_audioSources.Add(gameObject.AddComponent<AudioSource>());
        }
    }

	public void PlaySound(AudioClip clip)
    {
        AudioSource source = m_audioSources[m_audioSourceIndex++];
        if(m_audioSourceIndex >= AudioSourceCount)
        {
            m_audioSourceIndex = 0;
        }

        source.clip = clip;
        source.Play();
    }
}
