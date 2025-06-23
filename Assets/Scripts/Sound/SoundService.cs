using System;
using UnityEngine;

public class SoundService
{
    private SoundScriptableObject soundScriptableObject;
    private AudioSource audioEffects;

    public SoundService(SoundScriptableObject soundScriptableObject, AudioSource audioEffectSource)
    {
        this.soundScriptableObject = soundScriptableObject;
        audioEffects = audioEffectSource;
    }

    public void PlaySoundEffects(SoundType soundType)
    {
        AudioClip clip = GetSoundClip(soundType);
        if (clip != null)
        {
            audioEffects.clip = clip;
            audioEffects.PlayOneShot(clip);
        }
        else
            Debug.LogError("No Audio Clip selected.");
    }

    private AudioClip GetSoundClip(SoundType soundType)
    {
        Sounds sound = Array.Find(soundScriptableObject.audioList, item => item.soundType == soundType);
        if (sound.audio != null)
            return sound.audio;
        return null;
    }
}