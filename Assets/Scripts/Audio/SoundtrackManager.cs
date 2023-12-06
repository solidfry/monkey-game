using Database;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EasyAudioSystem
{
    public class SoundtrackManager : MonoBehaviour
    {
        public static SoundtrackManager Instance;

        public Sound[] Sounds;

        [SerializeField] private float _soundtrackVolume = 1;

        private Sound _base;
        private Sound _layerOne;
        private Sound _layerTwo;

        private bool _isLayerTwoPlaying = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);

            foreach (Sound s in Sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
            _base = Sounds[0];
            _layerOne = Sounds[1];
            _layerTwo = Sounds[2];

            _base.source.Play();

            _layerOne.source.Play();
            _layerOne.source.volume = 0;

            _layerTwo.source.Play();
            _layerTwo.source.volume = 0;

        }
        
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            CombinationDatabase.OnCorrectCombination += StartLayerTwoFade;
        }
        
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            CombinationDatabase.OnCorrectCombination -= StartLayerTwoFade;
        }

        public void Play(string name)
        {
            Sound s = Array.Find(Sounds, sound => sound.name == name);
            s.source.Play();
        }

        public void Stop()
        {
            foreach(Sound s in Sounds)
            {
                s.source.Stop();
            }
        }

        private void StartLayerTwoFade()
        {
            if (_isLayerTwoPlaying) return;
            _layerTwo.source.DOFade(_soundtrackVolume, 2f).OnComplete(() => StartCoroutine(CompleteLayerTwoFade()));
        }

        private IEnumerator CompleteLayerTwoFade()
        {
            yield return new WaitForSeconds(4);
            _layerTwo.source.DOFade(0, 2f).OnComplete(() => _isLayerTwoPlaying = false); ;
        }
        
        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            switch (arg0.buildIndex)
            {
                case 1:
                    // game scene
                    _layerOne.source.DOFade(_soundtrackVolume, 3f);
                    break;
                case 0:
                    // main menu scene
                    _layerOne.source.DOFade(0, 3f);
                    break;
            }
        }
        
    }
}