using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Audio
{
    public class GameplayOneShotAudioPlayer : MonoBehaviour, IOneShotAudioPlayer
    {
        [SerializeField, Min(1)] private int initialPoolSize = 16;
        [SerializeField, Min(1)] private int maxPoolSize = 32;
        [SerializeField] private AudioSource sourceTemplate;

        private readonly List<AudioSource> _sources = new();

        private void Awake()
        {
            if (sourceTemplate == null)
            {
                Debug.LogError($"{nameof(GameplayOneShotAudioPlayer)} requires source template.", this);
                enabled = false;
                return;
            }
            sourceTemplate.gameObject.SetActive(false);

            if (maxPoolSize < initialPoolSize)
                maxPoolSize = initialPoolSize;

            for (int i = 0; i < initialPoolSize; i++)
            {
                CreateSource();
            }
        }

        public void Play(AudioClip clip, Vector3 position, float volume)
        {
            if (clip == null)
                return;

            var source = GetAvailableSource();
            source.transform.position = position;
            source.clip = clip;
            source.volume = Mathf.Clamp01(volume);
            source.gameObject.SetActive(true);
            source.Play();
        }

        private AudioSource GetAvailableSource()
        {
            for (int i = 0; i < _sources.Count; i++)
            {
                if (!_sources[i].isPlaying)
                    return _sources[i];
            }

            if (_sources.Count < maxPoolSize)
                return CreateSource();

            var fallback = _sources[0];
            fallback.Stop();
            return fallback;
        }

        private AudioSource CreateSource()
        {
            var source = Instantiate(sourceTemplate, transform);
            source.gameObject.SetActive(true);
            source.playOnAwake = false;
            source.loop = false;
            source.clip = null;

            _sources.Add(source);
            return source;
        }
    }
}
