using System;
using UnityEngine;

namespace Audio
{
    public class UIAudioRequester : MonoBehaviour
    {
        [SerializeField] AudioClipListData list;
        public static event Action<string> OnPlayRandomUISoundEvent;
        public void Request(AudioClipListData list) => OnPlayRandomUISoundEvent?.Invoke(list.name);
    }
}