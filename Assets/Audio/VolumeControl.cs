using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class VolumeControl : MonoBehaviour
    {
        private const float MinVolume = -60;
        private const float MaxVolume = 20;
        private const string MasterVolumeName = "MasterVolume";
        private const string MusicVolumeName = "MusicVolume";
        private const string SoundVolumeName = "SoundsVolume";

        public Slider MasterSlider;
        public Slider MusicSlider;
        public Slider SoundSlider;
        public AudioMixer AudioMixer;

        private AudioSource _audioSource;
        private bool _initialized;

        private void Start()
        {
            InitSlider(MasterSlider, MasterVolumeName);
            InitSlider(MusicSlider, MusicVolumeName);
            InitSlider(SoundSlider, SoundVolumeName);
            _audioSource = GetComponent<AudioSource>();
            _audioSource.ignoreListenerPause = true;
            _initialized = true;
        }

        private void InitSlider(Slider slider, string volumeParameterName)
        {
            slider.minValue = MinVolume;
            slider.maxValue = MaxVolume;
            AudioMixer.GetFloat(volumeParameterName, out var volume);
            slider.value = volume;
        }

        public void OnMasterSliderVolumeChanged()
        {
            if (_initialized)
            {
                OnSliderVolumeChanged(MasterSlider, MasterVolumeName);
                PlayTestSound();
            }
        }

        public void OnMusicSliderVolumeChanged()
        {
            if (_initialized)
            {
                OnSliderVolumeChanged(MusicSlider, MusicVolumeName);
            }
        }

        public void OnSoundSliderVolumeChanged()
        {
            if (_initialized)
            {
                OnSliderVolumeChanged(SoundSlider, SoundVolumeName);
                PlayTestSound();
            }
        }

        private void PlayTestSound()
        {
            if (!_audioSource.isPlaying || _audioSource.time > 0.5F)
            {
                _audioSource.Play();
            }
        }

        private void OnSliderVolumeChanged(Slider slider, string volumeParameterName)
        {
            var volume = slider.value;
            if (volume <= MinVolume)
            {
                volume = -80;
            }

            AudioMixer.SetFloat(volumeParameterName, volume);
        }
    }
}
