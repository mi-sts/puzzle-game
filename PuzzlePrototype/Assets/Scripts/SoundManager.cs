using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager manager;

    [SerializeField]
    private List<Sound> sounds = new List<Sound>();

    private Dictionary<string, Sound> soundsDictionary = new Dictionary<string, Sound>();

    private void InitializeSound(Sound sound)
    {
        sound.source = gameObject.AddComponent<AudioSource>();
        sound.source.playOnAwake = false;
        sound.source.clip = sound.clip;
        sound.source.volume = sound.volume;
        sound.source.pitch = sound.pitch;
    }

    private void Awake()
    {
        if (manager == null) manager = this;

        foreach (Sound sound in sounds) {
            InitializeSound(sound);
            soundsDictionary.Add(sound.soundName, sound);
        }
    }

    public void Play(string soundName)
    {
        if (!soundsDictionary.ContainsKey(soundName)) {
            Debug.LogError("The sound does not exist!");
            return;
        }

        soundsDictionary[soundName].source.Play();
    }

    public void Stop(string soundName)
    {
        if (!soundsDictionary.ContainsKey(soundName)) {
            Debug.LogError("The sound does not exist!");
            return;
        }

        soundsDictionary[soundName].source.Stop();
    }
}
