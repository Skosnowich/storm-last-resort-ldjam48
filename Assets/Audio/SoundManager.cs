using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public class SoundManager : MonoBehaviour
    {
        public int MaxAudioSourceCount = 3;
        public float MinimumPlayDuration = 1;
        public float MinRange = 10;
        public float MaxRange = 100;

        public Transform AudioSourcePrefab;

        private Transform _audioSourceGroup;
        private int _audioSourceCount;
        private Dictionary<string, AudioSource> _loopingAudioSources;

        public static SoundManager FindByTag()
        {
            return GameObject.FindWithTag("SoundManager").GetComponent<SoundManager>();
        }

        private void Start()
        {
            _audioSourceGroup = new GameObject("AudioSourceGroup-" + gameObject.name).transform;
            _loopingAudioSources = new Dictionary<string, AudioSource>();
        }

        public void PlaySoundUnmodified(AudioClip audioClip, AudioMixerGroup audioMixerGroup, string loopingIdentifier = null)
        {
            PlaySound(audioClip, audioMixerGroup, 1, 1, 0, 0, MinRange, MaxRange, loopingIdentifier);
        }

        public void PlaySound(AudioClip audioClip, AudioMixerGroup audioMixerGroup, float? minRange = null, float? maxRange = null, float pitchMin = .975f,
            float pitchMax = 1.025f,
            float panMin = -0.05F, float panMax = 0.05F, string loopingIdentifier = null)
        {
            var audioSource = GetFreeAudioSource();
            if (audioSource == null)
            {
                Debug.Log("Keine freie Audioquelle gefunden -> Sound nicht abgespielt.");
                return;
            }

            audioSource.Stop();

            if (loopingIdentifier != null)
            {
                _loopingAudioSources.Add(loopingIdentifier, audioSource);
                audioSource.loop = true;
            }

            audioSource.minDistance = minRange ?? MinRange;
            audioSource.maxDistance = maxRange ?? MaxRange;

            audioSource.clip = audioClip;
            audioSource.outputAudioMixerGroup = audioMixerGroup;

            audioSource.pitch = Random.Range(pitchMin, pitchMax);
            audioSource.panStereo = Random.Range(panMin, panMax);

            audioSource.volume = 1;
            audioSource.Play();
        }

        public void StopLooping(string loopingIdentifier)
        {
            var loopingAudioSource = _loopingAudioSources[loopingIdentifier];
            loopingAudioSource.Stop();
            loopingAudioSource.loop = false;
            _loopingAudioSources.Remove(loopingIdentifier);
        }

        private AudioSource GetFreeAudioSource()
        {
            float latestTime = float.MinValue;
            AudioSource latestAudioSource = null;
            for (int i = 0; i < _audioSourceCount; i++)
            {
                var audioSource = _audioSourceGroup.GetChild(i).GetComponent<AudioSource>();
                if (!audioSource.isPlaying)
                {
                    return audioSource;
                }

                if (!audioSource.loop)
                {
                    var audioSourceTime = audioSource.time;
                    if (audioSourceTime > latestTime && audioSourceTime > MinimumPlayDuration)
                    {
                        latestTime = audioSourceTime;
                        latestAudioSource = audioSource;
                    }
                }
            }

            if (_audioSourceCount < MaxAudioSourceCount)
            {
                _audioSourceCount++;
                var audioSourceParent = Instantiate(AudioSourcePrefab, _audioSourceGroup);
                audioSourceParent.gameObject.SetActive(true);
                return audioSourceParent.GetComponent<AudioSource>();
            }

            return latestAudioSource;
        }
    }
}
