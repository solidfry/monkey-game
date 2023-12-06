using Database;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EasyAudioSystem
{
    public class SoundtrackManager : SingletonPersistent<SoundtrackManager>
    {
        public static SoundtrackManager Instance;

        public Sound[] Sounds;

        [SerializeField] private float _soundtrackVolume = 1;

        private Sound _base;
        private Sound _layerOne;
        private Sound _layerTwo;

        private bool _isLayerTwoPlaying = false;

       public override void Awake()
        {
            base.Awake();
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
            _base.source.volume = _soundtrackVolume;    

            _layerOne.source.Play();
            _layerOne.source.volume = 0;

            _layerTwo.source.Play();
            _layerTwo.source.volume = 0;
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
            _layerTwo.source.DOFade(0, 2f).OnComplete(() => _isLayerTwoPlaying = false);
        }

        private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Game")
            {
                // game scene
                _layerOne.source.DOFade(_soundtrackVolume - 0.2f, 3f);
            }

            else if (scene.name == "MainMenu")
            {
                // main menu scene
                _layerOne.source.DOFade(0, 3f);
            }
        }

        private void OnEnable()
        {
            CombinationDatabase.OnCorrectCombination += StartLayerTwoFade;
            SceneManager.sceneLoaded += OnLevelLoaded;
        }
        private void OnDisable()
        {
            CombinationDatabase.OnCorrectCombination -= StartLayerTwoFade;
            SceneManager.sceneLoaded -= OnLevelLoaded;
        }
    }
}