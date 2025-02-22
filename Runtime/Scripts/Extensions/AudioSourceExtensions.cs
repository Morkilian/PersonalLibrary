using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public static class AudioSourceExtensions
{
    /// <summary>
    /// Plays an AudioClip at a given position in world space.
    /// </summary>
    /// <param name="clip">Audio clip </param>
    /// <param name="position">Position where spawn gameobject who play sfx</param>
    /// <param name="volume">clip volume</param>
    /// <param name="group">Change mixer output</param>
    public static void PlayClipAtPoint
    (AudioClip clip, Vector3 position, float volume = 1.0f, AudioMixerGroup group = null)
    {
        if (clip == null) return;
        GameObject gameObject = new GameObject("One shot audio");
        gameObject.transform.position = position;
        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        if (group != null)
            audioSource.outputAudioMixerGroup = group;
        audioSource.clip = clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = volume;
        audioSource.Play();
        Object.Destroy(gameObject, clip.length * ((Time.timeScale < 0.01f) ? 0.01f : Time.timeScale));
    }
}
