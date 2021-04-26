using Audio;
using UnityEngine;
using UnityEngine.Audio;

namespace Ship
{
    public class CannonBall : MonoBehaviour
    {
        public float Distance;
        public Vector2 Speed;
        public ShipControl OriginShip;
        public float Damage;
        public float CrewDamage;

        public AudioMixerGroup AudioMixerGroup;

        public AudioClip EnemyCannonShotAudio;
        public AudioClip PlayerCannonShotAudio;
        public AudioClip FallInWaterAudio;
        public AudioClip HitAudio;

        private float _travelledDistance;
        private float _speed;
        private SoundManager _soundManager;

        private float _timeUntilSound;
        private bool _playedSound;

        private void Start()
        {
            var rigidbody = GetComponent<Rigidbody2D>();
            rigidbody.velocity = Speed;
            _speed = Speed.magnitude;

            _soundManager = SoundManager.FindByTag();

            _timeUntilSound = Random.Range(0, 0.04F);
            _travelledDistance -= Random.Range(0, 0.5F);
            
            
            if (_timeUntilSound <= 0.01F)
            {
                _playedSound = true;
            
                _soundManager.PlaySound(OriginShip.Team == Team.Player ? PlayerCannonShotAudio : EnemyCannonShotAudio, AudioMixerGroup, pitchMin: 0.95F, pitchMax: 1.05F);
            }
        }

        private void Update()
        {
            if (GlobalGameState.IsUnpaused() && !_playedSound)
            {
                _timeUntilSound -= Time.deltaTime;
                if (_timeUntilSound <= 0)
                {
                    _playedSound = true;
            
                    _soundManager.PlaySound(OriginShip.Team == Team.Player ? PlayerCannonShotAudio : EnemyCannonShotAudio, AudioMixerGroup, pitchMin: 0.95F, pitchMax: 1.05F);
                }
            }
        }

        private void FixedUpdate()
        {
            if (GlobalGameState.IsUnpaused())
            {
                _travelledDistance += _speed * Time.deltaTime;
                if (_travelledDistance >= Distance)
                {
                    _soundManager.PlaySound(FallInWaterAudio, AudioMixerGroup, pitchMin: 0.95F, pitchMax: 1.05F);
                    Destroy(gameObject);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var otherShipControl = ShipControl.FindShipControlInParents(other.gameObject);
            if (otherShipControl != null && otherShipControl != OriginShip)
            {
                _soundManager.PlaySound(HitAudio, AudioMixerGroup, pitchMin: 0.95F, pitchMax: 1.05F);
                otherShipControl.ChangeHullHealth(-Damage);
                otherShipControl.ChangeCrewHealth(-CrewDamage);
                Destroy(gameObject);
            }
        }
    }
}
