using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ2020
{
    public class SoundManager : SingletonMono<SoundManager>
    {
        [SerializeField]
        AudioClip[] audioClips;

        Dictionary<string, AudioClip> dicAudioClip;

        public enum AudioTypes
        {
            Bannerman_Die,
            Rock_Hit
        }

        private void Awake()
        {
            dicAudioClip = new Dictionary<string, AudioClip>();
            foreach (var audio in audioClips)
            {
                dicAudioClip.Add(audio.name, audio);
            }
        }

        public void Play(AudioTypes audioType)
        {
            if (dicAudioClip.TryGetValue(audioType.ToString(), out AudioClip clip))
            {
                AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
            }
        }
    }

}