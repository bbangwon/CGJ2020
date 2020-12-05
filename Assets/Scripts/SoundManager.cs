using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonMono<SoundManager>
{
    [SerializeField]
    AudioClip[] audioClips;

    Dictionary<string, AudioClip> dicAudioClip;

    private void Awake()
    {
        dicAudioClip = new Dictionary<string, AudioClip>();
        foreach (var audio in audioClips)
        {

        }
    }

    public void Play(string name)
    {
        if(dicAudioClip.TryGetValue(name, out AudioClip clip))
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        }
    }
}
