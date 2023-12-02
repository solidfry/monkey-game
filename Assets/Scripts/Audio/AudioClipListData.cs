using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(fileName = "AudioClipListData", menuName = "AudioClipListData", order = 1)]
    public class AudioClipListData : ScriptableObject
    {
        [SerializeField] private List<AudioClip> audioClips;
     
        public AudioClip GetRandomClip() => audioClips[Random.Range(0, audioClips.Count)];
    }
}