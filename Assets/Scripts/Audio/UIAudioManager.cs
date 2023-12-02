using System;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class UIAudioManager : SingletonPersistent<UIAudioManager>
    {
        [SerializeField] List<AudioList> audioLists = new ();
        [SerializeField] AudioSource audioSource;
        
        public override void Awake()
        {
            base.Awake();
            if (audioSource == null)
            {
                InitialiseAudioSource();
            }
        }
        
        private void OnValidate()
        {
            InitialiseAudioSource();
        }
        
        private void InitialiseAudioSource()
        {
            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }
        }
        
        private void OnEnable() => UIAudioRequester.OnPlayRandomUISoundEvent += PlayRandomUISound;
        
        private void OnDisable() => UIAudioRequester.OnPlayRandomUISoundEvent -= PlayRandomUISound;
        
        private void PlayRandomUISound(string soundListName)
        {
            AudioClip clip = GetRandomAudioClipFromList(soundListName);
            if (clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
        }
        
        private AudioClip GetRandomAudioClipFromList(string name)
        {
            foreach (var audioList in audioLists)
            {
                if (audioList.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return audioList.GetRandomClip();
                }
            }
        
            return null;
        }
        
        [Serializable]
        class AudioList
        {
            [SerializeField] private AudioClipListData clips;
            
            public AudioClip GetRandomClip() => clips.GetRandomClip();
            public string Name => clips.name;
        }
    }
    
    
}