using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Klassen AudioData indeholder alle de data som audiomanageren skal bruge når den skal spille et givent audioClip fra listen
[System.Serializable]
public class AudioData
{
    public string title;
    public AudioClip clip;
    public float volume = 1f;
    public bool loop;
    public float pitch;
}


public class AudioManager : MonoBehaviour
{
    // Vi instancierer audioManageren, idet vi kun skal bruge én audioManager af gangen
    public static AudioManager instance;
    public List<AudioData> audioDatas = new List<AudioData>();
    private List<AudioSource> audioSources = new List<AudioSource>();

    public void Awake()
    {
        instance = this;
    }



    // PlayAudio er den funktion andre scripts kalder, når de skal spille et spcifikt audioClip
    public static void PlayAudio(string title)
    {
        try
        {
            instance.InitAudio(title);
        }
        catch (InvalidCastException e)
        {
            Debug.LogWarning(e);
        }

    }

    void InitAudio(string title)
    {
        //Vi finder audioDataen med titlen
        AudioData audioData = GetAudioData(title);

        if (audioData != null)
        {
            //Udfra audioclippet, finder vi audiosourcen og giver den resten af dataen fra audioData-listen
            AudioSource audioSource = GetAudioSource();
            audioSource.clip = audioData.clip;

            audioSource.volume = audioData.volume;

            audioSource.loop = audioData.loop;

            audioSource.pitch = audioData.pitch;

            //Når alt er sat op, afspilles lyden så
            audioSource.Play();

        }
        else
        {
            Debug.Log("Can't find audio with title: " + title);
        }

    }

    //Denne method bliver kaldt, når vi gerne vil stoppe en lyd
    public static void StopAudio(string title)
    {
        //Vi finder audioDataen med titlen
        AudioData audioData = instance.GetAudioData(title);

        if (audioData != null)
        {
            // Hvis der findes en audioData med titlen, prøver vi at finde en audioSorce i listen audioSouces, 
            // der indeholder det samme klip som den givne audioData
            AudioSource audioSource = instance.audioSources.Find(nextAudioSource => nextAudioSource.clip == audioData.clip);

            // Hvis vi kunne finde en audioSouce og den stadig er igang med at afspille, stoppes den
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    //Her gennemgår vi listen og finder den første audioData med den valgte titel
    AudioData GetAudioData(string title)
    {
        AudioData audioData = audioDatas.Find(nextAudioData => nextAudioData.title == title);

        return audioData;
    }

    // Her gennemgår vi listen og finder den første audiosorce der ikke er igang med at afspille, hvis den kan findes, returneres den
    // Hvis den ikke kan findes, laver vi en ny audioSource og returnerer den i stedet.
    AudioSource GetAudioSource()
    {
        AudioSource audioSource = audioSources.Find(nextAudioSource => !nextAudioSource.isPlaying);

        if (audioSource != null)
        {
            return audioSource;
        }
        else
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSources.Add(audioSource);
            return audioSource;
        }
    }
}
