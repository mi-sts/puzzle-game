using UnityEngine;

[CreateAssetMenu(fileName = "NewSound", menuName = "Sound", order = 1)]
public class Sound : ScriptableObject 
{
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(0.1f, 3f)]
    public float pitch = 1f;
    public string soundName;

    [HideInInspector]
    public AudioSource source;
}
